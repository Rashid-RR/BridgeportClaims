import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ProfileManager } from '../services/profile-manager';
import { AuthGuard } from '../services/auth.guard';
import { LocalStorageService } from 'ngx-webstorage';
import { EventsService } from '../services/events-service';
declare var $: any;

@Component({
  selector: 'app-layout',
  templateUrl: './app-layout.component.html',
  styleUrls: ['./app-layout.component.css']
})
export class AppLayoutComponent implements OnInit, AfterViewInit {
  buildSha: '';
  buildDate: '';
  currentURL = '';
  constructor(
    private router: Router,
    private profileManager: ProfileManager,
    private guard: AuthGuard,
    private events: EventsService,
    private localSt: LocalStorageService
  ) {

  }

  ngOnInit() {
    const sideBarStatus = this.localSt.retrieve('sidebarOpen');
    sideBarStatus == null ? true : sideBarStatus;
    $('#vegascss').remove();
    this.adjustSideBar(!sideBarStatus);
    this.localSt.observe('sidebarOpen')
      .subscribe((value) => {
        this.adjustSideBar(value);
      });
    this.events.on('login', () => {
      const sideBarStatus = this.localSt.retrieve('sidebarOpen');
      sideBarStatus == null ? true : sideBarStatus;
      this.adjustSideBar(sideBarStatus);
    });
    this.events.on('logout', () => {
      const sideBarStatus = this.localSt.retrieve('sidebarOpen');
      sideBarStatus == null ? true : sideBarStatus;
      this.adjustSideBar(sideBarStatus);
    });
    this.events.on('sidebarOpen', () => {
      const st = document.body.classList;
      if (st.contains('sidebar-collapse')) {
        this.localSt.store('sidebarOpen', false);
      } else {
        this.localSt.store('sidebarOpen', true);
      }
    });

    this.currentURL = this.router.url;
    this.router.events.subscribe(ev => {
      if (ev instanceof NavigationEnd) {
        this.currentURL = this.router.url;
      }
    });
    $('body').removeClass('vegas-container');
  }
  adjustSideBar(status) {
    this.guard.isLoggedIn.subscribe(r => {
      if (!r) {
        window['jQuery']('body').removeClass('sidebar-mini');
        window['jQuery']('body').addClass('sidebar-collapse');
      } else {
        const st = document.body.classList;
        if (!st.contains('sidebar-mini')) {
          window['jQuery']('body').addClass('sidebar-mini');
        }
        if (!status) {
          window['jQuery']('body').addClass('sidebar-collapse');
        } else {
          window['jQuery']('body').removeClass('sidebar-collapse');
        }
      }
    }, err => { });
  }
  get isLoggedIn(): boolean {
    if (this.profileManager.profile) {
      return true;
    } else {
      return false;
    }
  }
  ngAfterViewInit() {
   
  }

}
