import {AfterViewInit, Component, NgZone, OnInit} from '@angular/core';

import {ReferenceManagerService} from '../../services/reference-manager.service';

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

  constructor(public rs: ReferenceManagerService) {

  }


  ngOnInit() {

  }


  search() {

    this.rs.setSearchText(this.adjustorName);
    this.rs.getadjustorslist()
  }

  filter($event) {
  }

  clearFilters() {

  }
}
