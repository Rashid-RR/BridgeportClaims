import { SortColumnInfo } from '../../directives/table-sort.directive';
import { Component, OnInit, Renderer2, AfterViewInit, NgZone, HostListener, AfterViewChecked, ElementRef, ViewChild } from '@angular/core';
import { ClaimManager } from '../../services/claim-manager';
import { HttpService } from '../../services/http-service';
import { Payment } from '../../models/payment';
import { EventsService } from '../../services/events-service';
import { DatePipe, DecimalPipe } from '@angular/common';
import { ConfirmComponent } from '../confirm.component';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DialogService } from 'ng2-bootstrap-modal';
import { ToastrService } from 'ngx-toastr';
import { ProfileManager } from '../../services/profile-manager';
declare var $: any;

@Component({
  selector: 'app-claim-payment',
  templateUrl: './claim-payment.component.html',
  styleUrls: ['./claim-payment.component.css']
})
export class ClaimPaymentComponent implements OnInit {

  sortColumn: SortColumnInfo;
  editing: Boolean = false;
  loadingPayment: Boolean = false;
  editingPaymentId: any;
  form: FormGroup;
  constructor(
    private rd: Renderer2, private ngZone: NgZone,
    private dp: DatePipe,
    private formBuilder: FormBuilder,
    public claimManager: ClaimManager,
    private events: EventsService,
    private dialogService: DialogService,
    private profileManager: ProfileManager,
    private toast: ToastrService,
    private decimalPipe: DecimalPipe,
    private http: HttpService
  ) {
    this.form = this.formBuilder.group({
      amountPaid: [null],
      checkNumber: [null],
      prescriptionPaymentId: [null],
      prescriptionId: [null],
      datePosted: [null],
    });
    this.claimManager.onClaimIdChanged.subscribe(() => {
      // this.fetchData();
    });

  }

  ngOnInit() {

  }
  showDocument(document: any) {
    if (document.documentId && document.fileName) {
      localStorage.setItem('file-' + document.documentId, JSON.stringify(document));
      window.open('#/main/indexing/indexed-image/' + document.documentId, '_blank');
    }
  }
  onSortColumn(info: SortColumnInfo) {
    this.sortColumn = info;
    this.fetchData();
  }

  update(payment: Payment) {
    this.editing = true;
    this.editingPaymentId = payment.prescriptionPaymentId;
    const checkAmt = Number(payment.checkAmt).toFixed(2);
    const postedDate = this.dp.transform(payment.postedDate, 'shortDate');
    this.form = this.formBuilder.group({
      amountPaid: [checkAmt],
      checkNumber: [payment.checkNumber],
      prescriptionPaymentId: [payment.prescriptionPaymentId],
      prescriptionId: [payment.prescriptionId],
      datePosted: [postedDate],
    });
    setTimeout(() => {
      $('#datePostedPicker').datepicker({
        autoclose: true
      });
      $('#datePostedPicker').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' });
      $('[inputs-mask]').inputmask();
      $('[data-mask]').inputmask();
    }, 500);
  }
  saveButtonClick() {
    const btn = $('#savePaymentButton');
    if (btn.length > 0) {
      $('#savePaymentButton').click();
    }
  }
  datePosted($event) {
  }
  savePayment(payment: Payment) {
    const d = $('#datePostedPicker').val();
    if (this.form.get('amountPaid').value && this.form.get('checkNumber').value && d) {
      this.claimManager.loading = true;
      this.form.controls['datePosted'].setValue(d);
      this.http.updatePrescriptionPayment(this.form.value).subscribe((res:any) => {
        this.toast.success(res.message);
        // this.removePayment(payment);
        this.claimManager.loading = false;
        payment.postedDate = this.form.get('datePosted').value;
        payment.checkAmt = this.form.get('amountPaid').value !== null ? this.form.get('amountPaid').value.replace(new RegExp(',', 'gi'), '') : 0;
        payment.checkNumber = this.form.get('checkNumber').value;
        this.cancel();
      }, error => {
        this.toast.error(error.message);
        this.claimManager.loading = false;
      });
    } else {
      this.toast.warning('You must fill amount paid, check Number and date posted to continue');
    }
  }
  validateNumber($event) {
    $event = ($event) ? $event : window.event;
    const charCode = ($event.which) ? $event.which : $event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode !== 44 && charCode !== 46) {
      return false;
    }
    return true;
  }
  textChange(controlName: string) {
    if (this.form.get(controlName).value === 'undefined' || this.form.get(controlName).value === '') {
      this.form.get(controlName).setValue(null);
    } else {
      switch (controlName) {
        case 'amountPaid':
          const val = this.form.get(controlName).value.replace(new RegExp(',', 'gi'), '');
          this.form.get(controlName).setValue(this.decimalPipe.transform(val, '1.2-2'));
          break;
        default:
          break;

      }
    }
  }
  get allowed(): Boolean {
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
      && this.profileManager.profile.roles.indexOf('Admin') > -1);
  }
  cancel() {
    this.editing = false;
    this.editingPaymentId = undefined;
    this.form.patchValue({
      amountPaid: [null],
      checkNumber: [null],
      prescriptionPaymentId: [null],
      prescriptionId: [null],
      datePosted: [null],
    });
  }

  del(payment: Payment) {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Delete payment',
      message: 'Are you sure you wish to remove this Payment  for Invoice Number: ' +
        payment.invoiceNumber + ' of $' + payment.checkAmt + '?'
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.claimManager.loading = true;
          this.http.deletePrescriptionPayment(payment.prescriptionPaymentId).subscribe((res:any) => {
            this.toast.success(res.message);
            this.removePayment(payment);
            this.claimManager.loading = false;
          }, error => {
            this.toast.error(error.message);
            this.claimManager.loading = false;
          });
        } else { }
      });
  }

  removePayment(payment) {
    for (let i = 0; i < this.claimManager.selectedClaim.payments.length; i++) {
      if (payment.prescriptionPaymentId === this.claimManager.selectedClaim.payments[i].prescriptionPaymentId && this.claimManager.selectedClaim.payments[i].prescriptionId === payment.prescriptionId) {
        this.claimManager.selectedClaim.payments.splice(i, 1);
      }
    }
  }
  fetchData() {
    this.claimManager.loadingPayment = true;
    const page = 1;
    const page_size = 1000;
    let sort = 'RxDate';
    let sort_dir: 'asc' | 'desc' = 'desc';
    if (this.sortColumn) {
      sort = this.sortColumn.column;
      sort_dir = this.sortColumn.dir;
    }
    this.loadingPayment = true;
    this.http.getPayments(this.claimManager.selectedClaim.claimId, sort, sort_dir,
      page, page_size)
      .subscribe((results:any) => {
        this.claimManager.selectedClaim.setPayment(results);
        this.loadingPayment = false;
        this.claimManager.loadingPayment = false;
      });
  }

}
