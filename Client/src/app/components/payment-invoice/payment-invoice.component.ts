import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { HttpService } from "../../services/http-service";
import { PaymentService } from "../../services/payment-service";
import { EventsService } from "../../services/events-service";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
  selector: 'app-payment-invoice',
  templateUrl: './payment-invoice.component.html',
  styleUrls: ['./payment-invoice.component.css']
})
export class PaymentInvoiceComponent implements OnInit, AfterViewInit {

  form: FormGroup;
  submitted: boolean = false;
  constructor(
    public paymentService: PaymentService,
    private formBuilder: FormBuilder,
    private http: HttpService,
    private router: Router,
    private events: EventsService,
    private toast: ToastsManager
  ) {
    this.form = this.formBuilder.group({
      claimNumber: [null],
      firstName: [null],
      lastName: [null],
      rxDate: [null],
      invoiceNumber: [null]
    });
  }
  ngOnInit() {

  }
  ngAfterViewInit() {
    //Date picker
    window['jQuery']('#datepicker').datepicker({
      autoclose: true
    });
    window['jQuery']("#datemask").inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" });
    window['jQuery']("[data-mask]").inputmask();
  }

  textChange(controlName: string) {
    if (this.form.get(controlName).value === 'undefined' || this.form.get(controlName).value === '') {
      this.form.get(controlName).setValue(null);
    }
  }
  search() {
    let form = this.form.value;
    if ((window['jQuery']('#datepicker').val())) {
      form.rxDate = window['jQuery']('#datepicker').val();
    }
    this.paymentService.search(this.form.value, false);
  }
  refresh() {
    var form = this.form.value;
    if ((window['jQuery']('#datepicker').val())) {
      form.rxDate = window['jQuery']('#datepicker').val();
    }
    this.paymentService.search(form, false);
  }
  clear() {
    this.paymentService.clearClaimsData();
    this.form.patchValue({
      claimNumber: null,
      firstName: null,
      lastName: null,
      rxDate: null,
      invoiceNumber: null
    });
  }

}
