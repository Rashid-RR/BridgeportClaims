
import { UUID } from "angular2-uuid";
import * as Immutable from "immutable";
import { Observable } from "rxjs/Observable";
import { Prescription } from "../models/prescription"
import { DetailedPaymentClaim } from "../models/detailed-payment-claim"
import { PaymentClaim } from "../models/payment-claim"
import { PrescriptionNoteType } from "../models/prescription-note-type"
import { Injectable } from "@angular/core";
import { HttpService } from "./http-service";
import { EventsService } from "./events-service";
import { Router } from "@angular/router";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

@Injectable()
export class PaymentService {
  private claims: Immutable.OrderedMap<Number, PaymentClaim> = Immutable.OrderedMap<Number, PaymentClaim>();
   claimsDetail: Immutable.OrderedMap<Number, DetailedPaymentClaim> = Immutable.OrderedMap<Number, DetailedPaymentClaim>();
  loading: Boolean = false;
  prescriptionSelected:Boolean = false

  constructor(private http: HttpService, private events: EventsService, private router: Router, private toast: ToastsManager) {
    
  }

  clearClaimsDetail(){
    this.claimsDetail = Immutable.OrderedMap<Number, DetailedPaymentClaim>();
  }
  search(data, addHistory = true) {
    if (data.claimNumber==null && data.firstName==null && data.lastName==null && data.rxNumber==null && data.invoiceNumber==null) {
          this.toast.warning('Please specify at least one search field');
    }else{
      this.loading = true;    
      this.http.getPaymentClaim(data).map(res => { return res.json() })
        .subscribe((result: any) => {
          this.loading = false;
          if (result.length < 1) {
            this.toast.info('No Claims were Found that Match your Search Criteria. Please try again.');
          }
          if (Object.prototype.toString.call(result) === '[object Array]') {
            let res: Array<PaymentClaim> = result;
            this.claims = Immutable.OrderedMap<Number, PaymentClaim>();
            result.forEach(claim => {
              var c = new PaymentClaim(claim.claimId,claim.claimNumber, claim.patientName, claim.payor,claim.numberOfPrescriptions);
              this.claims = this.claims.set(claim.claimNumber, c);
            })
          }
        }, err => {
          this.loading = false;
          try {
            let error = err.json();
            console.log(error);
          } catch (e) { }
        }, () => {
          this.events.broadcast("payment-updated")
        }) 
    }   
  }

  get dataSize() {
    return this.claims.size;
  }
  get selected(){
    return this.claimsDetail.toArray().filter(claim=>{
        return claim.selected;
    })
  }
  get amountSelected(){
    var amount:number = 0;
     this.selected.forEach(detailedPaymentClaim => {
        amount=amount+Number(detailedPaymentClaim.invoicedAmount)
    });
    return amount;
  }
  get claimsData(): PaymentClaim[] {
    return this.claims.asImmutable().toArray();
  }
  get claimsDataCount(): Number{
    let count = 0;
     this.claims.asImmutable().toArray().forEach(c=>{
       if(c.selected) count++;
     });
     return count;
  }
  get detailedClaimsDataCount(): Number{
    let count = 0;
     this.claimsDetail.asImmutable().toArray().forEach(c=>{
       if(c.selected) count++;
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
  
  getPaymentClaimDataByIds(ids: Array<Number>=[]) {
    if (ids.length >0) {
      this.loading = true;
      this.http.getDetailedPaymentClaim(ids).map(res => { return res.json() })
        .subscribe(result => {
          this.loading = false; 
          if (result.length < 1) {
          this.toast.info('No records were found with that search critera.');
          }
          if (Object.prototype.toString.call(result) === '[object Array]') {
            let res: Array<DetailedPaymentClaim> = result;
            this.claimsDetail = Immutable.OrderedMap<Number, DetailedPaymentClaim>();
            result.forEach(claim => {
              var c = new DetailedPaymentClaim(claim.prescriptionId,claim.claimId,claim.claimNumber, claim.patientName, claim.rxNumber, claim.invoicedNumber, claim.rxDate, claim.labelName,claim.outstanding, claim.invoicedAmount,claim.payor);
              this.claimsDetail = this.claimsDetail.set(claim.prescriptionId, c);
            })
          }         
        }, err => {
          this.loading = false;
          console.log(err);
          let error = err.json();
        }, () => {
          this.events.broadcast("payment-updated");          
        })
    }
  }
  
}
