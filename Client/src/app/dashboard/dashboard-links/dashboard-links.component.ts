import { Constants } from './../../shared/constants';
import { LocalStorageService } from 'ngx-webstorage';
import { Component, ElementRef, ViewEncapsulation, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { HttpService } from '../../services/http-service';
import { ProfileManager } from '../../services/profile-manager';
import { EventsService } from '../../services/events-service';
import { DatePipe } from '@angular/common';
import { DomSanitizer } from '@angular/platform-browser';
import { SwalComponent, SwalPartialTargets } from '@sweetalert2/ngx-sweetalert2';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-private',
  templateUrl: './dashboard-links.component.html',
  styleUrls: ['./dashboard-links.component.css']
})
export class DashboardLinksComponent implements OnInit, AfterViewInit {
  preload = 'auto';
  categories: Array<any> = [];
  data: Array<any> = [];
  win = window;
  over: any[] = [false];
  @ViewChild('searchSwal') private searchSwal: SwalComponent;
  @ViewChild('images') images: ElementRef;
  summary = {
    lastWorkDate: null,
    totalImagesScanned: null,
    totalImagesIndexed: null,
    totalImagesRemaining: null,
    fileWatcherHealthy: true,
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
  };
  isClockIn = false;
  constructor(
    private http: HttpService,
    private events: EventsService,
    private dp: DatePipe,
    private toast: ToastrService,
    public readonly swalTargets: SwalPartialTargets,
    private sanitizer: DomSanitizer,
    private profileManager: ProfileManager,
    private localStorage: LocalStorageService
  ) {}

  ngAfterViewInit() {

  }
  search() {
    if (this.allowed) {
      this.searchSwal.show().then((r) => {

      });
    }
  }

  sanitize(style) {
    return this.sanitizer.bypassSecurityTrustStyle(style);
  }

  ngOnInit() {
    this.isClockIn = this.localStorage.retrieve(Constants.IsClockInKey);
    this.http.getStartTime().subscribe(startTime => console.log(startTime));

    if (!this.isClient) {
      this.http.getKPIs()
        .subscribe((result: any) => {
          this.summary = result;
        }, () => null);
    }
  }

  get totalImagesIndexed() {
    return (100 * (this.summary.totalImagesIndexed || 0) / this.totalImages) || 0;
  }
  get totalImagesRemaining() {
    return (100 * (this.summary.totalImagesRemaining || 0) / this.totalImages) || 0;
  }

  get totalImages() {
    const total = (this.summary.totalImagesIndexed || 0) + (this.summary.totalImagesRemaining || 0);
    return total || 0;
  }

  get fileWatcherHealthy() {
    return this.fileWatcherHealthy;
  }

  get imagesSliderPosition() {
    return this.images.nativeElement.offsetTop;
  }

  get totalDiariesResolved() {
    return (100 * (this.summary.totalDiariesResolved || 0) / this.totalDiaries) || 0;
  }
  get totalDiariesUnResolved() {
    return (100 * (this.summary.totalDiariesUnResolved || 0) / this.totalDiaries) || 0;
  }

  get totalDiaries() {
    const total = (this.summary.totalDiariesResolved || 0) + (this.summary.totalDiariesUnResolved || 0);
    return total || 0;
  }

  get totalResolvedEpisodes() {
    return (100 * (this.summary.totalResolvedEpisodes || 0) / this.totalEpisodes) || 0;
  }
  get totalUnresolvedEpisodes() {
    return (100 * (this.summary.totalUnresolvedEpisodes || 0) / this.totalEpisodes) || 0;
  }

  get totalEpisodes() {
    const total = (this.summary.totalUnresolvedEpisodes || 0) + (this.summary.totalResolvedEpisodes || 0);
    return total || 0;
  }

  get allowed(): Boolean {
    return (this.profileManager.profile && this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
      && this.profileManager.profile.roles.indexOf('Admin') > -1);
  }
  formatDate(input: String) {
    if (!input) {
      return null;
    }
    if (input.indexOf('-') > -1) {
      const date = input.split('T');
      const d = date[0].split('-');
      return d[1] + '/' + d[2] + '/' + d[0];
    } else {
      return input;
    }
  }
  simpleDate(input: String) {
    if (!input) {
      return null;
    }
    if (input.indexOf('-') > -1) {
      const date = input.split('T');
      const d = date[0].split('-');
      return d[1] + '-' + d[2] + '-' + d[0];
    } else {
      return input;
    }
  }

  get isClient(): Boolean {
    return (this.profileManager.profile && this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
      && this.profileManager.profile.roles.indexOf('Client') > -1);
  }

  clockIn() {
    this.http.clockIn().subscribe(response => {
      this.toast.success(response.message);
      this.localStorage.store(Constants.IsClockInKey, true);
      this.isClockIn = true;
    });
  }

  clockOut() {
    this.http.clockOut().subscribe(response => {
      this.toast.success(response.message);
      this.localStorage.store(Constants.IsClockInKey, false);
      this.isClockIn = false;
    });
  }
}
