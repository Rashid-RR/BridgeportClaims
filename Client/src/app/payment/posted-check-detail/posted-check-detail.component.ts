import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ToastsManager, Toast } from 'ng2-toastr';
import { CurrencyPipe, DecimalPipe } from '@angular/common';
import { DocumentManagerService } from '../../services/document-manager.service';
import { HttpService } from '../../services/http-service';
import { DocumentItem } from '../../models/document';
import { IShContextMenuItem, BeforeMenuEvent } from 'ng2-right-click-menu/src/sh-context-menu.models';
import { DialogService } from 'ng2-bootstrap-modal/dist/dialog.service';
import { DeleteIndexConfirmationComponent } from '../delete-index-confirmation.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmComponent } from '../../components/confirm.component';
declare var $: any;

@Component({
  selector: 'app-posted-check-detail',
  templateUrl: './posted-check-detail.component.html',
  styleUrls: ['./posted-check-detail.component.css']
})
export class PostedCheckDetailComponent implements OnInit, AfterViewInit {

  goToPage: any = '';
  activeToast: Toast;
  items: IShContextMenuItem[];
  editing: any;
  form: FormGroup;
  lastForm: any;
  constructor(
    private cp: CurrencyPipe,
    private decimalPipe: DecimalPipe,
    private formBuilder: FormBuilder,
    public ds: DocumentManagerService,
    public http: HttpService,
    private toast: ToastsManager,
    private dialogService: DialogService) {
    this.form = this.formBuilder.group({
      amountPaid: ['', Validators.required],
      checkNumber: ['', Validators.required],
      prescriptionPaymentId: ['', Validators.required],
      prescriptionId: [''],
      datePosted: ['', Validators.required],
    });
  }

  ngOnInit() {
    this.items = [
      {
        label: '<span class="fa fa-trash text-red">View Details</span>',
        onClick: ($event) => {
          this.view($event.menuItem.id);
        }
      },
      {
        label: '<span class="fa fa-trash text-red">Remove</span>',
        onClick: ($event) => {
          this.remove($event.menuItem.id);
        }
      }
    ];
  }

  onBefore(event: BeforeMenuEvent, id) {
    event.open([
      {
        id: id,
        label: '<span class="text-primary">View Details</span>',
        onClick: ($event) => {
          this.view($event.menuItem.id);
        }
      },
      {
        id: id,
        label: '<span class="fa fa-trash text-red">Remove</span>',
        onClick: ($event) => {
          this.remove($event.menuItem.id);
        }
      }
    ]);
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
  savePayment(payment: any) {
    if (!this.form.valid) {
      this.toast.warning('You must fill amount paid, check Number and date posted to continue');
    } else if (!this.form.dirty) {
      this.toast.warning('Not saving. You haven\'t made any change');
    } else {
      this.ds.loading = true;
      this.http.updatePrescriptionPayment(this.form.value).single().subscribe(res => {
        this.toast.success(res.message);
        this.ds.loading = false;
        payment.datePosted = this.form.get('datePosted').value;
        payment.amountPaid = this.form.get('amountPaid').value || 0;
        payment.checkNumber = this.form.get('checkNumber').value;
        this.cancel();
      }, error => {
        this.toast.error(error.message);
        this.ds.loading = false;
      });
    }
  }
  cancel() {
    this.editing = undefined;
    this.form.patchValue({
      amountPaid: [null],
      checkNumber: [null],
      prescriptionPaymentId: [null],
      datePosted: [null]
    });
  }
  formatDate(input: String) {
    if (!input) {
      return null;
    }
    if (input.indexOf('-') > -1) {
      const date = input.split('T');
      const d = date[0].split('-');
      return d[1] + '/' + d[2] + '/' + d[0];
    } else {
      return input;
    }
  }
  edit(pay) {
    this.editing = pay.prescriptionPaymentId;
    const datePosted = this.formatDate(pay.datePosted as any);
    this.form.patchValue({
      amountPaid: pay.amountPaid,
      checkNumber: pay.checkNumber,
      prescriptionPaymentId: pay.prescriptionPaymentId,
      datePosted: datePosted,
    });
    this.lastForm = {
      amountPaid: pay.amountPaid,
      checkNumber: pay.checkNumber,
      prescriptionPaymentId: pay.prescriptionPaymentId,
      prescriptionId: pay.amountPaid,
      datePosted: datePosted,
    };
    setTimeout(() => {
      $('[data-mask]').inputmask();
      $('#datePosted').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' }).on('change', ($ev) => {
        this.form.controls.datePosted.setValue($ev.target.value);
        this.form.controls.datePosted.markAsDirty();
      });
    }, 100);
  }
  next() {
    this.ds.searchCheckes(true);
    this.goToPage = '';
  }
  openFile(file: DocumentItem) {
    localStorage.setItem('file-' + file.documentId, JSON.stringify(file));
    window.open(`#/main/indexing/indexed-image/${file.documentId}`);
  }
  view(file: DocumentItem) {
    this.ds.viewPostedDetail = true;
  }
  remove(file: DocumentItem) {
    const amount = this.cp.transform(file['amountPaid'], 'USD', true);
    this.dialogService.addDialog(ConfirmComponent, {
      message: 'Are you sure you wish to remove this Payment  for RX Number: ' +
        file.rxNumber + ' of ' + amount + '?'
    }).subscribe((isConfirmed) => {
      if (isConfirmed) {
        // this.ds.deleteAndKeep(file.documentId, isConfirmed);
        this.http.deletePrescriptionPayment(file['prescriptionPaymentId']).single().subscribe(res => {
          this.toast.success(res.message);
          for (let i = 0; i < this.ds.viewPostedDetail.length; i++) {
            if (file['prescriptionPaymentId'] === this.ds.viewPostedDetail[i].prescriptionPaymentId) {
              this.ds.viewPostedDetail.splice(i, 1);
            }
          }

        }, error => {
          this.toast.error(error.message);
        });
      }
    });
  }
  goto() {
    const page = Number.parseInt(this.goToPage);
    if (!this.goToPage) {

    } else if (page > 0 && page <= this.ds.checkTotalPages) {
      this.ds.searchCheckes(false, false, page);
    } else {
      if (this.activeToast && this.activeToast.timeoutId) {
        this.activeToast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.ds.checkTotalPages;
      } else {
        this.toast.warning('Page number entered is out of range. Enter a page number between 1 and '
          + this.ds.checkTotalPages).then((toast: Toast) => {
            this.activeToast = toast;
          });
      }
    }
  }
  prev() {
    this.ds.searchCheckes(false, true);
    this.goToPage = '';
  }
  keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;
    const inputChar = String.fromCharCode(event.charCode);
    const input = Number(this.goToPage + '' + inputChar);
    if (!pattern.test(inputChar)) {
      event.preventDefault();
    } else if (!this.isNumeric(input)) {
      event.preventDefault();
    } else if (input < 1) {
      event.preventDefault();
    }
  }
  isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
  }

  ngAfterViewInit() {

  }
}
