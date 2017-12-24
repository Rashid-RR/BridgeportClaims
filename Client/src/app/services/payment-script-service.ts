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
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DatePipe } from '@angular/common';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

import swal from "sweetalert2";
declare var $: any

@Injectable()
export class PaymentScriptService {


    checkAll: Boolean = false;
    inputs: Array<any> = [];
    form: FormGroup;
    searchText: string = '';
    exactMatch: boolean = false;
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

        /* this.events.on('payment-updated', (b: Boolean) => {
            try {
                swal.clickConfirm();
            } catch (e) { }
            if (b) {
                this.addScripts();
            }
        }); */
        this.events.on("payment-closed", a => {
            this.inputs = [];
            this.form.reset();
        });
    }

    get autoCompleteClaim(): string {
        return this.http.baseUrl + "/document/claim-search/?exactMatch=" + this.exactMatch + "&searchText=:keyword";
    }
    closeModal() {
        swal.clickCancel();
    }
    viewClaims() {
        let selectedClaims = [];
        var rows = $('tr.bgBlue');
        for (var i = 0; i < rows.length; i++) {
            var id = rows[i].id;
            selectedClaims.push(id);
        }
        if (selectedClaims.length == 0) {
            this.toast.warning('Please select one claim in order to view prescriptions.');
        } else {
            try {
                swal.clickCancel();
            } catch (e) { }
            swal({
                title: "",
                //width: (window.innerWidth - 460) + "px",
                html: "Searching claims... <br/> <img src='assets/1.gif'>",
                showConfirmButton: false
            }).catch(swal.noop);
            this.paymentService.prescriptionSelected = true
            this.paymentService.clearClaimsDetail();
            this.paymentService.getPaymentClaimDataByIds(selectedClaims);
        }
    }
    search() {
        let rxDate = this.dp.transform($('#datepicker').val(), "dd/M/yyyy"); 
        if (
            this.form.get('claimNumber').value || null == null && this.form.get('firstName').value || null == null && this.form.get('lastName').value || null == null && this.form.get('rxNumber').value || null == null && rxDate == null
        ) {
            swal.clickCancel();
            this.toast.warning('Please populate at least one search field.');
            this.events.broadcast('show-payment-script-modal', true);
        } else {
            swal({
                title: "",
                //width: (window.innerWidth - 740) + "px",
                html: "Searching claims... <br/> <img src='assets/1.gif'>",
                showConfirmButton: false
            }).catch(swal.noop);
            let data = JSON.parse(JSON.stringify(this.form.value));//copy data
            data.rxDate = rxDate || null
            this.paymentService.search(data, false, true);
        }
    }

    clear() {
        this.paymentService.clearClaimsData();
        this.form.reset();
        this.searchText = '';
    }
    checkAllRows() {
        this.paymentService.rawClaimsData.forEach(claim => {
            setTimeout(() => {
                $("tr#" + claim.claimId).removeClass("bgBlue");
                $("tr#" + claim.claimId).addClass("bgBlue");
                claim.selected = true;
                //$("input#row"+claim.claimId).attr("checked",true);
                this.paymentService.claims = this.paymentService.claims.set(claim.claimId, claim);
            }, 500);
        });
    }
    unCheckAllRows() {
        this.paymentService.rawClaimsData.forEach(claim => {
            setTimeout(() => {
                $("tr#" + claim.claimId).removeClass("bgBlue");
                claim.selected = true;
                //$("input#row"+claim.claimId).attr("checked",false);
                this.paymentService.claims = this.paymentService.claims.set(claim.claimId, claim);
            }, 500)
        });
    }
    updateTable(claimId: Number, checkAll) {
        let data = this.paymentService.rawClaimsData.get(claimId);
        this.paymentService.rawClaimsData.forEach(claim => {
            setTimeout(() => {
                if (claim.claimId == claimId) {
                    if (data) {
                        if ($("tr#" + claimId).hasClass("bgBlue") || checkAll === false) {
                            this.ngZone.run(() => {
                                $("tr#" + claimId).removeClass("bgBlue");
                            }); data.selected = false
                            this.ngZone.run(() => {
                                // $("input#row"+claimId).attr("checked",false); 
                                $("input#claimsCheckBox").attr("checked", false);
                            });
                        } else {
                            this.ngZone.run(() => {
                                $("tr#" + claimId).addClass("bgBlue");
                            });
                            data.selected = true
                            this.ngZone.run(() => {
                                //$("input#row"+claimId).attr("checked",true);
                            });
                        }
                        this.paymentService.rawClaimsData.set(claimId, data);
                    }
                }/* else{
                    $("tr#"+claim.claimId).removeClass("bgBlue");
                    claim.selected = false;
                    //$("input#row"+claim.claimId).attr("checked",false);
                    this.paymentService.claims = this.paymentService.claims.set(claim.claimId,claim);
                } */
            }, 100)
        });
    }
}
