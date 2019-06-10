import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { Observable, BehaviorSubject } from 'rxjs';

export interface QueryBuilder {
  claimId: number;
  prescriptionId: number;
  prescriptionPaymentId: number;
  groupName: string;
  pharmacy: string;
  stateCode: string;
  dateSubmitted: string;
  billed: string;
  payable: string;
  collected: string;
  prescriber: string;
  patientLast: string;
  patientFirst: string;
  claimNumber: string;
  isAttorneyManaged: boolean;
  attorneyName: string;
}

@Injectable()
export class QueryBuilderService {
  public filterText: string;
  rows: QueryBuilder[] = [];
  refreshList$ = new BehaviorSubject(false);

  constructor(private http: HttpService) {}

  fetchQueryBuilderReport(): Observable<QueryBuilder> {
    return this.http.queryBuilderReport();
  }
}
