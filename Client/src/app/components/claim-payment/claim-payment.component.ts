import { Component, OnInit } from '@angular/core';
import {ClaimManager} from "../../services/claim-manager";

@Component({
  selector: 'app-claim-payment',
  templateUrl: './claim-payment.component.html',
  styleUrls: ['./claim-payment.component.css']
})
export class ClaimPaymentComponent implements OnInit {

  constructor(public claimManager:ClaimManager) { 
    
  }

  ngOnInit() {
     
  }

}
