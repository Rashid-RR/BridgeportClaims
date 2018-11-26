import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { HttpService } from "../../services/http-service";
import { PaymentService } from "../../services/payment-service";
import { EventsService } from "../../services/events-service";
import { PaymentPosting } from '../../models/payment-posting';
import { ToastsManager } from 'ng2-toastr';
import { DecimalPipe } from '@angular/common';
import { DialogService } from "ng2-bootstrap-modal";
import { ConfirmComponent } from '../../components/confirm.component';
import * as Immutable from 'immutable';
import { DetailedPaymentClaim } from '../../models/detailed-payment-claim';
import { PaymentClaim } from '../../models/payment-claim';
import swal from 'sweetalert2';

@Component({
  selector: 'app-payment-input',
  templateUrl: './payment-input.component.html',
  styleUrls: ['./payment-input.component.css']
})
export class PaymentInputComponent implements OnInit, OnDestroy {

  form: FormGroup;
  submitted = false;
  disableCheckEntry = false;
  checkAmount = 0;
  paymentamountRemaining: any
  paymentSuspense: any;
  paymentClosed: any;
  documentId: any;
  constructor(private decimalPipe: DecimalPipe, public paymentService: PaymentService,
    private formBuilder: FormBuilder, private http: HttpService, private router: Router, private events: EventsService, private toast: ToastsManager,
    private route: ActivatedRoute, private dialogService: DialogService) {
    this.form = this.formBuilder.group({
      checkNumber: [null],
      documentId: [null, Validators.required],
      fileName: [null],
      fileUrl: [null],
      checkAmount: [null],
      amountSelected: [null],
      amountToPost: [null],
      amountRemaining: [null],
      prescriptionIds: [[]]
    });
  }
  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params['documentId']) {
        this.documentId = params['documentId']
        this.form.patchValue({ documentId: params['documentId'] });
        const file = localStorage.getItem('file-' + this.documentId);
        if (file) {
          try {
            let doc = JSON.parse(file) as any;
            this.form.patchValue({ fileName: doc.fileName, fileUrl: decodeURIComponent(doc.fileUrl) });
          } catch (e) { }
        }
      }
      if (params['checkNumber']) {
        this.form.patchValue({ checkNumber: params['checkNumber'] });
      }
    });
    this.paymentService.paymentPosting = new PaymentPosting();
    this.paymentSuspense = this.events.on("payment-suspense", a => {
      this.form.patchValue({
        checkNumber: null,
        checkAmount: Number(0).toFixed(2),
        amountSelected: Number(0).toFixed(2),
        amountToPost: null,
        amountRemaining: null
      });
      this.paymentService.paymentPosting = new PaymentPosting();
      this.disableCheckEntry = false;
      this.events.broadcast('disable-links', false);
    });
    this.paymentClosed = this.events.on('payment-closed', a => {
      this.form.patchValue({
        checkNumber: null,
        checkAmount: Number(0).toFixed(2),
        amountSelected: Number(0).toFixed(2),
        amountToPost: null,
        amountRemaining: null,
        prescriptionIds: []
      });
      this.paymentService.paymentPosting = new PaymentPosting();
      this.disableCheckEntry = false;
    });
    this.paymentamountRemaining = this.events.on('payment-amountRemaining', a => {
      this.form.get('amountRemaining').setValue(this.decimalPipe.transform(Number(a.amountRemaining), '1.2-2'));
      const c = Number(this.form.get('checkAmount').value.replace(new RegExp(',', 'gi'), '')).toFixed(2);
      const checkAmount = Number(c);
      let form: any = {};
      form = this.form.value;
      form.checkAmount = Number(form.checkAmount.replace(new RegExp(',', 'gi'), '')).toFixed(2);
      this.paymentService.paymentPosting.sessionId = a.sessionId;
      this.paymentService.paymentPosting.checkAmount = form.checkAmount;
      this.paymentService.paymentPosting.checkNumber = form.checkNumber;
      this.paymentService.paymentPosting.lastAmountRemaining = a.amountRemaining;
      form.paymentPostings = this.paymentService.paymentPosting.paymentPostings;
      form.lastAmountRemaining = a.amountRemaining;
      form.sessionId = this.paymentService.paymentPosting.sessionId;
      /* form.checkAmount = this.paymentService.paymentPosting.checkAmount;
      form.checkNumber = this.paymentService.paymentPosting.checkNumber; */
      form.amountSelected = this.paymentService.paymentPosting.amountSelected;
      if (a.amountRemaining === 0 && this.paymentService.paymentPosting.sessionId) {
        this.finalizePosting('Came from event');
      } else if (a.amountRemaining <= 0) {
        this.events.broadcast('disable-links', false);
        this.paymentService.prescriptionSelected = false;
      } else if (checkAmount - (a.amountRemaining as number) > 0) {
        this.disableCheckEntry = true;
        this.events.broadcast('disable-links', true);
      }
      this.paymentService.unSelectAllPrescriptions();
      this.form.get('amountToPost').setValue(null);
    });
  }
  ngOnDestroy() {
    this.events.flash('payment-amountRemaining');
    this.events.flash('payment-closed');
    this.events.flash('payment-suspense');
  }

  updateAmountRemaining() {
    const amnt = this.form.get('checkAmount').value;
    const amount = Number(amnt.replace(new RegExp(',', 'gi'), ''));
    this.form.get('amountRemaining').setValue(this.decimalPipe.transform(amount, '1.2-2'));
    this.paymentService.paymentPosting.lastAmountRemaining = Number(amount.toFixed(2));
  }

  finalizePosting(txt) {
    if (this.paymentService.paymentPosting.sessionId && this.paymentService.paymentPosting.sessionId !== null) {
      const disposable = this.dialogService.addDialog(ConfirmComponent, {
        title: 'Permanently Save Posting' + (this.paymentService.paymentPosting.paymentPostings.length !== 1 ? 's' : ''),
        message: 'Your  ' + (this.paymentService.paymentPosting.paymentPostings.length) + ' posting' + (this.paymentService.paymentPosting.paymentPostings.length !== 1 ? 's' : '') + ' are ready for saving. Would you like to permanently save now?'
      })
        .subscribe((isConfirmed) => {
          if (isConfirmed) {
            this.paymentService.finalizePosting({ sessionId: this.paymentService.paymentPosting.sessionId });
          }
          else {

          }
        });
    }
  }
  cancel() {
    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: 'Cancel posting',
      message: 'Are you sure you want to cancel ' + this.paymentService.paymentPosting.paymentPostings.length + ' posting' + (this.paymentService.paymentPosting.paymentPostings.length !== 1 ? 's' : '') + '?'
    })
      .subscribe((isConfirmed) => {
        // We get dialog result
        if (isConfirmed) {
          this.paymentService.loading = true;
          this.paymentService.prescriptionSelected = false;
          this.events.broadcast('disable-links', false);
          this.http.cancelPayment(this.paymentService.paymentPosting.sessionId)
            .single().subscribe(res => {
              this.toast.success(res.message);
              this.paymentService.loading = false;
              this.events.broadcast('payment-closed', true);
              this.disableCheckEntry = false;
              this.paymentService.claims = Immutable.OrderedMap<Number, PaymentClaim>();
              this.paymentService.claimsDetail = Immutable.OrderedMap<Number, DetailedPaymentClaim>();
              this.router.navigate([`/main/payments`]);
              // this.paymentService.paymentPosting = new PaymentPosting();
            }, error => {
              this.toast.error(error.message);
              this.paymentService.loading = false;
            });
        } else {

        }
      });
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
          const val = this.form.get(controlName).value.replace(new RegExp(',', 'gi'), '');
          this.form.get(controlName).setValue(this.decimalPipe.transform(val, '1.2-2'));
          if (controlName === 'checkAmount') {
            this.updateAmountRemaining();
          }
          break;
        default:
          break;

      }
    }
  }
  checkNumber($event) {
    $event = ($event) ? $event : window.event;
    const charCode = ($event.which) ? $event.which : $event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode !== 44 && charCode !== 46) {
      return false;
    }
    return true;
  }
  post() {
    const payments = [];
    const form = this.form.value;
    if (this.form.get('checkNumber').value == null) {
      this.toast.warning('The Check # field is mandatory in order to save the payment posting.');
    } else if (form.checkAmount == null || form.checkAmount === 0) {
      this.toast.warning('The Check Amount field is mandatory in order to save the payment posting.');
    } else {
      form.amountRemaining = undefined;
      form.amountToPost = form.amountToPost !== null ? Number((form.amountToPost || 0).replace(new RegExp(',', 'gi'), '')).toFixed(2) : (0).toFixed(2);

      this.paymentService.detailedClaimsData.forEach(p => {
        if (p.selected) {
          const amountToPost = this.paymentService.selected.length > 1 ? p.invoicedAmount : form.amountToPost;
          payments.push({
            patientName: p.patientName,
            rxDate: p.rxDate,
            amountPosted: amountToPost,
            prescriptionId: p.prescriptionId,
            currentOutstanding: p.outstanding
          });
        }
      });

      form.paymentPostings = payments;
      // this.form.get('amountRemaining').setValue(this.decimalPipe.transform(Number(this.amountRemaining),"1.2-2"));
      if (this.paymentService.paymentPosting.lastAmountRemaining === 0 && this.paymentService.paymentPosting.sessionId) {
        this.finalizePosting('Came from posting');
      } else {

        form.amountSelected = Number((String(this.paymentService.amountSelected) || '0').replace(new RegExp(',', 'gi'), '')).toFixed(2);
        form.checkAmount = Number(String(form.checkAmount).replace(new RegExp(',', 'gi'), '')).toFixed(2);
        this.paymentService.paymentPosting.checkAmount = Number(this.paymentService.paymentPosting.checkAmount) ? this.paymentService.paymentPosting.checkAmount : Number(Number((form.checkAmount || '0').replace(new RegExp(',', 'gi'), '')).toFixed(2));
        this.paymentService.paymentPosting.checkNumber = form.checkNumber;
        form.lastAmountRemaining = this.paymentService.paymentPosting.lastAmountRemaining;
        form.sessionId = this.paymentService.paymentPosting.sessionId;
        form.amuontToPost = Number.parseFloat(form.amuontToPost);
        if (this.paymentService.selected.length > 1 && form.amountToPost !== form.amountSelected) {
          this.toast.warning('Multi-line payments are not permitted for posting unless the "Amount Selected" is equal to the "Amount To Post"');
        }/* else if(form.checkAmount > form.amountToPost){
          this.localSt.store("partial-payment",payments);
          this.toast.info("Posting has been saved. Please continue posting until the Check Amount is posted in full before it is saved to the database");
        } */
        else if (this.paymentService.selected.length === 0) {
          this.toast.warning('You cannot post a payment without selecting any prescriptions!');
        } else if (Number(form.amountToPost) > Number(form.checkAmount)) {
          this.toast.warning('You may not post monies that exceed the total check amount;');
        } else if ((Number(form.lastAmountRemaining) - Number(form.amountToPost)) < 0) {
          this.toast.warning('Error. You may not post an amount that puts the \"Amount Remaining\" for this check into the negative.', null,
            { toastLife: 10000 });
        } else if (form.amountToPost === 0 || form.amuontToPost == null) {
          this.toast.warning('You need to specify amount to post');
        } else {
          form.prescriptionIds = [];
          this.paymentService.selected.forEach(p => {
            form.prescriptionIds.push(p.prescriptionId);
          });
          this.paymentService.post(form);
        }
      }
    }
  }
  get amountRemaining(): Number {
    const checkAmount = Number(this.form.get('checkAmount').value.replace(new RegExp(',', 'gi'), ''));
    const amountSelected = Number(this.form.get('amountSelected').value.replace(new RegExp(',', 'gi'), ''));
    const amount = Number((checkAmount ? checkAmount : 0) - amountSelected);
    return amount;
  }
  tosuspense(noteText: String = '') {
    const form = this.form.value;
    form.checkAmount = Number(form.checkAmount.replace(new RegExp(',', 'gi'), '')).toFixed(2);
    const amountSelected = Number(this.form.get('amountSelected').value.replace(new RegExp(',', 'gi'), ''));
    form.lastAmountRemaining = this.paymentService.paymentPosting.lastAmountRemaining;
    form.amountToPost = form.amountToPost !== null ? Number((form.amountToPost || 0).replace(new RegExp(',', 'gi'), '')).toFixed(2) : (0).toFixed(2);
    if (this.form.get('checkNumber').value == null || this.form.get('checkNumber').value === 0)
      this.toast.warning('The Check # field is mandatory in order to conclude the payment posting process.');
    else if (form.checkAmount == null || form.checkAmount === 0)
      this.toast.warning('The Check Amount field is mandatory in order to conclude the payment posting process.');
    else if (this.paymentService.selected.length > 0 || amountSelected > 0)
      this.toast.warning('You have selected ' + this.paymentService.selected.length + ' row' + (this.paymentService.selected.length > 1 ? 's' : '') + ' prescriptions. You cannot associate any prescriptions to an Amount that you are suspending. Please uncheck all prescriptions and retry.');
    else if (Number(form.amountToPost) > Number(form.checkAmount)) {
      this.toast.warning('You cannot post a payment without selecting any prescriptions. You must send any monies without prescriptions to suspense.');
    } else {
      const width = window.innerWidth * 1.799 / 3;
      const amountToSuspend = (form.lastAmountRemaining != null && form.lastAmountRemaining > 0) ? form.lastAmountRemaining : 0;

      const amount = this.decimalPipe.transform(Number(amountToSuspend), '1.2-2');
      swal({
        title: 'Postings To Suspense',
        width: width + 'px',
        html:
          `<p>
            Are you ready to post ` + this.paymentService.paymentPosting.paymentPostings.length + ` posting` + (this.paymentService.paymentPosting.paymentPostings.length !== 1 ? 's' : '') + ` and an amount of $` + amount + ` to suspense?
          </p>
          <div class="form-group">
                  <label id="noteTextLabel">You may optionally enter a suspense note below:</label>
                  <textarea class="form-control"  id="noteText" rows="5" cols="5"  style="resize: vertical;font-size:12pt;">` + noteText + `</textarea>
              </div>
            `,
        showCancelButton: true,
        showLoaderOnConfirm: true,
        confirmButtonText: 'Save',
        preConfirm: function () {
          return new Promise(function (resolve) {
            resolve([
              window['jQuery']('#noteText').val()
            ]);
          });
        },
        onOpen: function () {
          window['jQuery']('#noteText').focus();
        }
      }).then((results) => {
        if (!results.dismiss) {
          const result = results.value;
          this.confirmSuspense(amountToSuspend, result[0]);
        }
      }).catch(swal.noop);
    }

  }
  confirmSuspense(amountToSuspend: Number, text: String) {
    const form = this.form.value;
    this.paymentService.paymentToSuspense({ documentId: form.documentId, checkNumber: form.checkNumber, sessionId: this.paymentService.paymentPosting.sessionId, amountToSuspense: amountToSuspend, noteText: text });
  }

}
