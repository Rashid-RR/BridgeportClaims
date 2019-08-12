import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { InvoiceProcess } from './../models/invoice.model';
import { HttpService } from './http-service';

@Injectable()
export class InvoiceProcessService {
  loading: Boolean = false;
  public filterText: string;
  rows: InvoiceProcess[] = [];
  states: string[];
  refreshList$ = new BehaviorSubject(false);

  constructor(private http: HttpService) {}

  // getPatientAddressEdit(): Observable<AddressEdit> {
  //   return this.http.getPatientAddressEdit();
  // }

  getInvoiceProcess(): Observable<InvoiceProcess> {
    return this.http.getInvoicesProcess();
  }
}
