import { Component, ViewChild, ElementRef, OnInit } from '@angular/core';
import { EpisodeNoteModalComponent } from '../components-barrel';
import { WindowsInjetor, CustomPosition, Size, WindowConfig } from '../ng-window';
import { ClaimManager } from '../../services/claim-manager';
import { EventsService } from '../../services/events-service';
import { Episode } from '../../interfaces/episode';
import { SortColumnInfo } from '../../directives/table-sort.directive';
import { HttpService } from '../../services/http-service';
import { EpisodeService } from '../../services/episode.service';
import { DialogService } from 'ng2-bootstrap-modal';
import { ToastrService } from 'ngx-toastr';
declare var $: any;
import { ConfirmComponent } from '../confirm.component';
import { SwalComponent } from '@toverux/ngx-sweetalert2';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { DecisionTreeModalComponent } from '../decesiontree-modal/decesiontree-modal.component';

@Component({
  selector: 'app-claim-episode',
  templateUrl: './claim-episode.component.html',
  styleUrls: ['./claim-episode.component.css']
})
export class ClaimEpisodeComponent implements OnInit {


  @ViewChild('prescriptionTable') table: ElementRef;
  @ViewChild('episodeActionSwal') private episodeSwal: SwalComponent;
  sortColumn: SortColumnInfo;

  constructor(public episodeService: EpisodeService,
    private myInjector: WindowsInjetor, private dialogService: DialogService,
    public claimManager: ClaimManager, private events: EventsService,
    public dialog: MatDialog,
    private http: HttpService, private toast: ToastrService) {
  }

  ngOnInit() {
    this.events.on('episode-note-updated', (episode: Episode) => {
      this.claimManager.selectedClaim.episodes.forEach(ep => {
        if (episode.episodeId == ep.episodeId) {
          ep.episodeNoteCount = episode.episodeNoteCount;
          ep.noteCount = episode.episodeNoteCount;
        }
      });
    });
  }

  openDialogue(id) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.minWidth = '900px'
    dialogConfig.minHeight = '500px'
    dialogConfig.data = {
      episodeId: id,
    };
    this.dialog.open(DecisionTreeModalComponent, dialogConfig);
  }

  getTypeName(id: number): string {
    // find in list for item to get name!!
    if (id) {
      const item = this.claimManager.EpisodeNoteTypes.find(p => p.episodeRoleId === id);
      if (item) {
        return item.episodeRoleName;
      }
      return 'not found';
    }
    return 'not specified';
  }

  showNoteWindow(episode: Episode) {
    if (!episode.episodeId && episode['id']) {
      episode.episodeId = episode['id'];
    }
    const config = new WindowConfig('Episode Note(s)', new Size(400, 700));  // height, width
    config.position = new CustomPosition((window.innerWidth - 700) / 2 + 50, 60); // left,top
    config.minusTop = 0;
    config.minusHeight = 0;
    config.minusLeft = 0;
    config.minusWidth = 0;
    config.centerInsideParent = false;
    const temp = {};
    config.forAny = [temp];
    config.openAsMaximize = false;
    this.myInjector.openWindow(EpisodeNoteModalComponent, config)
      .then((win: EpisodeNoteModalComponent) => {
        win.showNote(episode);
      });
  }

  edit(episode: Episode) {
    this.events.broadcast('edit-episode', episode);
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
      page, page_size)
      .subscribe((results: any) => {
        this.claimManager.selectedClaim.setEpisodes(results);
        this.claimManager.loadingEpisodes = false;
      }, err => {
        this.claimManager.loadingEpisodes = false;
      });
  }

  assign(episode: Episode) {
    this.episodeService.episodetoAssign = episode;
    this.episodeSwal.show().then((r) => {

    });
  }

  markAsResolved($event, episode) {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Mark Episode as Resolved',
      message: 'Are you sure you want to resolve this episode?'
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.claimManager.loading = true;
          this.http.markEpisodeAsSolved(episode.episodeId || episode['id']).subscribe(res => {
            this.toast.success(res.message);
            this.claimManager.loading = false;
            episode.resolved = true;
          }, error => {
            this.toast.error(error.message);
            $event.target.checked = false;
            this.claimManager.loading = false;
          });
        } else {
          $event.target.checked = false;
        }
      });
  }

}
