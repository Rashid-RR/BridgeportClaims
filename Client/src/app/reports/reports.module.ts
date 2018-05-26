import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ReportsRoutes } from './reports.routing';
import { SweetAlert2Module } from '@toverux/ngx-sweetalert2';
import { SharedModule } from '../shared';
import { ReportComponent } from './report/report.component';
import { ReportListComponent } from './report-list/report-list.component';
import { ReportSampleComponent } from './report-sample/report-sample.component';
import { ReportAccountReceivableComponent } from './report-account-receivable/report-account-receivable.component';
import { DuplicateClaimsComponent } from './duplicate-claims/duplicate-claims.component';
import {  AccountReceivableSearchComponent} from './account-receivable-search/account-receivable-search.component';
import { AccountReceivableResultComponent} from './account-receivable-result/account-receivable-result.component';
import { DuplicateClaimSearchComponent } from './duplicate-claim-search/duplicate-claim-search.component';
import { DuplicateClaimListComponent } from './duplicate-claim-list/duplicate-claim-list.component';
@NgModule({
  imports: [
    CommonModule,
    SweetAlert2Module.forRoot(),
    SharedModule,
    RouterModule.forChild(ReportsRoutes),
  ],
  declarations: [
    AccountReceivableSearchComponent,AccountReceivableResultComponent,
    ReportComponent,ReportListComponent, ReportSampleComponent, ReportAccountReceivableComponent, DuplicateClaimsComponent, DuplicateClaimSearchComponent, DuplicateClaimListComponent
  ]
})
export class ReportsModule { }
