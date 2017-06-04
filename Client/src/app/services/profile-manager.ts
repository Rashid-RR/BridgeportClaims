
import {UUID} from "angular2-uuid";
import * as Immutable from "immutable";
import {Observable} from "rxjs/Observable";
import {UserProfile} from "../models/profile" 
import {Injectable} from "@angular/core";
import {HttpService} from "./http-service";
import {EventsService} from "./events-service";
  
@Injectable()
export class ProfileManager{
  private userCache: Immutable.OrderedMap<String, UserProfile> = Immutable.OrderedMap<String, UserProfile>();
  profile: UserProfile = null;
 
  constructor(private http:HttpService,private events:EventsService) {
      this.events.on("profile",(profile)=>{
        this.profile = profile as UserProfile;
      });
      this.events.on("logout",(v)=>{
        this.clearUsers();
    });
  }
  userInfo(userId: String): Observable<UserProfile> {
    let v = this.userCache.get(userId);
    if (v) {
      return Observable.of(v);
    } else {         
      let s = this.http.userFromId(userId);
      s.subscribe(res => {
        let u = res.json() as UserProfile;
        this.userCache = this.userCache.set(u.userName, u);
      });
      return s.map(res => res.json() as UserProfile);
    }
  }
  setProfile(u:UserProfile){
    let profile = new UserProfile(u.id || u.userName,u.login  || u.userName,u.displayName  || u.userName,u.email  || u.userName,u.userName,u.avatarUrl,u.createdOn);
     this.userCache = this.userCache.set(profile.userName, profile);
  }
  userProfile(userId: String){
      return this.userCache.get(userId);
  }
  
  clearUsers(){
    this.userCache = Immutable.OrderedMap<String, UserProfile>();
  }
  get User():Observable<UserProfile> {
    const user = localStorage.getItem("user");
    return Observable.create((observer)=>{
        if (user !== null && user.length > 0) {
          try {
            let us = JSON.parse(user);
            //this.eventservice.broadcast('profile', us);
            this.userInfo(us.id).single().subscribe( res => {
              this.profile= res; 
              this.events.broadcast('profile', res);
              observer.next(res);
            },(error)=>{              
                observer.error();
            })
          } catch (error) {
            console.log(error);
          }
        }else{
          observer.error();
        }
    });
  }
}
