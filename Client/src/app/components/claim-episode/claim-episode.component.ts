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
  getTypeName(id: number): string {
    // find in list for item to get name!!
    if (id) {
      let item =this.claimManager.EpisodeNoteTypes.find(p => p.episodeTypeId == id);
      if (item) {
        return item.episodeTypeName;
      }
      return 'not found';
    }
    return 'not specified';
  }
  edit(id:Number,type:String){
      this.events.broadcast("edit-episode",id,type);    
  }

}
