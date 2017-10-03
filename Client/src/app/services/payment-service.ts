
import { UUID } from 'angular2-uuid';
import * as Immutable from 'immutable';
import { Observable } from 'rxjs/Observable';
import { Prescription } from '../models/prescription';
import { DetailedPaymentClaim } from '../models/detailed-payment-claim';
import { PaymentClaim } from '../models/payment-claim';
import { PrescriptionNoteType } from '../models/prescription-note-type';
import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { EventsService } from './events-service';
import { Router } from '@angular/router';
import {PaymentPosting} from "../models/payment-posting";
import {PaymentPostingPrescription} from '../models/payment-posting-prescription'
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { SortColumnInfo } from "../directives/table-sort.directive";

declare var $:any
@Injectable()
export class PaymentService {
  claims: Immutable.OrderedMap<Number, PaymentClaim> = Immutable.OrderedMap<Number, PaymentClaim>();
  claimsDetail: Immutable.OrderedMap<Number, DetailedPaymentClaim> = Immutable.OrderedMap<Number, DetailedPaymentClaim>();
  loading: Boolean = false;
  loadingPayment: Boolean = false;
  paymentPosting:PaymentPosting= new PaymentPosting();
  prescriptionSelected: Boolean = false;
  sortColumn: SortColumnInfo;
  lastPrescriptionIds: Array<Number>=[];
  constructor(private http: HttpService, private events: EventsService, private router: Router, private toast: ToastsManager) {
    this.events.on('postPaymentPrescriptionReturnDtos',data=>{
          data.prescriptions.forEach(d=>{
             var prescription = this.claimsDetail.get(d.prescriptionId);
            if(prescription){
              prescription.outstanding = Number(d.outstanding);
            }
           });
    });
  }


  clearClaimsDetail() {
    this.claimsDetail = Immutable.OrderedMap<Number, DetailedPaymentClaim>();
  }

  onSortColumn(info: SortColumnInfo) {
    this.sortColumn = info;
    this.getPaymentClaimDataByIds(this.lastPrescriptionIds);
  }
  search(data, addHistory = true) {
    if (data.claimNumber == null && data.firstName == null && data.lastName == null && data.rxDate == null && data.invoiceNumber == null) {
          this.toast.warning('Please populate at least one search field.');
    } else {
      this.loading = true;
      this.http.getPaymentClaim(data).map(res => { return res.json(); })
        .subscribe((result: any) => {
          this.loading = false;
          if (result.length < 1) {
            this.toast.info('No claims were found. Please try searching again.');
          }
          if (Object.prototype.toString.call(result) === '[object Array]') {
            const res: Array<PaymentClaim> = result;
             this.claims = Immutable.OrderedMap<Number, PaymentClaim>();
            result.forEach(claim => {
              const c = new PaymentClaim(claim.claimId, claim.claimNumber, claim.patientName, claim.payor, claim.numberOfPrescriptions);
              if(result.length==1){
                c.selected = true;
              }
              this.claims = this.claims.set(claim.claimId, c);
            });
          }
        }, err => {
          this.loading = false;
          try {
            const error = err.json();
            console.log(error);
          } catch (e) { }
        }, () => {
          this.events.broadcast('payment-updated',true);
        });
    }
  }
  post(data) {
     if (!data) {
          this.toast.warning('Please populate at least one search field.');
    } else {
      this.loading = true;
      this.http.paymentPosting(data).map(res => { return res.json(); })
        .subscribe((result: any) => {
          this.loading = false;
          this.toast.info("Posting has been saved. Please continue posting until the Check Amount is posted in full before it is saved to the database");
           this.events.broadcast('payment-amountRemaining',result);
           //this.events.broadcast('postPaymentPrescriptionReturnDtos',{prescriptions:result.postPaymentPrescriptionReturnDtos});
           result.paymentPostings.forEach(prescription=>{
             try{
                this.claimsDetail.get(prescription.prescriptionId).outstanding = prescription.outstanding;
                //this.claimsDetail.get(prescription.prescriptionId).outstanding = this.claimsDetail.get(prescription.prescriptionId).invoicedAmount+prescription.outstanding;
                this.claimsDetail.get(prescription.prescriptionId).selected = false;
             }catch(e){}
             let posting  = prescription as PaymentPostingPrescription;
             this.paymentPosting.payments = this.paymentPosting.payments.set(prescription.id,posting);
           })
        }, err => {
          this.loading = false;
          try {
            const error = err.json();
            console.log(error);
          } catch (e) { }
        }, () => {
          this.events.broadcast('payment-updated');
        });
    }
  }

  get dataSize() {
    return this.claims.size;
  }
  get selected(){
    return this.claimsDetail.toArray().filter(claim => {
        return claim.selected;
    });
  }
  get unselected(){
    return this.claimsDetail.toArray().filter(claim => {
        return !claim.selected;
    });
  }
  get amountSelected(){
     let amount = 0;
     this.selected.forEach(detailedPaymentClaim => {
        amount = amount + Number(detailedPaymentClaim.invoicedAmount);
    });
    return amount;
  }
  get claimsData(): PaymentClaim[] {
    return this.claims.asImmutable().toArray();
  }
  get claimsDataCount(): Number{
    let count = 0;
     this.claims.asImmutable().toArray().forEach(c => {
       if (c.selected) {
        count++;
       }
     });
     return count;
  }
  get detailedClaimsDataCount(): Number{
     let count = 0;
     this.claimsDetail.asImmutable().toArray().forEach(c => {
       if (c.selected) {
         count++;
       }
     });
     return count;
  }
  get rawClaimsData(): Immutable.OrderedMap<Number, PaymentClaim> {
    return this.claims;
  }
  get rawDetailedClaimsData(): Immutable.OrderedMap<Number, DetailedPaymentClaim> {
    return this.claimsDetail;
  }
  get detailedClaimsData(): DetailedPaymentClaim[] {
    return this.claimsDetail.asImmutable().toArray();
  }
  clearClaimsData() {
    this.claims = Immutable.OrderedMap<Number, PaymentClaim>();
  }

