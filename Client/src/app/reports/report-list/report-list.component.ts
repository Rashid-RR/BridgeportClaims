import { Component, OnInit } from '@angular/core';
import { ReportLoaderService } from '../../services/services.barrel';
import { ProfileManager } from '../../services/profile-manager';

@Component({
  selector: 'app-report-list',
  templateUrl: './report-list.component.html',
  styleUrls: ['./report-list.component.css']
})
export class ReportListComponent implements OnInit {

  constructor(private profileManager: ProfileManager, public reportloader: ReportLoaderService) {}

  ngOnInit() {
    this.reportloader.current = ' ';
    this.reportloader.currentURL = '';
    this.reportloader.loading = false;
  }

  get adminOnly(): Boolean {
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles
      instanceof Array) && this.profileManager.profile.roles.indexOf('Admin') > -1);
  }

  get indexerOnly(): boolean {
    const allowed = (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
                    && (this.profileManager.profile.roles.indexOf('Admin') > -1 || this.profileManager.profile.roles.indexOf('Indexer') > -1));
    return allowed;
  }
}
