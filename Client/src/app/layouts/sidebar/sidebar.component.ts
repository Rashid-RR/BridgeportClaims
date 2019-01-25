import { Component, OnInit} from '@angular/core';
import { EventsService } from '../../services/events-service';
import { ProfileManager } from '../../services/profile-manager';
import { ClaimManager } from '../../services/claim-manager';
import { HttpService } from '../../services/http-service';
import { Router } from '@angular/router';
declare var $: any;

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {

  disableLinks = false;
  constructor(
    private events: EventsService,
    public router: Router,
    private profileManager: ProfileManager,
    public claimManager: ClaimManager
  ) { }

  ngOnInit() {
    this.events.on('disable-links', (status: boolean) => {
      this.disableLinks = status;
    });
  }

  claimsActive() {
    if (this.router.url.indexOf('/main/claims') > -1) {
      this.events.broadcast('clear-claims', true);
      this.events.broadcast('disable-links', false);
    } else if (!this.disableLinks) {
      this.router.navigate(['/main/claims']);
    }
  }

  get userName() {
    return this.profileManager.profile ? this.profileManager.profile.firstName + ' ' + this.profileManager.profile.lastName : '';
  }
  get avatar() {
    return this.profileManager.profile ? this.profileManager.profile.avatarUrl : '';
  }

  get allowed(): Boolean {
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
      && this.profileManager.profile.roles.indexOf('Admin') > -1);
  }
  get adminOrAsociate(): Boolean {
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
      && (this.profileManager.profile.roles.indexOf('Admin') > -1 || this.profileManager.profile.roles.indexOf('Indexer') > -1));
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


  get isClient(): Boolean {
    return (this.profileManager.profile && this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
      && this.profileManager.profile.roles.indexOf('Client') > -1);
  }

  get isOnlyUser(): Boolean {
    return (this.profileManager.profile && this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
      && this.profileManager.profile.roles.indexOf('Client') === -1
      && this.profileManager.profile.roles.indexOf('User') > -1
      && this.profileManager.profile.roles.indexOf('Admin') === -1
      && this.profileManager.profile.roles.indexOf('Indexer') === -1);
  }

}