  unSelectAllPrescriptions(){
    this.claimsDetail.asImmutable().toArray().forEach(c => {
      c.selected = false;
      c.filterSelected = false;
      c.searchSelected = false;
   });
   $('input#claimsCheckBox').prop('checked', false);
   this.events.broadcast('claimsCheckBox',false);
  }
  moveSelectedPrescriptions(){
    this.claimsDetail.asImmutable().toArray().forEach(c => {
      if(c.searchSelected && !c.selected)
        c.selected = true;
        c.filterSelected = false;
        c.searchSelected = false;
   });
  }
  moveUnselectedPrescriptions(){
    this.claimsDetail.asImmutable().toArray().forEach(c => {
      if(c.filterSelected && c.selected)
        c.selected = false;
        c.filterSelected = false;
        c.searchSelected = false;
      });
  }
  selectAllPrescriptions(){
    this.claimsDetail.asImmutable().toArray().forEach(c => {
       c.selected = true;
       c.filterSelected = false;
       c.searchSelected = false;
    }); 
    $('input#claimsCheckBox').prop('checked', false);
    this.events.broadcast('claimsCheckBox',false);
  }
  finalizePosting(data:any){
    this.loading = true;
    this.http.finalizePosting(data).map(res => { return res.json(); })
      .subscribe(result => {
        this.loading = false;
        //console.log(result);
        if (result.message) {
          this.toast.success(result.message);
        }
        this.claims = Immutable.OrderedMap<Number, PaymentClaim>();
        this.claimsDetail= Immutable.OrderedMap<Number, DetailedPaymentClaim>();
        this.events.broadcast('payment-updated',false);
        this.events.broadcast('payment-closed',false);
        this.events.broadcast("disable-links",false);
      }, err => {
        this.loading = false;
        console.log(err);
        if (err.message) {
          this.toast.error(err.message);
        }
        this.events.broadcast('payment-updated',true);
      }, () => {
      });
  }
  paymentToSuspense(data:any){
    this.loading = true;
    this.http.paymentToSuspense(data).map(res => { return res.json(); })
      .subscribe(result => {
        this.loading = false;
        if (result.message) {
          this.toast.success(result.message);
        }        
        this.claims = Immutable.OrderedMap<Number, PaymentClaim>();
        this.claimsDetail= Immutable.OrderedMap<Number, DetailedPaymentClaim>();        
        this.events.broadcast('payment-suspense',false);
        this.events.broadcast("disable-links",false);
      }, err => {
        this.loading = false;
        console.log(err);
        if (err.message) {
          this.toast.error(err.message);
        }
      }, () => {
      });
  }
  deletePayment(data:any){
    this.loading = true;
    this.http.deletePayment(data).map(res => { return res.json(); })
      .subscribe(result => {
        this.loading = false;
        this.paymentPosting.payments = this.paymentPosting.payments.delete(data.id);
        //console.log(result);
        if (result.message) {
          this.toast.success(result.message);
          if(result.amountRemaining){
            let data = {amountRemaining:result.amountRemaining,sessionId:this.paymentPosting.sessionId};
            this.events.broadcast('payment-amountRemaining',data);
          }
        }
      }, err => {
        this.loading = false;
        console.log(err);
        if (err.message) {
          this.toast.error(err.message);
        }
      }, () => {
      });
  }
  getPaymentClaimDataByIds(ids: Array<Number>= []) {
      let page = 1;
      let page_size = 1000;
      let sort: string = 'RxDate';
      let sort_dir: 'asc' | 'desc' = 'desc';
      if (this.sortColumn) {
        sort = this.sortColumn.column;
        sort_dir = this.sortColumn.dir;
      }
      if (ids.length > 0) {
      this.loading = true;
      this.http.getDetailedPaymentClaim(ids,sort,sort_dir,page,page_size).map(res => { return res.json(); })
        .subscribe(result => {
          this.lastPrescriptionIds = ids;
          this.loading = false;
          if (result.length < 1) {
          this.toast.info('No records were found from your search');
          }
          if (Object.prototype.toString.call(result) === '[object Array]') {

          this.loadingPayment = true;
            const res: Array<DetailedPaymentClaim> = result;
            this.claimsDetail = Immutable.OrderedMap<Number, DetailedPaymentClaim>();
            //var i=0; //test
            result.forEach(claim => {
              // claim.isReversed = i%2== 0 ? true : false; //test
              const c = new DetailedPaymentClaim(claim.prescriptionId, claim.claimId, claim.claimNumber, claim.patientName,
                claim.rxNumber, claim.invoicedNumber, claim.rxDate, claim.labelName, claim.outstanding, claim.invoicedAmount, claim.payor,claim.isReversed);
                c.selected = result.length ==1  && !claim.isReversed ? true : false;
                this.claimsDetail = this.claimsDetail.set(claim.prescriptionId, c);
              //i++;//meant for test
            });
            this.loadingPayment = false;
          }
         this.events.broadcast('payment-updated',false);
        }, err => {
        this.loadingPayment = false;
          this.loading = false;
          console.log(err);
          //const error = err.json();
          this.events.broadcast('payment-updated',true);
        }, () => {
        });
    }
  }
}
