import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { ClaimManager } from "../../services/claim-manager";
import { EventsService } from "../../services/events-service";

@Component({
  selector: 'app-claim-result',
  templateUrl: './claim-result.component.html',
  styleUrls: ['./claim-result.component.css']
})
export class ClaimResultComponent implements OnInit {

  @Input() expand: Function;
  @Input() minimize: Function;



  constructor(
    public claimManager: ClaimManager,
    private formBuilder: FormBuilder,
    private router: Router,
    private events: EventsService
  ) { }

  ngOnInit() {
    let claimsLength = this.claimManager.claimsData;
    console.log(claimsLength.length);

  }

  view(claimID: Number) {
    this.claimManager.getClaimsDataById(claimID);
    this.events.broadcast('minimize', []);
    //this.minimize();
  }

}
