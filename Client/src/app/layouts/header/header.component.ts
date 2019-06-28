import { Component, Inject, OnInit, HostListener, ElementRef, ViewChild } from '@angular/core';
import { EventsService } from '../../services/events-service';
import { ProfileManager } from '../../services/profile-manager';
import { HttpService } from '../../services/http-service';
import { Router } from '@angular/router';
import { DOCUMENT } from '@angular/platform-browser';
import { ToastrService } from 'ngx-toastr';
import { LocalStorageService } from 'ngx-webstorage';
import { FormControl } from '@angular/forms';
import { BsDropdownDirective } from 'ngx-bootstrap/dropdown';

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
  options: string[] = ['One', 'Two', 'Three'];

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
    @Inject(DOCUMENT) private document
  ) { }

  ngOnInit() {
    this.date = Date.now();
    this.eventservice.on('disable-links', (status: boolean) => {
      this.disableLinks = status;
    });
  }

  selectItem(txt) {
    this.placeholder = txt;
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
