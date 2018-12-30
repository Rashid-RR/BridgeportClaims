import { UUID } from 'angular2-uuid';
import * as Immutable from 'immutable';
import { Observable } from 'rxjs/Observable';
import { PaymentClaim } from '../models/payment-claim';
import { PrescriptionNoteType } from '../models/prescription-note-type';
import { Injectable, NgZone } from '@angular/core';
import { PaymentService } from './payment-service';
import { HttpService } from './http-service';
import { EventsService } from './events-service';
import { Router } from '@angular/router';
import { ToastsManager } from 'ng2-toastr';
import { DatePipe } from '@angular/common';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import swal from 'sweetalert2';

declare var $: any;

@Injectable()
export class PaymentScriptService {

    form: FormGroup;
    searchText = '';
    exactMatch = false;
    constructor(private http: HttpService, private dp: DatePipe, private ngZone: NgZone, private formBuilder: FormBuilder,
        public paymentService: PaymentService, private events: EventsService, private router: Router, private toast: ToastsManager) {
        this.form = this.formBuilder.group({
            claimId: [null, Validators.compose([Validators.required])],
            rxNumber: [null],
            rxDate: [null],
            invoiceNumber: [null],
            claimNumber: [null],
            firstName: [null],
            lastName: [null]
        });
        this.events.on('payment-closed', a => {
            this.form.reset();
        });
    }

    get autoCompleteClaim(): string {
        return this.http.baseUrl + '/document/claim-search/?exactMatch=' + this.exactMatch + '&searchText=:keyword';
    }
    closeModal() {
        swal.clickCancel();
    }
    viewClaims() {
        const selectedClaims = [];
        const rows = $('tr.bgBlue');
        for (let i = 0; i < rows.length; i++) {
            const id = rows[i].id;
            selectedClaims.push(id);
        }
        if (selectedClaims.length == 0) {
            this.toast.warning('Please select one claim in order to view prescriptions.');
        } else {
            try {
                swal.clickCancel();
            } catch (e) { }
            swal({
                title: '',
                html: 'Searching claims... <br/> <img src=\'assets/1.gif\'>',
                showConfirmButton: false
            }).catch(swal.noop);
            this.paymentService.prescriptionSelected = true;
            this.paymentService.clearClaimsDetail();
            this.paymentService.getPaymentClaimDataByIds(selectedClaims);
        }
    }
    search() {
        const rxDate = this.dp.transform($('#datepicker').val(), 'MM/dd/yyyy');
        if (
            this.form.get('claimNumber').value  == null && this.form.get('firstName').value  == null && this.form.get('lastName').value  == null && this.form.get('rxNumber').value  == null && rxDate == null
        ) {
            swal.clickCancel();
            this.toast.warning('Please populate at least one search field.');
            this.events.broadcast('show-payment-script-modal', true);
        } else {
            swal({
                title: '',
                // width: (window.innerWidth - 740) + "px",
                html: 'Searching claims... <br/> <img src=\'assets/1.gif\'>',
                showConfirmButton: false
            }).catch(swal.noop);
            const data = JSON.parse(JSON.stringify(this.form.value)); // copy data
            data.rxDate = rxDate || null;
            this.paymentService.search(data, false, true);
        }
    }

    clear() {
        this.paymentService.clearClaimsData();
        this.form.reset();
        this.searchText = '';
    }
}
