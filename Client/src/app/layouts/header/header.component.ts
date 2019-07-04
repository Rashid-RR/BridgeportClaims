import { Component, Inject, OnInit, ElementRef, ViewChild } from '@angular/core';
import { EventsService } from '../../services/events-service';
import { ProfileManager } from '../../services/profile-manager';
import { HttpService, GlobalSearchResult } from '../../services/http-service';
import { Router } from '@angular/router';
import { DOCUMENT } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { LocalStorageService } from 'ngx-webstorage';
import { FormControl } from '@angular/forms';
import { BsDropdownDirective } from 'ngx-bootstrap/dropdown';
import { Observable, Subject } from 'rxjs';
import { ClaimManager } from '../../services/claim-manager';
import { NotificationService } from '../../services/notification.service';
import { md5 } from '../md5/md5';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  date: number;
  disableLinks = false;
  isAutoCompleteOpen = false;
  public imgSrc!: string;
  placeholder = 'Search';
  @ViewChild('dropdown') dropdown: BsDropdownDirective;
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
  ) { }

  ngOnInit() {

    this.setUserImage();
    this.date = Date.now();
    this.eventservice.on('disable-links', (status: boolean) => {
      this.disableLinks = status;
    });
    this.notificationservice.getNotification().subscribe((countParam: number) => {
      this.notificationCount = countParam;
    });

    this.sidebarToggle();
  }

  get userName() {
    this.setUserImage();
    return this.profileManager.profile ? this.profileManager.profile.firstName + ' ' + this.profileManager.profile.lastName : '';
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
    this.avatarHash = md5(JSON.parse(userDetail).email);
    this.imgSrc = `https://www.gravatar.com/avatar/${this.avatarHash}/?random=` + new Date().getTime();
  }

}
