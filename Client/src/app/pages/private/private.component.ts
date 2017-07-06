import { Component, OnInit } from '@angular/core';
import {HttpService} from "../../services/http-service";
import {ProfileManager} from "../../services/profile-manager";
import {EventsService} from "../../services/events-service";

@Component({
  selector: 'app-private',
  templateUrl: './private.component.html',
  styleUrls: ['./private.component.css']
})
export class PrivateComponent implements OnInit {
  constructor(private http: HttpService, private events: EventsService,private profileManager:ProfileManager)  {

   }

  ngOnInit() {
    
  }
  get allowed():Boolean{
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array) && this.profileManager.profile.roles.indexOf('Admin')>-1)
  }

}
