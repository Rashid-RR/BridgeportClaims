import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { InvoiceScreen } from './../models/invoice.model';
import { HttpService } from './http-service';

@Injectable()
export class InvoicesService {
  public filterText: string;
  rows: InvoiceScreen[] = [];
  states: string[];
  refreshList$ = new BehaviorSubject(false);

  constructor(private http: HttpService) {}

  getInvoices(): Observable<InvoiceScreen> {
    return this.http.getInvoices();
  }
}
