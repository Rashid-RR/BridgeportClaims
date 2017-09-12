import { Component, OnInit, trigger, state, style, transition, animate } from '@angular/core';
import { PaymentService, PaymentScriptService } from "../../services/services.barrel";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css'],
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
export class PaymentComponent implements OnInit {

  tabState = 'in';

  constructor(public paymentScriptService: PaymentScriptService, public paymentService: PaymentService, private toast: ToastsManager) { }

  ngOnInit() {

  }

  viewClaims() {
    let selectedClaims = [];
    var checkboxes = window['jQuery']('.claimCheckBox');
    for (var i = 0; i < checkboxes.length; i++) {
      if (window['jQuery']("#" + checkboxes[i].id).is(':checked')) {
        selectedClaims.push(Number(checkboxes[i].id));
      }
    }
    if (selectedClaims.length == 0) {
      this.toast.warning('Please select one or more claims in order to view prescriptions.');
    } else {
      this.paymentService.prescriptionSelected = true
      this.paymentService.clearClaimsDetail();
      this.paymentService.getPaymentClaimDataByIds(selectedClaims);
    }
  }

  toggleTab() {
    this.tabState = this.tabState === 'out' ? 'in' : 'out';
  }
  
}
