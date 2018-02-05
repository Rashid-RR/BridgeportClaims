import { Component, OnInit, AfterViewInit } from '@angular/core';
import { HttpService } from '../../services/http-service';
import { ProfileManager } from '../../services/profile-manager';
import { EventsService } from '../../services/events-service';
import { DatePipe, DecimalPipe } from '@angular/common';
declare var Highcharts: any;

@Component({
  selector: 'app-private',
  templateUrl: './dashboard-links.component.html',
  styleUrls: ['./dashboard-links.component.css']
})
export class DashboardLinksComponent implements OnInit, AfterViewInit {
  preload = 'auto';
  categories: Array<any> = [];
  data: Array<any> = [];
  summary: any = {
    lastWorkDate: null,
    totalImagesScanned: null,
    totalImagesIndexed: null,
    totalImagesRemaining: null,
    diariesAdded: null,
    newClaims: null,
    newEpisodes: null,
    newInvoicesPrinted: null,
    newPaymentsPosted: null,
    newPrescriptions: null,
    newReversedPrescriptions: null,
    totalDiariesResolved: null,
    totalDiariesUnResolved: null,
    totalResolvedEpisodes: null,
    totalUnresolvedEpisodes: null
  }
  constructor(
    private http: HttpService,
    private events: EventsService,
    private dp: DatePipe,
    private profileManager: ProfileManager
  ) { }

  ngAfterViewInit() {

  }
  ngOnInit() {
    this.http.getKPIs().map(res => { return res.json(); })
      .subscribe((result: any) => {
        console.log(result);
        this.summary = result;
      }, err => null);
  }

  get allowed(): Boolean {
    return (this.profileManager.profile && this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
      && this.profileManager.profile.roles.indexOf('Admin') > -1);
  }

}
