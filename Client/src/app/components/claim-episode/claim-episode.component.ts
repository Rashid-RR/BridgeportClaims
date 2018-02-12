import { Component,ViewChild,ElementRef, OnInit } from '@angular/core';
import { EpisodeNoteModalComponent } from '../../components/components-barrel';
import { WindowsInjetor, CustomPosition, Size, WindowConfig } from '../ng-window';
import {ClaimManager} from "../../services/claim-manager";
import {EventsService} from "../../services/events-service";
import { Episode } from 'app/interfaces/episode';
import { SortColumnInfo } from '../../directives/table-sort.directive';
import { HttpService } from '../../services/http-service';

@Component({
  selector: 'app-claim-episode',
  templateUrl: './claim-episode.component.html',
  styleUrls: ['./claim-episode.component.css']
})
export class ClaimEpisodeComponent implements OnInit {


  @ViewChild('prescriptionTable') table: ElementRef;
  sortColumn: SortColumnInfo;
  constructor(private myInjector: WindowsInjetor,public claimManager:ClaimManager,private events:EventsService,private http:HttpService) { }

  ngOnInit() {
  
  }

  getTypeName(id: number): string {
    // find in list for item to get name!!
    if (id) {
      let item =this.claimManager.EpisodeNoteTypes.find(p => p.episodeRoleId == id);
      if (item) {
        return item.episodeRoleName;
      }
      return 'not found';
    }
    return 'not specified';
  }
  showNoteWindow(episode: Episode) {
    let config = new WindowConfig("Episode Note(s)", new Size(400, 700))  //height, width
    
    config.position = new CustomPosition((window.innerWidth-700)/2+50, 60)//left,top
    config.minusTop = 0;
    config.minusHeight = 0;
    config.minusLeft = 0;
    config.minusWidth = 0;
    config.centerInsideParent = false;
    var temp = {}
    config.forAny = [temp];
    config.openAsMaximize = false;
    this.myInjector.openWindow(EpisodeNoteModalComponent, config)
      .then((win: EpisodeNoteModalComponent) => {
        win.showNote(episode);
      })
  }
  edit(episode:Episode){
      this.events.broadcast("edit-episode",episode);    
  }
  onSortColumn(info: SortColumnInfo) {
    this.sortColumn = info;
    this.fetchData();
  }
  fetchData() {
    this.claimManager.loadingEpisodes = true;
      const page = 1;
      const page_size = 30;
      let sort = 'RxDate';
      let sort_dir: 'asc' | 'desc' = 'desc';
      if (this.sortColumn) {
        sort = this.sortColumn.column;
        sort_dir = this.sortColumn.dir;
      }
      this.http.sortEpisodes(this.claimManager.selectedClaim.claimId, sort, sort_dir,
        page, page_size).map(p => p.json())
        .subscribe(results => {
          this.claimManager.selectedClaim.setEpisodes(results);
          this.claimManager.loadingEpisodes = false;
        },err=>{
          this.claimManager.loadingEpisodes = false;
        });
    }

}
