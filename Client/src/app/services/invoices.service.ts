import { InvoiceScreen } from './../models/invoice.model';
import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { UsState } from '../models/us-state';
import { HttpService } from './http-service';

@Injectable()
export class InvoicesService {
  public filterText: string;
  rows: InvoiceScreen[] = [];
  states: string[];
  refreshList$ = new BehaviorSubject(false);

  constructor(private http: HttpService) {}

  // getPatientAddressEdit(): Observable<AddressEdit> {
  //   return this.http.getPatientAddressEdit();
  // }

  getInvoices(): Observable<InvoiceScreen> {
    return this.http.getInvoices();
  }
}
