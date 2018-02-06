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
    lastWorkDate:  "2018-02-05T00:00:00.0000000",
    totalImagesScanned: 70,
    totalImagesIndexed: 33,
    totalImagesRemaining: 113,
    diariesAdded: 53,
    newClaims: 53,
    newEpisodes: 22,
    newInvoicesPrinted: 6,
    newPaymentsPosted: 33,
    newPrescriptions: 90,
    newReversedPrescriptions: 39,
    totalDiariesResolved: 41,
    totalDiariesUnResolved: 54,
    totalResolvedEpisodes: 76,
    totalUnresolvedEpisodes: 11
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
        //this.summary = result;
      }, err => null);
  }

  get allowed(): Boolean {
    return (this.profileManager.profile && this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
      && this.profileManager.profile.roles.indexOf('Admin') > -1);
  }
  formatDate(input: String) {
    if (!input) return null;
    if (input.indexOf("-") > -1) {
      let date = input.split("T");
      let d = date[0].split("-");
      return d[1] + "/" + d[2] + "/" + d[0];
    } else {
      return input;
    }
  }

}
