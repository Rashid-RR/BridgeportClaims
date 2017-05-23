
import {UUID} from "angular2-uuid";
import * as Immutable from "immutable";
import {Observable} from "rxjs/Observable";
import {UserProfile} from "../models/profile" 
import {Injectable} from "@angular/core";
import {HttpService} from "./http-service";
import {EventsService} from "./events-service";
  
@Injectable()
export class ProfileManager{
  private userCache: Immutable.OrderedMap<UUID, UserProfile> = Immutable.OrderedMap<UUID, UserProfile>();
  profile: UserProfile = null;
 
  constructor(private http:HttpService,private events:EventsService) {
      this.events.on("profile",(profile)=>{
        this.profile = profile as UserProfile;
      });
      this.events.on("logout",(v)=>{
        this.clearUsers();
    });
  }
  userInfo(userId: UUID): Observable<UserProfile> {
    let v = this.userCache.get(userId);
    if (v) {
      return Observable.of(v);
    } else {         
      let s = this.http.userFromId(userId);
      s.subscribe(res => {
        let u = res.json() as UserProfile;
        this.userCache = this.userCache.set(u.id, u);
      });
      return s.map(res => res.json() as UserProfile);
    }
  }
  setProfile(u:UserProfile){
    let profile = new UserProfile(u.id,u.login,u.displayName,u.email,u.avatarUrl,u.createdOn);
    this.userCache = this.userCache.set(u.id, profile);
  }
  userProfile(userId: UUID){
      return this.userCache.get(userId);
  }
  
  clearUsers(){
    this.userCache = Immutable.OrderedMap<UUID, UserProfile>();
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
