import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { HttpService } from "../../../services/http-service";
import { PaymentService } from "../../../services/payment-service";
import { EventsService } from "../../../services/events-service";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-new-payment-input',
  templateUrl: './new-payment-input.component.html',
  styleUrls: ['./new-payment-input.component.css']
})
export class NewPaymentInputComponent implements OnInit {
  form: FormGroup;
  submitted: boolean = false;
  checkAmount: number = 0;
  constructor(private decimalPipe: DecimalPipe, public paymentService: PaymentService, private formBuilder: FormBuilder, private http: HttpService, private router: Router, private events: EventsService, private toast: ToastsManager) {
    this.form = this.formBuilder.group({
      checkNumber: [null],
      checkAmount: [null],
      amountSelected: [null],
      amountToPost: [null],
      amountRemaining: [null]
    });
    this.events.on("payment-amountRemaining", a => {
      this.form.get('amountRemaining').setValue(this.decimalPipe.transform(Number(a), "1.2-2"));
      if (a <= 0) {

      }
      this.form.get('amountToPost').setValue(null);
    })
  }
  ngOnInit() {

  }

  search() {

  }

  textChange(controlName: string) {
    if (this.form.get(controlName).value === 'undefined' || this.form.get(controlName).value === '') {
      this.form.get(controlName).setValue(null);
    } else {
      switch (controlName) {
        case 'checkAmount':
        case 'amountToPost':
          var val = this.form.get(controlName).value.replace(",", '');
          this.form.get(controlName).setValue(this.decimalPipe.transform(val, "1.2-2"));
          break;
        default:
          break;

      }
    }
  }
  checkNumber($event) {
    $event = ($event) ? $event : window.event;
    var charCode = ($event.which) ? $event.which : $event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 44 && charCode != 46) {
      return false;
    }
    return true;
  }
  post() {
    var prescriptions = [];
    this.paymentService.detailedClaimsData.forEach(p => {
      if (p.selected) {
        prescriptions.push(p.prescriptionId)
      }
    })
    //this.form.get('amountRemaining').setValue(this.decimalPipe.transform(Number(this.amountRemaining),"1.2-2"));
    var form = this.form.value;
    form.prescriptionIds = prescriptions;
    form.amountRemaining = undefined;
    form.amountToPost = form.amountToPost !== null ? Number(form.amountToPost.replace(",", "")).toFixed(2) : (0).toFixed(2);
    form.amountSelected = Number(form.amountSelected.replace(",", "")).toFixed(2);
    form.checkAmount = Number(form.checkAmount.replace(",", "")).toFixed(2);
    if (form.amountToPost == 0 || form.amountToPost == null) {
      this.toast.warning("You need to specify amount to post");
    } else if (form.amountToPost == form.amountSelected) {
      this.paymentService.post(form);
    } else if (this.paymentService.selected.length > 1) {
      this.toast.warning("Multi-line, partial payments are not supported at this time. Please correct to continue...");
    } else {
      this.paymentService.post(form);
    }
  }
  get amountRemaining(): Number {
    var checkAmount = Number(this.form.get('checkAmount').value.replace(",", ""));
    var amountSelected = Number(this.form.get('amountSelected').value.replace(",", ""));
    var amount = Number((checkAmount ? checkAmount : 0) - amountSelected);
    return amount;
  }

}
