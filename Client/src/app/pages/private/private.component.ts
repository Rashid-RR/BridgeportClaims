import { Component, OnInit } from '@angular/core';
import {HttpService} from "../../services/http-service";
import {ProfileManager} from "../../services/profile-manager";
import {EventsService} from "../../services/events-service";
import {VgAPI} from 'videogular2/core';

@Component({
  selector: 'app-private',
  templateUrl: './private.component.html',
  styleUrls: ['./private.component.css']
})
export class PrivateComponent implements OnInit {
  preload:string = 'auto';
  api:VgAPI;
  constructor(private http: HttpService, private events: EventsService,private profileManager:ProfileManager)  {

   }

  ngOnInit() {
    
  }
  onPlayerReady(api:VgAPI) {
    this.api = api;

    this.api.getDefaultMedia().subscriptions.timeUpdate.subscribe(()=>{
      try{
        if(this.api.getDefaultMedia().currentTime>14){
          this.api.pause();
        }
      }catch(e){

      }
    });
    this.api.play();
    
    /* this.api.getDefaultMedia().subscriptions.ended.subscribe(
        () => {
            // Set the video to the beginning
            this.api.getDefaultMedia().currentTime = 0;
            this.api.play();
        }
    ); */
  }
  get allowed():Boolean{
    return (this.profileManager.profile && this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array) && this.profileManager.profile.roles.indexOf('Admin')>-1)
  }

}
