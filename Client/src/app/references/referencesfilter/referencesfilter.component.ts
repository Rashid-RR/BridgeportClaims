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
  adjustorName: string;
  submitted = false;
  public flag= 'File Name';

  constructor() {

  }

  // get isuserNotAdmin(): Boolean {
  //   return (this.profileManager.profile && this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
  //     && this.profileManager.profile.roles.indexOf('Admin') === -1
  //   );
  // }

  ngOnInit() {

  }


  search() {
   var  data = {
      "searchText": this.adjustorName,
      "sort": "AdjustorName",
      "sortDirection": "ASC",
      "page": 1,
      "pageSize": 30
    }
  }

  filter($event) {
  }

  clearFilters() {

  }
}
