import {AfterViewInit,Renderer2, Component,OnDestroy, OnInit} from "@angular/core";
import {Http,Headers} from "@angular/http";
import { Router,NavigationEnd } from '@angular/router';

 


import {HttpService} from "./services/http-service";
import {ProfileManager} from "./services/profile-manager";
import {UserProfile} from "./models/profile";
import {EventsService} from "./services/events-service";


  @Component({
  selector: 'app-root',
  template: `<router-outlet></router-outlet>`
})
export class AppComponent implements OnInit,OnDestroy {
 
  constructor(private http:HttpService,private events: EventsService,private profileManager:ProfileManager) {
     
  }
  
 ngOnDestroy(){
    
  }
  ngOnInit() {
    var user = localStorage.getItem("user");
    if (user !== null && user.length > 0) {
      try {
        let us = JSON.parse(user);
        //this.events.broadcast('profile', us);
          this.http.setAuth(us.access_token);
          let profile = new UserProfile(us.id || us.email,us.email,us.firstName || us.email,us.lastName  || us.email,us.email  || us.email,us.email,us.avatarUrl,us.createdOn,us.roles);
          this.profileManager.setProfile(profile);
          this.profileManager.profile = profile;
          let auth = localStorage.getItem("token");
          /*this.profileManager.userInfo(us.userName).single().subscribe( res => {
          this.profileManager.profile= res; 
          this.events.broadcast('profile', res);
         },(error)=>{
           
         });*/
      } catch (error) {
        console.log(error);
      }
    }
    this.events.on("logout",(v)=>{
      this.profileManager.clearUsers();
      this.profileManager.profile=undefined;
    }) 
    
  }
}
