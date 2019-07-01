import { Component, Inject, OnInit, HostListener, ElementRef, ViewChild } from '@angular/core';
import { EventsService } from '../../services/events-service';
import { ProfileManager } from '../../services/profile-manager';
import { HttpService, GlobalSearchResult } from '../../services/http-service';
import { Router } from '@angular/router';
import { DOCUMENT } from '@angular/platform-browser';
import { ToastrService } from 'ngx-toastr';
import { LocalStorageService } from 'ngx-webstorage';
import { FormControl } from '@angular/forms';
import { BsDropdownDirective } from 'ngx-bootstrap/dropdown';
import { Observable, of, merge, Subject, combineLatest } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, skip, take, mapTo, startWith, map, shareReplay, tap } from 'rxjs/operators';
import { ClaimManager } from '../../services/claim-manager';
import { HttpErrorResponse } from '@angular/common/http';
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
  placeholder: string = 'Search by last name';
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

    const userDetail = localStorage.getItem('user');
    this.avatarHash = md5(JSON.parse(userDetail).email);

    this.date = Date.now();
    this.eventservice.on('disable-links', (status: boolean) => {
      this.disableLinks = status;
    });
    this.selectItem('LastName');
    this.prepareSearchStream();
    this.handleAutoCompleteStyles();
    this.notificationservice.getNotification().subscribe((countParam: number) => {
      this.notificationCount = countParam;
    });
  }

  prepareSearchStream() {
    this.rowSearchHits$ = this.myControl.valueChanges.pipe(
      // delay emits
      debounceTime(300),
      distinctUntilChanged(),
      // use switch map so as to cancel previous subscribed events, before creating new ones
      switchMap((val: any) => {
        // TODO: if the "val" comes in as a CustomerSearchHit object, hit the API again with the FullName.
        // preferably, I would like to avoid doing this to avoid hitting the API at all if it isn't necessary.
        if(val.length > 2 && this.selectedType.length > 0) {
          this.cleanSearch = true;
          return this.http.getGlobalSearch(val, this.selectedType);
        } else {
          this.cleanSearch = false;
          return of([]);
        }
      }),
      tap(val => this.highlightedTexts = this.highlightRows(val)),
      startWith([])
    );
  }
  onAutocompleteOpened() {
    this.autocompleteOpened$.next(true);
  }

  onAutocompleteClosed() {
      this.autocompleteOpened$.next(false);
  }

  private handleAutoCompleteStyles() {
    const isFirstSearchResultReceived$ = this.rowSearchHits$.pipe(
        skip(0), // skip initial state
        take(1), // unsubscribe after first result
        mapTo(true)
    );

    const isSearchCtrlValueExist$ = this.myControl.valueChanges.pipe(
        startWith(''),
        map(value => !!value)
    );

    const isAutocompleteOpened$ = merge(this.autocompleteOpened$.asObservable(), isFirstSearchResultReceived$).pipe(
        shareReplay(1)
    );

    const isSearchInputExpanded$ = combineLatest([isSearchCtrlValueExist$, isAutocompleteOpened$]).pipe(
        map(([isSearchCtrlValueExist, autocompleteOpened]) => isSearchCtrlValueExist || (!isSearchCtrlValueExist && autocompleteOpened))
    );

    isFirstSearchResultReceived$.subscribe(() => this.isFirstSearchResultReceived = true);
    isSearchInputExpanded$.subscribe(x => this.isSearchInputExpanded = x);
    isAutocompleteOpened$.subscribe(x => this.isAutocompleteOpened = x);
  }
  goToClaim(id: Number) {
    if (!this.disableLinks) {
      this.claimManager.search({
        claimNumber: null, firstName: null, lastName: null,
        rxNumber: null, invoiceNumber: null, claimId: id
      }, false);
      if (this.router.url !== '/main/claims') {
        this.router.navigate(['/main/claims']);
      }
    }
  }

  onSearchHit(id) {
    this.selectedClaimId = parseInt(id) || null;
    this.selectedClaimId && this.goToClaim(this.selectedClaimId);
    this.myControl.setValue('');
    this.cleanSearch = false;
    this.prepareSearchStream();
  }
  
  selectItem(txt) {
    this.selectedType = txt;
    switch (this.selectedType) {
      case 'FirstName':
      this.placeholder = 'Search by First Name...';
      break;
      case 'LastName':
      this.placeholder = 'Search by Last Name...';
      break;
      case 'ClaimNumber':
      this.placeholder = 'Search by Claim Number...';
      break;
      default:
        this.placeholder = '';
        break;
      }
      this.cleanSearch = false;
      this.myControl.setValue('');
      this.prepareSearchStream();
  }

  private highlightRows(rows: any[]): string[] {
    if (!rows || rows.length === 0) {
        return [];
    }

    const searchText: string = this.myControl.value;
    const regexp = new RegExp(searchText, 'gi');
    // Check that prevents an error if the authentication token is expired.
    if (rows && !(rows instanceof HttpErrorResponse)) {
        return rows.map((x: any) => {
            const wholeString = x.firstName + ' ' + x.lastName + ' - ' + x.claimNumber
            return wholeString.replace(regexp, match => `<span class="highlighted">${match}</span>`);
        });
    } else {
        return [];
    }
}

  get userName() {
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
    return this.document.location.hostname == 'bridgeportclaims-testing.azurewebsites.net';
  }
  get allowed(): Boolean {
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
}
