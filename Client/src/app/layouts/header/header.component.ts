import {Component, OnInit} from "@angular/core";
import {EventsService} from "../../services/events-service";
import {ProfileManager} from "../../services/profile-manager";
import {HttpService} from "../../services/http-service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  date: string;

  constructor(private http: HttpService, private router: Router, public eventservice: EventsService,public profileManager:ProfileManager) {

  }

  ngOnInit() {
    this.http.getDate().subscribe(response => {
        this.date = response.json().message;
        console.log(this.date)
      }
    )
  }

  logout() {
    this.eventservice.broadcast("logout", true);  
    this.profileManager.profile=undefined;
    localStorage.removeItem('user');  
    this.router.navigate(['/login']);
   /* this.http.logout().subscribe(res=>{
        console.log(res);
    });*/
  }
}
