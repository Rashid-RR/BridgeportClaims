import { Component, OnInit, trigger, state, style, transition, animate } from "@angular/core";
import { EventsService } from "../../services/events-service";
import { ProfileManager } from "../../services/profile-manager";
import { ClaimManager } from "../../services/claim-manager";
import { HttpService } from "../../services/http-service";
import { Router } from "@angular/router";

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
  animations: [
    trigger('slideInOut', [
      state('out', style({
        transform: 'translate3d(-100%, 0, 0) translateY(-50%)'
      })),
      state('in', style({
        transform: 'translate3d(0, 0, 0) translateY(-50%)'
      })),
      transition('in => out', animate('400ms ease-in-out')),
      transition('out => in', animate('400ms ease-in-out'))
    ]),
  ]
})
export class SidebarComponent implements OnInit {

  disableLinks = false;
  tabState = 'in';

  constructor(
    private http: HttpService,
    private events: EventsService,
    private router: Router,
    private profileManager: ProfileManager,
    public claimManager: ClaimManager
  ) { }

  ngOnInit() {
    //this.disableLinks = true;
    this.events.on("disable-links", (status: boolean) => {
      this.disableLinks = status;
    });
  }

  get userName() {
    return this.profileManager.profile ? this.profileManager.profile.firstName + ' ' + this.profileManager.profile.lastName : '';
  }
  get avatar() {
    return this.profileManager.profile ? this.profileManager.profile.avatarUrl : '';
  }

  get allowed(): Boolean {
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array) && this.profileManager.profile.roles.indexOf('Admin') > -1)
  }
  goToClaim(id: Number) {
    if (!this.disableLinks) {
      this.claimManager.search({
        claimNumber: null, firstName: null, lastName: null,
        rxNumber: null, invoiceNumber: null, claimId: id
      }, false);
      if (this.router.url != '/main/claims') {
        this.router.navigate(['/main/claims']);
      }
    }
  }

  toggleTab() {
    this.tabState = this.tabState === 'out' ? 'in' : 'out';
  }

}
