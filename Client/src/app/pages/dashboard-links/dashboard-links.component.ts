import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../services/http-service';
import { ProfileManager } from '../../services/profile-manager';
import { EventsService } from '../../services/events-service';

@Component({
  selector: 'app-private',
  templateUrl: './dashboard-links.component.html',
  styleUrls: ['./dashboard-links.component.css']
})
export class DashboardLinksComponent implements OnInit {
  preload = 'auto';
  constructor(
    private http: HttpService,
    private events: EventsService,
    private profileManager: ProfileManager
  ) { }

  ngOnInit() { }

  get allowed(): Boolean {
    return (this.profileManager.profile && this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
    && this.profileManager.profile.roles.indexOf('Admin') > -1);
  }

}
