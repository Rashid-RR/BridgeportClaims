import {AfterViewInit, Component, NgZone, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {DatePipe} from '@angular/common';
// Services
import {DocumentManagerService} from '../../services/document-manager.service';
import {EventsService} from '../../services/events-service';
import {ProfileManager} from '../../services/profile-manager';

declare var $: any;

@Component({
  selector: 'payment-unindexed-check-filter',
  templateUrl: './unindexed-check-filter.component.html',
  styleUrls: ['./unindexed-check-filter.component.css']
})
export class PaymentCheckFilterComponent implements OnInit, AfterViewInit {

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
    this.events.on('payment-suspense', () => {
      this.search();
    });
    this.events.on('payment-closed', () => {
      this.search();
    });
  }

  // get isuserNotAdmin(): Boolean {
  //   return (this.profileManager.profile && this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
  //     && this.profileManager.profile.roles.indexOf('Admin') === -1
  //   );
  // }

  ngOnInit() {

  }

  ngAfterViewInit() {
    // Date picker
    $('#checksdate').datepicker({
      autoclose: true
    });
    this.route.params.subscribe(params => {
      if (params['date'] && params['date'] !== 'invoice') {
        this.date = params['date'].replace(/\-/g, '/');
        this.zone.run(() => {
        });
      }
    });
  }

  search() {
    const date = this.dp.transform($('#checksdate').val(), 'MM/dd/yyyy');
    this.ds.checksData.date = date || null;
    this.ds.checksData.fileName = this.fileName || null;
    this.ds.viewPostedDetail = undefined;
    if (this.ds.isuserNotAdmin === true) {
      this.ds.postedChecks = true;
      this.ds.viewPostedDetail = false;
      this.ds.archivedChecksData.archived = null;
      this.ds.searchCheckes();

    }
    else {
      this.ds.searchCheckes();

    }
  }

  filter($event) {
    switch ($event.target.value) {
      case 'posted':
        this.ds.postedChecks = true;
        this.ds.viewPostedDetail = false;

        this.search();

        break;
      case 'archived':
        this.ds.postedChecks = false;
        this.ds.viewPostedDetail = false;
        this.ds.archivedChecksData.archived = $event.target.checked;
        this.search();


        break;
      case 'default':
        this.ds.postedChecks = false;
        this.ds.archivedChecksData.archived = null;
        this.search();


        break;

    }
  }

  clearFilters() {
    $('#checksdate').val('');
    $('#CarchivedCheck').prop('checked', false);
    $('#CarchivedCheck1').prop('checked', false);
    this.ds.viewPostedDetail = undefined;
    this.fileName = '';
    this.search();
  }
}
