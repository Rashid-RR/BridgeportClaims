import {AfterViewInit,Renderer2, Component,OnDestroy, OnInit, ViewContainerRef} from "@angular/core";
import {Http,Headers} from "@angular/http";
import { Router,NavigationEnd,ActivatedRoute } from '@angular/router';
import { ToastsManager,Toast } from 'ng2-toastr/ng2-toastr';

import {HttpService} from "./services/services.barrel";
import {ProfileManager} from "./services/profile-manager";
import {UserProfile} from "./models/profile";
import {EventsService} from "./services/events-service";


  @Component({
  selector: 'app-root',
  template: `<router-outlet></router-outlet>`
})
export class AppComponent implements OnInit, OnDestroy {
 
  activeToast:Toast;
  t:any;
  constructor(
    private http:HttpService,
    private events: EventsService,
    private profileManager: ProfileManager,
    private toast: ToastsManager,
    private vcr: ViewContainerRef,private route:ActivatedRoute,
    public viewContainerRef:ViewContainerRef
  ) {
    this.toast.setRootViewContainerRef(vcr);
  }
  
 ngOnDestroy(){
    
  }
  ngOnInit() {
    var user = localStorage.getItem("user");
    /* setTimeout(()=>{
      this.t = this.toast.warning('Please select at least one prescription 1',null,{maxShown:1,toastLife:10000}).then((toast:Toast)=>{
        this.activeToast = toast;
    })},1000);
   
    this.t = this.toast.info('Please select at least one prescription 2',null,{maxShown:1,toastLife:10000}).then((toast:Toast)=>{
        this.activeToast = toast;
    })
    
    this.t = this.toast.error('Please select at least one prescription 3',null,{maxShown:1,toastLife:10000}).then((toast:Toast)=>{
        this.activeToast = toast;
    }) */
    if (user !== null && user.length > 0) {
      try {
        let us = JSON.parse(user);
        //this.events.broadcast('profile', us);
          this.http.setAuth(us.access_token);
          let profile = new UserProfile(us.id || us.email,us.email,us.firstName || us.email,us.lastName  || us.email,us.email  || us.email,us.email,us.avatarUrl,us.createdOn,us.roles);
          this.profileManager.setProfile(profile);
          this.profileManager.profile = profile;
          let auth = localStorage.getItem("token");
          if(window.location.hash.indexOf("#/confirm-email")!==0){         
            this.http.userFromId(us.id).single().subscribe( res => {
                //console.log(res);
                this.profileManager.profile.roles = res.json().roles;
            },(error)=>{
              //console.log(error)
            });
          }
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
