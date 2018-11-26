import { Component, OnInit, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { Toast, ToastsManager } from 'ng2-toastr';
import { PaymentScriptService } from '../../services/payment-script-service';
import { Subject } from 'rxjs/Subject';
import { DatePipe, DecimalPipe } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../services/http-service';
import { Prescription } from '../../models/prescription';
import { DialogService } from 'ng2-bootstrap-modal';
import { ConfirmComponent } from '../confirm.component';

declare var $: any;

@Component({
  selector: 'invoice-search-modal',
  templateUrl: './invoice-search-modal.component.html',
  styleUrls: ['./invoice-search-modal.component.css']
})
export class InvoiceSearchComponent implements OnInit, AfterViewInit {

  @ViewChild('lastname') lastname: ElementRef;
  loading = false;
  exactMatch = false;
  searchText = '';
  editing: Prescription;
  amount: number;
  placeholder = 'Start typing to search claims...';
  dropdownVisible = false;
  showDropDown = new Subject<any>();
  form: FormGroup;
  public prescriptions: Prescription[];

  constructor(
    private formBuilder: FormBuilder,
    public payment: PaymentScriptService,
    private toast: ToastsManager,
    private dialogService: DialogService,
    private dp: DatePipe,
    private decPipe: DecimalPipe,
    private http: HttpService) {
    this.form = this.formBuilder.group({
      claimNumber: [null],
      invoiceNumber: [null],
      rxDate: [null],
      rxNumber: [null],
      claimId: [null, Validators.compose([Validators.required])]
    });
  }

  ngOnInit() {

  }
  ngAfterViewInit() {
    $('#rxDate').datepicker({
      autoclose: true
    });
    // this.lastname.nativeElement.focus();
  }
  search() {
    if (!this.form.valid) {
      this.toast.warning('Please link a claim to search');
    } else {
      const form = this.form.value;
      const rxDate = this.dp.transform($('#rxDate').val(), 'MM/dd/yyyy');
      form.rxDate = rxDate;
      this.loading = true;
      this.http.invoiceAmounts(form).single().subscribe(res => {
        this.loading = false;
        this.prescriptions = res;
        if (res && res.length === 0) {
          this.toast.info('No records found from this search criteria');
        }
      }, err => {
        this.loading = false;
        this.toast.error(err.message);
      });
    }
  }
  save() {
    if (this.amount === this.editing['billedAmount']) {
      this.toast.warning('You haven\'t changed the Billed Amount value, and therefore, there is nothing to save');
    } else {
      const amount = this.amount.toString().replace(/,/g, '');
      const from = this.decPipe.transform(this.editing['billedAmount'], '.2');
      const toAmount = this.decPipe.transform(amount, '.2');
      this.dialogService.addDialog(ConfirmComponent, {
        title: 'Update Billed Amount',
        message: `Are you sure you wish to update this Billed Amount for Rx # ${this.editing['rxNumber']} from $${from} to $${toAmount}?`
      })
        .subscribe((isConfirmed) => {
          if (isConfirmed) {
            this.loading = true;
            this.http.updateBilledAmount({ prescriptionId: this.editing.prescriptionId,
               billedAmount: amount }).single().subscribe(res => {
              const p = this.prescriptions.find(p => p.prescriptionId === this.editing.prescriptionId);
              if (p) {
                p['billedAmount'] = amount;
              }
              this.toast.success(res.message, null, { toastLife: 5500 });
              this.cancel();
              this.loading = false;
            }, error => {
              this.toast.error(error.message);
              this.loading = false;
            });
          }
        });
    }
  }
  validateNumber($event) {
    $event = ($event) ? $event : window.event;
    const charCode = ($event.which) ? $event.which : $event.keyCode;
    if (!this.amount && charCode === 46) {
      return false;
    }
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode !== 45 && charCode !== 46) {
      return false;
    }
    return true;
  }
  validateAfter($event) {
    if ((this.amount && this.amount.toString().length > 1 && this.amount.toString().lastIndexOf('-') > 0) ||
      (this.amount && this.amount.toString().length > 1 && this.amount.toString().lastIndexOf('.') > this.amount.toString().indexOf('.')) ||
      (this.amount && this.amount && this.amount.toString().lastIndexOf('.') > -1 &&
       this.amount.toString().length - this.amount.toString().lastIndexOf('.') > 3) ||
      (this.amount && this.amount.toString().length === 1 && this.amount.toString().lastIndexOf('.') > 0)
    ) {
      const num = String(this.amount);
      this.amount = isNaN(Number(num.substring(0, num.length - 1))) ? null : Number(num.substring(0, num.length - 1));
    }
  }
  update(p) {
    this.editing = p;
    this.amount = p.billedAmount;
   // $('.money').mask("#,##0.00", {reverse: true});

  }
  cancel() {
    this.editing = undefined;
    this.amount = undefined;
  }
  clear() {
    this.placeholder = 'Start typing to search claims...';
    $('#rxDate').val('').datepicker('update');
    this.cancel();
    this.form.reset();
  }
  checkMatch($event) {
    this.exactMatch = $event.target.checked;
    this.showDropDown.next($event.target.checked);
  }
  lastInput($event) {
    this.searchText = $event.target.value;
  }
  get autoCompleteClaim(): string {
    return this.http.baseUrl + '/document/claim-search/?exactMatch=' + this.exactMatch + '&searchText=:keyword';
  }
  claimSelected($event) {
    if (this.searchText && $event.claimId) {
      this.form.patchValue({
        // episodeId: this.episodeService.episodetoAssign.episodeId,
        claimNumber: $event.claimNumber,
        claimId: $event.claimId
      });
      this.toast.info('Episode will be linked to ' + $event.lastName + ' ' + $event.firstName + ' ' + $event.claimNumber,
       'Claim Link ready to save', { enableHTML: true, showCloseButton: true, positionClass: 'toast-top-center' })
        .then((toast: Toast) => {
          const toasts: Array<HTMLElement> = $('.toast-message');
          for (let i = 0; i < toasts.length; i++) {
            const msg = toasts[i];
            if (msg.innerHTML === toast.message) {
              msg.parentNode.parentElement.style.left = 'calc(50vw - 200px)';
              msg.parentNode.parentElement.style.position = 'fixed';
            }
          }
        });
      setTimeout(() => {
        this.placeholder = $event.lastName + ' ' + $event.firstName + ' ~ ' + $event.claimNumber;
        this.searchText = undefined;
        this.dropdownVisible = false;
      }, 100);
    }
  }
}
