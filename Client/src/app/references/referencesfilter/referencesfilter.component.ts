import {AfterViewInit, Component, NgZone, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {DatePipe} from '@angular/common';
// Services
import {DocumentManagerService} from '../../services/document-manager.service';
import {EventsService} from '../../services/events-service';
import {ProfileManager} from '../../services/profile-manager';

declare var $: any;

@Component({
  selector: 'app-referencesfilter',
  templateUrl: './referencesfilter.component.html',
  styleUrls: ['./referencesfilter.component.css']
})
export class ReferencesfilterComponent implements OnInit {

  date: string;
  fileName: string;
  submitted = false;
  public flag= 'File Name';

  constructor(
    public ds: DocumentManagerService,
    private dp: DatePipe,
    private zone: NgZone,
    private profileManager: ProfileManager,
    private events: EventsService,
    private route: ActivatedRoute) {

  }

  // get isuserNotAdmin(): Boolean {
  //   return (this.profileManager.profile && this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
  //     && this.profileManager.profile.roles.indexOf('Admin') === -1
  //   );
  // }

  ngOnInit() {

  }


  search() {
  }

  filter($event) {
  }

  clearFilters() {

  }
}
