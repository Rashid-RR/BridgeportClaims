import { UUID } from 'angular2-uuid';
import * as Immutable from 'immutable';
import { Observable } from 'rxjs/Observable';
import { PaymentClaim } from '../models/payment-claim';
import { PrescriptionNoteType } from '../models/prescription-note-type';
import { Injectable } from '@angular/core';
import { PaymentService } from './payment-service';
import { EventsService } from './events-service';
import { Router } from '@angular/router';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DatePipe } from '@angular/common';

import swal from "sweetalert2";
declare var $:any

@Injectable()
export class PaymentScriptService {

    inputs:Array<any>=[];
  constructor(private dp: DatePipe,
    private paymentService: PaymentService, private events: EventsService, private router: Router, private toast: ToastsManager) {
        this.events.on('payment-updated',()=>{
            swal.clickConfirm();
            this.addScripts();
        });
  }
addScripts() {    
        let claimsHTML = '';
        this.paymentService.claimsData.forEach(claim => {     
            let numberOfPrescriptions = claim.numberOfPrescriptions>0 ?
            ` <a class="label label-info bg-darkblue" style="cursor: not-allowed;">
                    `+claim.numberOfPrescriptions+`
                </a>`  : claim.numberOfPrescriptions;
            claimsHTML = claimsHTML + `
                <tr>
                    <td>`+ claim.claimNumber + `</td>
                    <td>`+ claim.patientName + `</td>
                    <td>`+ claim.payor + `</td>
                    <td>`+ numberOfPrescriptions + `</td>              
                </tr>`;
        });
        let claimNumber = this.inputs[0] || '',firstName = this.inputs[1] || '',lastName = this.inputs[2] || '',rxDate = this.inputs[3] || '',invoiceNumber = this.inputs[4] || '';
         let html = `<div class="row">
                            <div class="col-sm-12" id="accordion">
                                <div class="box bottom-0">
                                    <div class="box-body payment-input">
                                        <form role="form" autocomplete="off" autocapitalize="none" autocomplete="off">
                                            <div class="row" style="margin-left:2px;">
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label>Claim #</label>
                                                        <input class="form-control" id="claimNumber" value="`+claimNumber+`" type="text" focus-on>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label>First Name</label>
                                                        <input class="form-control" id="firstName"  value="`+firstName+`" type="text">
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label>Last Name</label>
                                                        <input class="form-control" id="lastName"  value="`+lastName+`" type="text" focus-on>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <div class="form-group">
                                                        <label>Rx Date</label>
                                                        <div class="input-group date">
                                                        <div class="input-group-addon">
                                                            <i class="fa fa-calendar"></i>
                                                        </div>
                                                        <input class="form-control pull-right"  type="text"  value="`+rxDate+`" id="datepicker" name="rxDate" inputs-inputmask="'alias': 'mm/dd/yyyy'" inputs-mask focus-on>                  
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-2 col-md-1">
                                                    <div class="form-group">
                                                        <label>Invoice #</label>
                                                        <input class="form-control" id="invoiceNumber"   value="`+invoiceNumber+`" type="text" focus-on>
                                                    </div>
                                                </div>                                                    
                                                <div class="col-sm-2 col-md-3">
                                                    <div class="form-group">
                                                        <label>&nbsp;</label><br/>
                                                        <button class="btn bg-darkRed btn-flat btn-small search-claims" type="button">Search</button>
                                                        <button class="btn bg-darkblue btn-flat btn-small clear-inputs" type="button">Clear</button>
                                                        <button class="color-palette btn bg-DarkGreen btn-flat btn-small refresh-search" type="button">Refresh</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                        <div class="col-sm-12" id="accordion">
                            <div class="box">
                                <div class="box-header bg-head-box">
                                    <h4 class="box-title text-center panel-head">
                                        <u><img src="assets/img/iconClaims.png"> Claims</u>
                                    </h4>
                                    <span  class="tally pull-right" style="margin-right:250px;">
                                        <span>
                                            <span style="font-size:13pt">
                                            `+(this.paymentService.claimsData.length)+` &nbsp; 
                                            </span> Record`+(this.paymentService.claimsData.length>1 ? 's':'')+` found
                                        </span>
                                        `+(this.paymentService.claimsDataCount ? `
                                        <span class="label bg-darkblue" style="margin-left:20px;font-size:9pt">
                                            <span style="font-size:11pt">
                                                `+this.paymentService.claimsDataCount+` 
                                            </span> Row `+(this.paymentService.claimsDataCount>1 ? 's':'')+` selected
                                        </span>` : '')+`
                                    </span>                                               
                                    <div class="box-tools pull-right">
                                        <button class="btn bg-darkblue btn-flat btn-small" type="button" (click)="viewClaims()"  style="margin-left:auto;margin-right:auto;">View Prescriptions</button>
                                    </div>                                                
                                </div>
                                <div class="box-body claims payment-result panel-body-bg">
                                    <div class="row claim-info"  style="overflow:hidden;">
                                        <div class="col-sm-12 claim-col expanded" style="overflow:hidden;">
                                            <div class="table-responsive top-header scroll-y">
                                                <table class="table no-margin table-striped">
                                                    <thead class="overflowable" id="fixed-thead">
                                                        <tr>                                                           
                                                            <th>Claim #</th>
                                                            <th>Patient Name</th>                        
                                                            <th>Payor</th>
                                                            <th># of Prescriptions</th>
                                                        </tr>
                                                    </thead>
                                                </table>
                                            </div>
                                            <div class="table-responsive table-body">
                                                <table class="table no-margin table-striped" id="maintable">                                            
                                                    <tbody>`
                                                        + claimsHTML+`
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>`;
         swal({
          title: '',
          customClass:'paymeny-modal',
          width: window.innerWidth * 3.9 / 4 + "px",
          html: html,
          showConfirmButton:false,
          showLoaderOnConfirm: true,
          preConfirm: function () {
            return new Promise(function (resolve) {
              resolve([
                window['jQuery']('#claimNumber').val(),
                window['jQuery']('#firstName').val(),
                window['jQuery']('#lastName').val(),
                window['jQuery']('#datepicker').val(),
                window['jQuery']('#invoiceNumber').val()
              ])
            })
          },
        }).then(inputs => {
            if (inputs[0] == '' && inputs[1] == '' && inputs[2] == '' && inputs[3] == '' && inputs[4] == '') {
                this.inputs=inputs;
                this.addScripts();
                this.toast.warning('Please populate at least one search field.');                
            }else{
                swal({ 
                        title: "",
                        width: window.innerWidth * 3.9 / 4 + "px",
                        html: "Searching claims... <br/> <img src='assets/1.gif'>",
                        showConfirmButton: false
                    });              
                let d = {claimNumber:inputs[0] || null,firstName:inputs[1]  || null,lastName:inputs[2] || null,rxDate:inputs[3] || null,invoiceNumber:inputs[4] || null}
                this.paymentService.search(d,false);
                this.inputs=inputs;
            }
            console.log(inputs);
        }).catch(swal.noop);
        window['jQuery']('#datepicker').datepicker({
            autoclose: true
        });
        window['jQuery']("#datemask").inputmask("mm/dd/yyyy", {"placeholder": "mm/dd/yyyy"});
        window['jQuery']("[inputs-mask]").inputmask();
        $(".search-claims").click(()=>{
            console.log("Click confirm...")
            swal.clickConfirm();
        });
        $(".clear-inputs").click(()=>{
            console.log("Clear button confirm...")
        });
        $(".refresh-search").click(()=>{
            console.log("Refresh button confirm...")
        });
    }     
}
