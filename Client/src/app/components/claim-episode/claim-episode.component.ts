import { Component, OnInit } from '@angular/core';
import {ClaimManager} from "../../services/claim-manager";
import {EventsService} from "../../services/events-service";

@Component({
  selector: 'app-claim-episode',
  templateUrl: './claim-episode.component.html',
  styleUrls: ['./claim-episode.component.css']
})
export class ClaimEpisodeComponent implements OnInit {

  constructor(public claimManager:ClaimManager,private events:EventsService) { }

  ngOnInit() {
  }

  edit(id:Number){
      this.events.broadcast("edit-episode",id);    
  }

}
