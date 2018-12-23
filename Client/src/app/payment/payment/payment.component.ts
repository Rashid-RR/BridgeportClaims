import { Component, ViewChild, OnInit, trigger, state, style, transition, animate } from '@angular/core';
import { PaymentService, PaymentScriptService } from '../../services/services.barrel';
import { EventsService } from '../../services/events-service';
import { ToastsManager } from 'ng2-toastr';
import { ConfirmComponent } from '../../components/confirm.component';
import { DialogService } from 'ng2-bootstrap-modal';
import { UUID } from 'angular2-uuid';
import { PaymentPostingPrescription } from '../../models/payment-posting-prescription';
import { DatePipe } from '@angular/common';
import { SwalComponent, SwalPartialTargets } from '@toverux/ngx-sweetalert2';
import swal from 'sweetalert2';
import { ActivatedRoute } from '@angular/router';
import { DocumentManagerService } from '../../services/document-manager.service';

declare var $: any;

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css'],
  animations: [
    trigger('slideInOut', [
      state('out', style({
        transform: 'translate3d(-100%, 0, 0)'
      })),
      state('in', style({
        transform: 'translate3d(0, 0, 0)'
      })),
      transition('in => out', animate('400ms ease-in-out')),
      transition('out => in', animate('400ms ease-in-out'))
    ]),
  ]
})
export class PaymentComponent implements OnInit {

  over: boolean[];
  flag = 'test';
  tabState = 'in';
  documentId: any;
  checkNumber: any;
  document: any;
  @ViewChild('addScriptSwal') private addScriptSwal: SwalComponent;
  constructor(public readonly swalTargets: SwalPartialTargets, public paymentScriptService: PaymentScriptService, public paymentService: PaymentService,
    public ds: DocumentManagerService, private route: ActivatedRoute, private dialogService: DialogService, private toast: ToastsManager, private events: EventsService, private dp: DatePipe) {
    this.route.params.subscribe(params => {
      if (params['checkNumber']) {
        this.checkNumber = params['checkNumber'];
      }
      if (params['documentId']) {
        this.documentId = params['documentId'];
        this.showDocument();
      }
    });
    this.over = new Array(2);
    this.over.fill(false);
    this.events.on('payment-suspense', () => {
      this.tabState = 'in';
      this.paymentService.documentWindow.forEach(win => {
        win.close();
      });
      this.ds.searchCheckes();
    });
    this.events.on('payment-closed', () => {
      this.tabState = 'in';
      this.paymentService.documentWindow.forEach(win => {
        win.close();
      });

      this.ds.searchCheckes();
    });
    this.events.on('payment-updated', (b: Boolean) => {
      try {
        swal.clickConfirm();
      } catch (e) { }
      if (b) {
        setTimeout(() => { this.showModal(true); }, 100);
      }
    });

    this.events.on('show-payment-script-modal', (b: Boolean) => {
      setTimeout(() => { this.showModal(); }, 100);
    });
  }
  showDocument() {
    const file = localStorage.getItem('file-' + this.documentId);
    if (file) {
      try {
        const doc = JSON.parse(file) as any;
        this.document = doc;
      } catch (e) { }
    }
    const win = window.open('#/main/indexing/indexed-image/' + this.documentId, '_blank');
    this.paymentService.documentWindow.push(win);
  }

  ngOnInit() {
  }
  showModal(reload: boolean = false) {
    if (!reload) {
      this.paymentService.clearClaimsData();
      this.paymentScriptService.form.reset();
    }
    this.addScriptSwal.show().then((r) => {

    });
  }

  viewClaims() {
    const selectedClaims = [];
    const checkboxes = window['jQuery']('.claimCheckBox');
    for (let i = 0; i < checkboxes.length; i++) {
      if (window['jQuery']('#' + checkboxes[i].id).is(':checked')) {
        selectedClaims.push(Number(checkboxes[i].id));
      }
    }
    if (selectedClaims.length === 0) {
      this.toast.warning('Please select one or more claims in order to view prescriptions.');
    } else {
      this.paymentService.prescriptionSelected = true;
      this.paymentService.clearClaimsDetail();
      this.paymentService.getPaymentClaimDataByIds(selectedClaims);
    }
  }

  toggleTab() {
    this.tabState = this.tabState === 'out' ? 'in' : 'out';
  }

  deletePayment(prescription: PaymentPostingPrescription, sessionId: UUID) {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Delete Payment',
      message: 'Are you sure you wish to remove this Payment Posting for ' + prescription.patientName + ' of $' + prescription.amountPosted + '?'
    })
      .subscribe((isConfirmed) => {
        // We get dialog result
        if (isConfirmed) {
          this.paymentService.deletePayment({ sessionId: this.paymentService.paymentPosting.sessionId, id: prescription.id, prescriptionId: prescription.prescriptionId });
        } else {

        }
      });
  }

  formatDate(input: String) {
    const d = input.toString().substring(0, 10);
    const date = this.dp.transform(d, 'MM/dd/y');
    return date;
  }

}
