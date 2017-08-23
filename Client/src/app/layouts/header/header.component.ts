import { Component, OnInit } from "@angular/core";
import { EventsService } from "../../services/events-service";
import { ProfileManager } from "../../services/profile-manager";
import { HttpService } from "../../services/http-service";
import { Router } from "@angular/router";
import { Subscription } from 'rxjs/Subscription';
import {AuthGuard} from "../../services/auth.guard"
import {LocalStorageService} from 'ng2-webstorage';

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
    private localSt:LocalStorageService
  ) { }
  
  ngOnInit() {
    this.date = Date.now();
    
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
