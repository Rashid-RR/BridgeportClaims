import { UUID } from 'angular2-uuid';
import * as Immutable from 'immutable';
import { Observable } from 'rxjs/Observable';
import { PaymentClaim } from '../models/payment-claim';
import { PrescriptionNoteType } from '../models/prescription-note-type';
import { Injectable,NgZone } from '@angular/core';
import { PaymentService } from './payment-service';
import { EventsService } from './events-service';
import { Router } from '@angular/router';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DatePipe } from '@angular/common';

import swal from "sweetalert2";
declare var $:any

@Injectable()
export class PaymentScriptService {

    
    checkAll:Boolean=false;
    inputs:Array<any>=[];
    constructor(private dp: DatePipe, private ngZone:NgZone,
        private paymentService: PaymentService, private events: EventsService, private router: Router, private toast: ToastsManager) {
            this.events.on('payment-updated',(b:Boolean)=>{
                try{
                    swal.clickConfirm();
                }catch(e){}
                if(b){
                    this.addScripts();
                }
            });
            this.events.on("payment-closed",a=>{
                this.inputs = [];
            });
    }
    addScripts() {  
        let claimIds : Array<any>=[];
          this.paymentService.claimsDetail.toArray().forEach(c=>{
              claimIds.push(c.claimId);
          })
         let claimsHTML = '';
        this.paymentService.claimsData.forEach(claim => {   
            let numberOfPrescriptions = claim.numberOfPrescriptions>0 ?
            ` <a class="label label-info bg-darkblue" style="cursor: not-allowed;">
                    `+claim.numberOfPrescriptions+`
                </a>`  : claim.numberOfPrescriptions;
            claimsHTML = claimsHTML + `
                <tr id="`+claim.claimId+`" class="clr-change claimRow`+(claimIds.includes(claim.claimId) || this.paymentService.claimsData.length==1 ? ' bgBlue' :'')+`">
                    <td>`+ claim.claimNumber + `</td>
                    <td>`+ claim.patientName + `</td>
                    <td>`+ claim.payor + `</td>
                    <td>`+ numberOfPrescriptions + `</td>              
                </tr>`;
        });
        let claimNumber = this.inputs[0] || '',firstName = this.inputs[1] || '',lastName = this.inputs[2] || '',rxDate = this.inputs[3] || '',invoiceNumber = this.inputs[4] || '';
        let html = `<button class="close-button"><i class="fa fa-times"></i></button>
                        <div class="row">
                            <div class="col-sm-12" id="accordion">
                                <div class="box bottom-0">
                                    <div class="box-body payment-input">
                                        <form role="form" autocomplete="off" autocapitalize="none" autocomplete="off">
                                            <div class="row" style="margin-left:2px;">
                                               <div class="search-col">
                                                    <div class="form-group">
                                                        <label>Last Name</label>
                                                        <input class="form-control" id="lastName"  value="`+lastName+`" type="text" focus-on>
                                                    </div>
                                                </div>
                                                <div class="search-col">
                                                    <div class="form-group">
                                                        <label>First Name</label>
                                                        <input class="form-control" id="firstName"  value="`+firstName+`" type="text">
                                                    </div>
                                                </div> 
                                                <div class="search-col">
                                                    <div class="form-group">
                                                        <label>Claim #</label>
                                                        <input class="form-control" id="claimNumber" value="`+claimNumber+`" type="text" focus-on>
                                                    </div>
                                                </div>                                            
                                                <div class="search-col">
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
                                                <div class="search-col">
                                                    <div class="form-group">
                                                        <label>Invoice #</label>
                                                        <input class="form-control" id="invoiceNumber"   value="`+invoiceNumber+`" type="text" focus-on>
                                                    </div>
                                                </div>                                                    
                                                <div class="search-col">
                                                    <div class="form-group">
                                                        <label>&nbsp;</label><br/>
                                                        <button class="btn bg-darkRed btn-flat btn-small search-claims" type="button"> <i class="fa fa-search"></i> Search</button>
                                                        <button class="btn bg-darkblue btn-flat btn-small clear-inputs" type="button"> <i class="fa fa-eraser"></i>  Clear</button>
                                                        <button class="color-palette btn bg-DarkGreen btn-flat btn-small refresh-search" type="button"> <i class="fa fa-refresh"></i>  Refresh</button>
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
                                    <h4 class="box-title pull-left text-center panel-head">
                                        <u><img src="assets/img/iconClaims.png"> Claim Results</u>
                                    </h4>                                               
                                    <div class="box-tools pull-right">
                                        <button class="btn white-border view-prescriptions btn-flat btn-small" type="button" style="margin-left:auto;margin-right:auto;"><i class="fa fa-eye" aria-hidden="true"></i> Show Scripts</button>
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
                                                            <th>Scripts</th>
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
        //width: (window.innerWidth - 770) + "px",
        html: html,
        showConfirmButton:false,
        showLoaderOnConfirm: true,
        preConfirm: function () {
            return new Promise(function (resolve) {
            resolve([
                $('#claimNumber').val(),
                $('#firstName').val(),
                $('#lastName').val(),
                $('#datepicker').val(),
                $('#invoiceNumber').val()
            ])
            })
        },
        onOpen: function () {
            $('#lastName').focus()
        }
        }).then(inputs => {
            if (inputs[0] == '' && inputs[1] == '' && inputs[2] == '' && inputs[3] == '' && inputs[4] == '') {
                this.inputs=inputs;
                this.addScripts();
                this.toast.warning('Please populate at least one search field.');                
            }else{
                swal({ 
                        title: "",
                        //width: (window.innerWidth - 740) + "px",
                        html: "Searching claims... <br/> <img src='assets/1.gif'>",
                        showConfirmButton: false
                    }).catch(swal.noop);              
                let d = {claimNumber:inputs[0] || null,firstName:inputs[1]  || null,lastName:inputs[2] || null,rxDate:inputs[3] || null,invoiceNumber:inputs[4] || null}
                this.paymentService.search(d,false);
                this.inputs=inputs;
            }
        //    console.log(inputs);
        }).catch(swal.noop);
        $('#datepicker').datepicker({
            autoclose: true
        });
        $("#datepicker").inputmask("mm/dd/yyyy", {"placeholder": "mm/dd/yyyy"});
        $("[inputs-mask]").inputmask();
        $("[data-mask]").inputmask();
        $(".search-claims").click(()=>{ 
            try{
                swal.clickConfirm();
            }catch(e){}
        });
        $(".clear-inputs").click(()=>{
            this.paymentService.clearClaimsData();
            $('#claimNumber').val('');
            $('#firstName').val('');
            $('#lastName').val();
            $('#datepicker').val('');
            $('#invoiceNumber').val('');
            try{
                swal.clickCancel();
            }catch(e){}
            this.inputs=[];
            this.addScripts();
        });
        $(".refresh-search").click(()=>{
            try{
                swal.clickConfirm();
            }catch(e){}
        });
        $(".close-button").click(()=>{
            try{
                swal.clickCancel();
            }catch(e){}
        });
        $("input.form-control").keypress((e)=>{
            var key = e.which; if(key == 13){
                try{
                    swal.clickConfirm();
                }catch(e){}
            }
        });
        $("button.view-prescriptions").click((e)=>{
            this.viewClaims();
        });
        let ps = this;
        $(".claimRow").click(function(){  
            let row =  $(this)[0];
            let claimId = row.id;
            ps.updateTable(parseInt(claimId),null);             
        });           
        $("input#claimsCheckBox").click(function($event){  
            this.checkAll =  $event.target.checked;  
            if(this.checkAll){
                ps.checkAllRows();
            }else{
                ps.unCheckAllRows();
            }
        });
    } 
    viewClaims(){
        let selectedClaims = [];
        var rows = $('tr.bgBlue');
        for (var i = 0; i < rows.length; i++) {
            var id =rows[i].id;
            selectedClaims.push(id);
        }
        if (selectedClaims.length == 0) {
        this.toast.warning('Please select one claim in order to view prescriptions.');
        }else{
            try{
                swal.clickCancel();
            }catch(e){}
            swal({ 
                title: "",
                //width: (window.innerWidth - 460) + "px",
                html: "Searching claims... <br/> <img src='assets/1.gif'>",
                showConfirmButton: false
            }).catch(swal.noop); 
            this.paymentService.prescriptionSelected=true
            this.paymentService.clearClaimsDetail();
            this.paymentService.getPaymentClaimDataByIds(selectedClaims);
        }
    }
    checkAllRows(){
        this.paymentService.rawClaimsData.forEach(claim=>{   
            setTimeout(()=>{
                $("tr#"+claim.claimId).removeClass("bgBlue");            
                $("tr#"+claim.claimId).addClass("bgBlue"); 
                claim.selected = true;
                //$("input#row"+claim.claimId).attr("checked",true);
                this.paymentService.claims = this.paymentService.claims.set(claim.claimId,claim);
            },500);
        });
    }
    unCheckAllRows(){
        this.paymentService.rawClaimsData.forEach(claim=>{   
            setTimeout(()=>{
                $("tr#"+claim.claimId).removeClass("bgBlue");
                claim.selected = true;
                //$("input#row"+claim.claimId).attr("checked",false);
                this.paymentService.claims = this.paymentService.claims.set(claim.claimId,claim);
            },500)
        });
    }
    updateTable(claimId:Number,checkAll){
        let data = this.paymentService.rawClaimsData.get(claimId);
        this.paymentService.rawClaimsData.forEach(claim=>{   
            setTimeout(()=>{
                if(claim.claimId == claimId){                        
                    if(data){   
                        if($("tr#"+claimId).hasClass("bgBlue") || checkAll===false){                        
                            this.ngZone.run(()=>{
                                $("tr#"+claimId).removeClass("bgBlue");
                            });    data.selected = false                       
                            this.ngZone.run(()=>{
                            // $("input#row"+claimId).attr("checked",false); 
                                $("input#claimsCheckBox").attr("checked",false); 
                            });
                        }else{                        
                            this.ngZone.run(()=>{
                                $("tr#"+claimId).addClass("bgBlue"); 
                            });
                            data.selected = true                        
                            this.ngZone.run(()=>{
                                //$("input#row"+claimId).attr("checked",true);
                            });
                        }
                        this.paymentService.rawClaimsData.set(claimId,data);
                    }
                }/* else{
                    $("tr#"+claim.claimId).removeClass("bgBlue");
                    claim.selected = false;
                    //$("input#row"+claim.claimId).attr("checked",false);
                    this.paymentService.claims = this.paymentService.claims.set(claim.claimId,claim);
                } */
            },100)
        });
    }
}
