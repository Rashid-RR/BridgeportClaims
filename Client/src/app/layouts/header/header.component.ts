import { Component, OnInit } from "@angular/core";
import { EventsService } from "../../services/events-service";
import { ProfileManager } from "../../services/profile-manager";
import { HttpService } from "../../services/http-service";
import { Router } from "@angular/router";
import { Subscription } from 'rxjs/Subscription';
import {AuthGuard} from "../../services/auth.guard"
import {LocalStorageService} from 'ng2-webstorage';
import {ToastsManager } from 'ng2-toastr/ng2-toastr';

declare var jQuery:any
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  date: number;

  constructor(
    private http: HttpService,
    private router: Router,
    public eventservice: EventsService,
    public profileManager: ProfileManager,
    private guard:AuthGuard,
    private toast:ToastsManager,
    private localSt:LocalStorageService
  ) { }
  
  ngOnInit() {
    this.date = Date.now();
    
  }

  clearCache(){
    this.http.clearCache().map(r=>{return r.json()}).subscribe(res=>{
        this.toast.success(res.message);
    },err=>{
      this.toast.error(err.message);
    })
  }
  get allowed():Boolean{
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array) && this.profileManager.profile.roles.indexOf('Admin')>-1)
  }
  sidebarToggle(){
    this.eventservice.broadcast("sidebarOpen", true);    
  }

  logout() {
    this.eventservice.broadcast("logout", true);
    this.profileManager.profile = undefined;
    localStorage.removeItem('user');
    this.router.navigate(['/login']);
    /* this.http.logout().subscribe(res=>{
         console.log(res);
     });*/
  }
}
