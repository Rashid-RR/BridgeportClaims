import { Component,ElementRef,ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { HttpService } from '../../services/http-service';
import { ProfileManager } from '../../services/profile-manager';
import { EventsService } from '../../services/events-service';
import { DatePipe, DecimalPipe } from '@angular/common';
import { DomSanitizer } from '@angular/platform-browser';
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
  win=window;
  @ViewChild('images') images:ElementRef;
  summary = {
    lastWorkDate:  new Date(),
    totalImagesScanned: 92,
    totalImagesIndexed: 69,
    totalImagesRemaining: 23,
    fileWatcherHealthy:true,
    diariesAdded: 53,
    newClaims: 53,
    newEpisodes: 22,
    newInvoicesPrinted: 6,
    newPaymentsPosted: 33,
    newPrescriptions: 90,
    newReversedPrescriptions: 39,
    totalDiariesResolved: 11,
    totalDiariesUnResolved: 54,
    totalResolvedEpisodes: 76,
    totalUnresolvedEpisodes: 11
  }
  constructor(
    private http: HttpService,
    private events: EventsService,
    private dp: DatePipe,
    private sanitizer: DomSanitizer,
    private profileManager: ProfileManager
  ) { }

  ngAfterViewInit() {

  }

  sanitize(style){
    return this.sanitizer.bypassSecurityTrustStyle(style);
  }
  ngOnInit() { 
    this.http.getKPIs().map(res => { return res.json(); })
      .subscribe((result: any) => { 
        this.summary = result;
      }, err => null);
  }

  get totalImagesIndexed(){
       return (100*(this.summary.totalImagesIndexed || 0)/this.totalImages) || 0;
  }
  get totalImagesRemaining(){
       return (100*(this.summary.totalImagesRemaining || 0)/this.totalImages) || 0;
  }

  get totalImages(){
      let total = (this.summary.totalImagesIndexed || 0)+(this.summary.totalImagesRemaining || 0);
      return total || 0;
  }

  get imagesSliderPosition(){
    return this.images.nativeElement.offsetTop;
  }

  get totalDiariesResolved(){
       return (100*(this.summary.totalDiariesResolved || 0)/this.totalDiaries) || 0;
  }
  get totalDiariesUnResolved(){
       return (100*(this.summary.totalDiariesUnResolved || 0)/this.totalDiaries) || 0;
  }

  get totalDiaries(){
      let total = (this.summary.totalDiariesResolved || 0)+(this.summary.totalDiariesUnResolved || 0);
      return total || 0;
  }

  get totalResolvedEpisodes(){
       return (100*(this.summary.totalResolvedEpisodes || 0)/this.totalEpisodes ) || 0;
  }
  get totalUnresolvedEpisodes(){
       return (100*(this.summary.totalUnresolvedEpisodes || 0)/this.totalEpisodes ) || 0;
  }

  get totalEpisodes(){
      let total = (this.summary.totalUnresolvedEpisodes || 0)+(this.summary.totalResolvedEpisodes || 0);
      return total || 0;
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
  simpleDate(input: String) {
    if (!input) return null;
    if (input.indexOf("-") > -1) {
      let date = input.split("T");
      let d = date[0].split("-");
      return d[1] + "-" + d[2] + "-" + d[0];
    } else {
      return input;
    }
  }
}
