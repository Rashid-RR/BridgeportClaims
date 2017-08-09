import { Component, OnInit } from '@angular/core';
import {PaymentService} from "../../services/payment-service";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent implements OnInit {

  constructor(public paymentService:PaymentService,private toast:ToastsManager) { }

  ngOnInit() {

  }

  viewClaims(){
    let selectedClaims = [];
    var checkboxes = window['jQuery']('.claimCheckBox');
    for (var i = 0; i < checkboxes.length; i++) {
      if (window['jQuery']("#" + checkboxes[i].id).is(':checked')) {
        selectedClaims.push(Number(checkboxes[i].id));
      }
    }
    if (selectedClaims.length == 0) {
      this.toast.warning('Please select at least one claim');
    }else{
      this.paymentService.prescriptionSelected=true
      this.paymentService.clearClaimsDetail();
      this.paymentService.getPaymentClaimDataByIds(selectedClaims);
    }
  }
}
