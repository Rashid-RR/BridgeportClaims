import { DOCUMENT } from '@angular/common';
import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { BsDropdownDirective } from 'ngx-bootstrap/dropdown';
import { ToastrService } from 'ngx-toastr';
import { LocalStorageService } from 'ngx-webstorage';
import { Observable, Subject } from 'rxjs';
import { ClaimManager } from '../../services/claim-manager';
import { EventsService } from '../../services/events-service';
import { GlobalSearchResult, HttpService } from '../../services/http-service';
import { NotificationService } from '../../services/notification.service';
import { ProfileManager } from '../../services/profile-manager';
import { md5 } from '../md5/md5';
import * as moment from 'moment';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  host: {
    '(document:click)': 'onClick($event)',
  },
})
export class HeaderComponent implements OnInit {

  date: number;
  disableLinks = false;
  isAutoCompleteOpen = false;
  public imgSrc!: string;
  placeholder = 'Search';
  @ViewChild('dropdown') dropdown: BsDropdownDirective;
  @ViewChild('dropdownContainer') dropdownContainer: ElementRef;
  myControl = new FormControl();
  options: string[] = [];
  selectedType = '';
  displayFullName = '';
  highlightedTexts = [];
  rowSearchHits$!: Observable<GlobalSearchResult[]>;
  selectedClaimId = null;
  isSearchInputExpanded = false;
  isAutocompleteOpened = false;
  isFirstSearchResultReceived = false;
  cleanSearch = true;
  notificationCount = 0;
  avatarHash: any;
  timeString: string;
  second: number;
  minutes: number;
  hours: number;
  interval: any;

  private autocompleteOpened$: Subject<boolean> = new Subject<boolean>();

  // @HostListener('document:click', ['$event'])
  // clickout(event) {
  //   if (!this.eRef.nativeElement.contains(event.target)) {
  //     this.dropdown.isOpen = false;
  //   }

  // }

  constructor(
    private http: HttpService,
    private eRef: ElementRef,
    private router: Router,
    public eventservice: EventsService,
    public profileManager: ProfileManager,
    private toast: ToastrService,
    public localSt: LocalStorageService,
    @Inject(DOCUMENT) private document,
    public claimManager: ClaimManager,
    public notificationservice: NotificationService,
  ) {
    this.eventservice.on('login', () => {
      this.setUserImage();
      if (this.profileManager.profile) {
        if (this.profileManager.profile.roles[0] === 'Admin') {
        this.fetchNotifications();
        }
      }
    });
    this.eventservice.on('logout', () => {
      this.notificationCount = 0;
    });
  }

  ngOnInit() {
    this.setUserImage();
    this.date = Date.now();
    this.eventservice.on('disable-links', (status: boolean) => {
      this.disableLinks = status;
    });
    if (this.profileManager.profile) {
      if (this.profileManager.profile.roles[0] === 'Admin') {
      this.fetchNotifications();
      }
    }
    this.sidebarToggle();

    this.http.getStartTime().subscribe(startTime => {      
      if (startTime) {
        let now  = moment().format("DD/MM/YYYY HH:mm:ss");
        let then = moment(startTime + '-07:00').format("DD/MM/YYYY HH:mm:ss");

        let ms = moment(now,"DD/MM/YYYY HH:mm:ss").diff(moment(then,"DD/MM/YYYY HH:mm:ss"));
        let d = moment.duration(ms);
        let s = Math.floor(d.asHours()) + moment.utc(ms).format(":mm:ss");
        let time = s.split(":");
        let sec = parseInt(time[2]);
        this.updateClock(parseInt(time[0]), parseInt(time[1]), sec);
        this.interval = setInterval(() => {
          if (this.second || this.second == 0) {
            this.updateClock(this.hours, this.minutes, this.second);
          }
        }, 1000);
      }
    });

    this.http.getClock().subscribe(val => {
      if (val == false) {
        clearInterval(this.interval);
      } else {
        if (val) {
          this.second = undefined;
          this.hours = undefined;
          this.minutes = undefined;
          let s = "00:00:00";
          let time = s.split(":");
          let sec = parseInt(time[2]);
          this.updateClock(parseInt(time[0]), parseInt(time[1]), sec);
            this.interval = setInterval(() => {
              if (this.second || this.second == 0) {
                this.updateClock(this.hours, this.minutes, this.second);
              }
            }, 1000);
        }
      }
    });
  }

  updateClock(h, m, s) {
    let hour = h, mins = m, secs = s;
    secs = secs + 1;
    this.second = secs;
    this.minutes = mins;
    this.hours = hour;
      if (secs == 60){
        secs = 0;
        this.second = secs;
        mins = mins + 1;
        this.minutes = this.minutes + 1;
      }
      if (mins==60){
        mins = 0;
        this.minutes = mins;
        hour = hour + 1;
        this.hours = this.hours + 1;
      }
      if (hour==13){
        hour = '0' + 1;
        this.hours = 1;
      }

      this.timeString = this.formatDigit(hour) + ':' + this.formatDigit(mins) + ':' + this.formatDigit(secs);
  }

  formatDigit(digit){ 
    let zpad = digit + '';
    if (digit < 10) {
        zpad = "0" + zpad;
    }
    return zpad;
  }



  fetchNotifications(): void {
      this.notificationservice.getNotification().subscribe((countParam: number) => {
        if (!this.isClient) {
           this.notificationCount = countParam;
        }
      });
  }
  get userName() {
    return this.profileManager.profile ? this.profileManager.profile.firstName + ' ' + this.profileManager.profile.lastName : '';
  }
  onDropdownOutsideClick($event): void {
    console.log($event);
  }
  clearCache() {
    this.http.clearCache().subscribe(res => {
      this.toast.success(res.message);
    }, err => {
      this.toast.error(err.message);
    });
  }
  get isTestDomain() {
    return this.document.location.hostname === 'bridgeportclaims-testing.azurewebsites.net';
  }
  get allowed(): Boolean {
    // tslint:disable-next-line: max-line-length
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array) && this.profileManager.profile.roles.indexOf('Admin') > -1);
  }
  sidebarToggle() {
    this.eventservice.broadcast('sidebarOpen', true);
  }

  logout() {
    if (!this.disableLinks) {
      this.eventservice.broadcast('logout', true);
      this.profileManager.profile = undefined;
      localStorage.removeItem('user');
      this.router.navigate(['/login']);
    }
  }

  onFocus() {
    this.isAutoCompleteOpen = true;
  }
  onBlur() {
    this.isAutoCompleteOpen = false;
  }

  setUserImage() {
    const userDetail = localStorage.getItem('user');
    if (!userDetail) {
      return;
    }
    this.avatarHash = md5(JSON.parse(userDetail).email);
    this.imgSrc = `https://www.gravatar.com/avatar/${this.avatarHash}/?random=` + new Date().getTime();
  }
  get isClient(): boolean {
    return (!this.profileManager.profile || !this.profileManager.profile.roles) ||
      (this.profileManager.profile && this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
        && this.profileManager.profile.roles.indexOf('Client') > -1);
  }
  get adminOnly(): Boolean {
    return (this.profileManager.profile && this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
      && this.profileManager.profile.roles.indexOf('Admin') > -1);
  }
  onClick = function  (event) {
    if (this.dropdownContainer && !this.dropdownContainer.nativeElement.contains(event.target) &&
    !event.target.closest('dialog-holder') &&
    !event.target.classList.contains('payor-search-elm') &&
    !event.target.classList.contains('mat-option-text') &&
    !event.target.classList.contains('mat-option')
    ) {
        this.dropdown.hide();
    }
  };

}
