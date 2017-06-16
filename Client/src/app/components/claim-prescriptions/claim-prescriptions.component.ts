import { Component, OnInit } from '@angular/core';
import {ClaimManager} from "../../services/claim-manager";

@Component({
  selector: 'app-claim-prescriptions',
  templateUrl: './claim-prescriptions.component.html',
  styleUrls: ['./claim-prescriptions.component.css']
})
export class ClaimPrescriptionsComponent implements OnInit {

  constructor(public claimManager:ClaimManager) { }

  ngOnInit() {
  }

}
