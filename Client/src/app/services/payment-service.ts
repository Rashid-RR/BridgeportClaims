
import { UUID } from "angular2-uuid";
import * as Immutable from "immutable";
import { Observable } from "rxjs/Observable";
import { Prescription } from "../models/prescription"
import { Invoice } from "../models/invoice"
import { PrescriptionNoteType } from "../models/prescription-note-type"
import { Injectable } from "@angular/core";
import { HttpService } from "./http-service";
import { EventsService } from "./events-service";
import { Router } from "@angular/router";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

@Injectable()
export class PaymentService {
  private invoices: Immutable.OrderedMap<Number, Invoice> = Immutable.OrderedMap<Number, Invoice>();
  loading: Boolean = false;
  
  constructor(private http: HttpService, private events: EventsService, private router: Router, private toast: ToastsManager) {
    
  }

  
  search(data, addHistory = true) {
    this.loading = true;
    this.http.getInvoices(data).map(res => { return res.json() })
      .subscribe((result: any) => {
        this.loading = false;
        if (result.length < 1) {
          this.toast.warning('No records were found with that search critera.');
        }
        if (Object.prototype.toString.call(result) === '[object Array]') {
          let res: Array<Invoice> = result;
          this.invoices = Immutable.OrderedMap<Number, Invoice>();
          result.forEach(invoice => {
            var c = new Invoice(invoice.claimNumber, invoice.firstName, invoice.lastName, invoice.rxNumber, invoice.invoiceNumber, invoice.rxDate,
              invoice.labelName, invoice.outstanding, invoice.invoiceAmount, invoice.payor);
            this.invoices = this.invoices.set(invoice.invoiceNumber, c);
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

  get dataSize() {
    return this.invoices.size;
  }
  get selected(){
    return this.invoices.toArray().filter(invoice=>{
        return invoice.selected;
    })
  }
  get amountSelected(){
    var amount:number = 0;
     this.selected.forEach(invoice => {
        amount=amount+Number(invoice.outstanding)
    });
    return amount;
  }
  get invoicesData(): Invoice[] {
    return this.invoices.asImmutable().toArray();
  }
  clearInvoicesData() {
    this.invoices = Immutable.OrderedMap<Number, Invoice>();
  }
  
  getinvoicesDataById(id: Number, addHistory = true) {
     var claim: Invoice = this.invoices.get(id) as Invoice;
    if (id !== undefined) {
      this.loading = true;
      this.http.getClaimsData({ claimId: id }).map(res => { return res.json() })
        .subscribe(result => {
          this.loading = false;          
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
