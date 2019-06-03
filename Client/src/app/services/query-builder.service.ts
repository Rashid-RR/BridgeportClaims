import { Injectable } from '@angular/core';
import { HttpService } from './http-service';

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
  loading = false;
  rows: QueryBuilder[] = [];

  constructor(private http: HttpService) {}

  fetchQueryBuilderReport() {
    this.loading = true;
    this.http.queryBuilderReport().subscribe(r => {
      this.rows = r;
      this.loading = false;
    }, err => {
      this.loading = false;
      this.rows = [{} as any];
  });
  }
}
