import { Component, OnInit } from "@angular/core";
import { EventsService } from "../../services/events-service";
import { ProfileManager } from "../../services/profile-manager";
import { HttpService } from "../../services/http-service";
import { Router } from "@angular/router";
import { Subscription } from 'rxjs/Subscription';

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
    public profileManager: ProfileManager
  ) { }
  
  ngOnInit() {
    this.date = Date.now();
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
