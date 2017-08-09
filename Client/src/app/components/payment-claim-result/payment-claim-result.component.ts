import { Component, OnInit,NgZone } from '@angular/core';
import {HttpService} from "../../services/http-service";
import {PaymentService} from "../../services/payment-service";
import {EventsService} from "../../services/events-service";
import {PaymentClaim} from "../../models/payment-claim";
import {ToastsManager } from 'ng2-toastr/ng2-toastr';
import {Router} from "@angular/router";

declare var jQuery:any;


@Component({
  selector: 'app-payment-claim-result',
  templateUrl: './payment-claim-result.component.html',
  styleUrls: ['./payment-claim-result.component.css']
})
export class PaymentClaimResultComponent implements OnInit {
  checkAll:Boolean=false;
  constructor(private ngZone:NgZone,public paymentService:PaymentService, private http: HttpService, private router: Router, private events: EventsService,private toast: ToastsManager) { }

  ngOnInit() {
  }
  select(invoice:any,$event){
    invoice.selected = $event.target.checked
  }

  showNotes(){

  }
  activateClaimCheckBoxes(){
    jQuery('#claimsCheckBox').click();
  }
  claimsCheckBox($event){    
     this.checkAll =  $event.target.checked;    
     if(this.checkAll){
       this.paymentService.claimsData.forEach(claim=>{
         claim.selected = false;
       })
     }else{
       this.paymentService.claimsData.forEach(claim=>{
         claim.selected = false;
       });
     }
  }
  prescriptionsCheckBox(invoice:any,$event){
      if($event.target.checked){

      }
  }
}
