import {Component, OnInit} from "@angular/core";
import {EventsService} from "../../services/events-service";
import {ProfileManager} from "../../services/profile-manager";
import {HttpService} from "../../services/http-service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  constructor(private http: HttpService,private events:EventsService, private router: Router, private profileManager:ProfileManager) { }

  ngOnInit() {

  }

  get userName(){
    return this.profileManager.profile ? this.profileManager.profile.displayName : '';
  }
  get avatar(){
    return this.profileManager.profile ?   this.profileManager.profile.avatarUrl : '';
  }


}
