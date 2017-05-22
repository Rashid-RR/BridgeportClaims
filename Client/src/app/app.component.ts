import {AfterViewInit,Renderer2, Component,OnDestroy, OnInit} from "@angular/core";
import {Http,Headers} from "@angular/http";
import { Router,NavigationEnd } from '@angular/router';
import {ChatService,HttpService, EventsService, ProfileManager, StoragePropService} from "web-common";
import {BrowserPermissionService} from "./providers/browser-permission.service";
@Component({
  selector: 'app-root',
  template: `<router-outlet></router-outlet>`
})
export class AppComponent implements OnInit,OnDestroy {
 
  private hashChange:Function;
  constructor(private renderer: Renderer2,private router: Router,private req:Http,private http:HttpService,private storage:StoragePropService,private chatService: ChatService,private eventservice: EventsService,private profileManager:ProfileManager,private browserPermission: BrowserPermissionService ) {
    this. hashChange = this.renderer.listen('window', 'urlHashChange', (event) => {
        let url:string = event['url'];
        if(event['app'] !=='main-app'  && !this.router.isActive(url,false)){
          console.log("App 2 URL was not active");  
          this.router.navigateByUrl(event['url']);
        }
      });
  }
 ngOnDestroy(){
    this.hashChange();
  }
  ngOnInit() {
    var user = localStorage.getItem("user");
    if (user !== null && user.length > 0) {
      try {
        let us = JSON.parse(user);
        //this.eventservice.broadcast('profile', us);
        this.profileManager.userInfo(us.id).single().subscribe( res => {
          this.profileManager.profile= res; 
          this.eventservice.broadcast('profile', res);
         },(error)=>{
           
         });
      } catch (error) {
        console.log(error);
      }
    }
    console.log(this.storage.space);
    this.eventservice.on("logout",(v)=>{
      this.chatService.clearChat();
      this.profileManager.clearUsers();
      this.profileManager.profile=undefined;
    });     
   this.router.events.subscribe((event) => {
       if( event instanceof NavigationEnd /*&& !this.router.isActive(event.url,true)*/) {
            var wevent = document.createEvent('Event');
            wevent.initEvent('urlHashChange', true, true);
            wevent['url']=event.url;
            wevent['app']='main-app';
            window.dispatchEvent(wevent);
        }
    });
  }
  

}
