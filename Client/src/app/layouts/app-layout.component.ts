import {Component, OnInit, ViewChild, ViewContainerRef, AfterViewInit} from "@angular/core";
import {Router} from "@angular/router";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import {ProfileManager} from "../services/profile-manager"
@Component({
  selector: 'app-layout',
  templateUrl: "./app-layout.component.html",
  styleUrls: ["./app-layout.component.css"]
})
export class AppLayoutComponent implements OnInit, AfterViewInit {
  buildSha: '';
  buildDate: '';

  @ViewChild('toastContainer', { read: ViewContainerRef }) toastVcr: ViewContainerRef;

  constructor(
    private router: Router,
    private profileManager:ProfileManager,
    private toast: ToastsManager
  ) { 
    
  }

  ngOnInit() {
 
  }
   ngAfterViewInit() {
    this.toast.setRootViewContainerRef(this.toastVcr);
 }

   get isLoggedIn():boolean{
    if(this.profileManager.profile){
       // window['jQuery']('body').addClass('sidebar-mini');
        return true;
    }else{
       // window['jQuery']('body').removeClass('sidebar-mini');
        //window['jQuery']('body').addClass('sidebar-collapse');
        return false;
    }
  }

}
