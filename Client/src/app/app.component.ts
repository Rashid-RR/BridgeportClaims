import { Component, OnDestroy, OnInit, ViewContainerRef } from '@angular/core';
import { HttpService } from './services/services.barrel';
import { ProfileManager } from './services/profile-manager';
import { UserProfile } from './models/profile';
import { EventsService } from './services/events-service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-root',
  template: `<div  id="highlighter" style="width:0px !important;height:0px !important"></div><router-outlet></router-outlet>`
})
export class AppComponent implements OnInit, OnDestroy {
  t: any;
  constructor(
    private toast: ToastrService,
    private http: HttpService,
    private events: EventsService,
    private profileManager: ProfileManager,
    public viewContainerRef: ViewContainerRef
  ) {

  }

  ngOnDestroy() {

  }
  ngOnInit() {  
    const user = localStorage.getItem('user');
    if (user !== null && user.length > 0) {
      try {
        const us = JSON.parse(user);
        // this.events.broadcast('profile', us);
        this.http.setAuth(us.access_token);
        const profile = new UserProfile(us.id || us.email, us.email, us.firstName || us.email, us.lastName || us.email, us.email || us.email, us.email, us.avatarUrl, us.createdOn, us.roles);
        this.profileManager.setProfile(profile);
        this.profileManager.profile = profile;
        if (window.location.hash.indexOf('#/confirm-email') !== 0) {
          this.http.userFromId(us.id).subscribe(res => {
            this.profileManager.profile.roles = res.roles;
            this.profileManager.profile.extension = res.extension;
          }, _ => {
          });
        }
      } catch (error) {
      }
    }
    this.events.on('logout', (v) => {
      this.profileManager.clearUsers();
      this.profileManager.profile = undefined;
    });

  }
}
