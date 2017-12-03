import {Component, OnInit, ViewChild, ViewContainerRef, AfterViewInit} from "@angular/core";
import {Router,NavigationEnd} from "@angular/router";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import {ProfileManager} from "../services/profile-manager"
import {AuthGuard} from "../services/auth.guard"
import {LocalStorageService} from 'ng2-webstorage';
import { EventsService } from "../services/events-service";

@Component({
  selector: 'app-layout',
  templateUrl: "./app-layout.component.html",
  styleUrls: ["./app-layout.component.css"]
})
export class AppLayoutComponent implements OnInit, AfterViewInit {
  buildSha: '';
  buildDate: '';
  currentURL ='';
  @ViewChild('toastContainer', { read: ViewContainerRef }) toastVcr: ViewContainerRef;
  constructor(
    private router: Router,
    private profileManager:ProfileManager,
    private toast: ToastsManager,
    private guard:AuthGuard,
    private events: EventsService,
    private  localSt:LocalStorageService
  ) {

  }

  ngOnInit() {
    var sideBarStatus = this.localSt.retrieve("sidebarOpen");
    sideBarStatus == null ? true : sideBarStatus;
    this.adjustSideBar(!sideBarStatus);
    this.localSt.observe("sidebarOpen")
    .subscribe((value) =>{
       this.adjustSideBar(value);
    });
    this.events.on('login', ()=>{
     var sideBarStatus = this.localSt.retrieve("sidebarOpen");
      sideBarStatus == null ? true : sideBarStatus;
      this.adjustSideBar(sideBarStatus);
    });
    this.events.on('logout', ()=>{
      var sideBarStatus = this.localSt.retrieve("sidebarOpen");
      sideBarStatus == null ? true : sideBarStatus;
      this.adjustSideBar(sideBarStatus);
    });
    this.events.on('sidebarOpen', ()=>{
      var st = document.body.classList;
       if(st.contains('sidebar-collapse')){
        this.localSt.store("sidebarOpen",false);
      }else{
        this.localSt.store("sidebarOpen",true);        
      }
    });
    
    this.currentURL = this.router.url; 
      this.router.events.subscribe(ev=>{
        if(ev instanceof NavigationEnd){ 
          this.currentURL = this.router.url;          
        }
      });
  }
  adjustSideBar(status){
    this.guard.isLoggedIn.single().subscribe(r=>{
       if(!r){
        window['jQuery']('body').removeClass('sidebar-mini');
        window['jQuery']('body').addClass('sidebar-collapse');
      }else{
        var st = document.body.classList;
        if(!st.contains('sidebar-mini')){
          window['jQuery']('body').addClass('sidebar-mini');
        }
        if(!status){
          window['jQuery']('body').addClass('sidebar-collapse');
        }else{
            window['jQuery']('body').removeClass('sidebar-collapse'); 
        }
      }            
    },err=>{});    
  }
  get isLoggedIn():boolean{
    if(this.profileManager.profile){
       return true;
    }else{
        return false;
    }
  }
  ngAfterViewInit() {
    this.toast.setRootViewContainerRef(this.toastVcr);
  }

}
