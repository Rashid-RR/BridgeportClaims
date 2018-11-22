import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReportsRoutes } from './reports.routing';
import { SweetAlert2Module } from '@toverux/ngx-sweetalert2';
import { SharedModule } from '../shared';
import { ReportComponent } from './report/report.component';
import { ReportListComponent } from './report-list/report-list.component';
import { ReportSampleComponent } from './report-sample/report-sample.component';
import { ReportAccountReceivableComponent } from './report-account-receivable/report-account-receivable.component';
import { DuplicateClaimsComponent } from './duplicate-claims/duplicate-claims.component';
import { AccountReceivableSearchComponent } from './account-receivable-search/account-receivable-search.component';
import { AccountReceivableResultComponent } from './account-receivable-result/account-receivable-result.component';
import { DuplicateClaimSearchComponent } from './duplicate-claim-search/duplicate-claim-search.component';
import { DuplicateClaimListComponent } from './duplicate-claim-list/duplicate-claim-list.component';
import { ShortPayReportComponent } from './short-pay-report/short-pay-report.component';
import { SkippedPaymentListComponent } from './skipped-payment-list/skipped-payment-list.component';
import { SkippedPaymentSearchComponent } from './skipped-payment-search/skipped-payment-search.component';
import { ShortPaySearchComponent } from './short-pay-search/short-pay-search.component';
import { SkippedPaymentComponent } from './skipped-payment/skipped-payment.component';
import { ShortPayComponent } from './short-pay/short-pay.component';
import { DuplicateClaimComponent } from './duplicate-claim/duplicate-claim.component';
import { DuplicateClaimManualComponent } from './duplicate-claim-manual/duplicate-claim-manual.component';
import { CollectionBonusComponent } from './collection-bonus/collection-bonus.component';
import { CollectionBonusSearchComponent } from './collection-bonus-search/collection-bonus-search.component';
import { CollectionBonusListComponent } from './collection-bonus-list/collection-bonus-list.component';
@NgModule({
  imports: [
    CommonModule,
    SweetAlert2Module.forRoot(),
    SharedModule,
    RouterModule.forChild(ReportsRoutes),
  ],
  declarations: [
    AccountReceivableSearchComponent, AccountReceivableResultComponent,
    ReportComponent, ReportListComponent, ReportSampleComponent, ReportAccountReceivableComponent, DuplicateClaimsComponent, DuplicateClaimSearchComponent, DuplicateClaimListComponent, ShortPayReportComponent, SkippedPaymentListComponent, SkippedPaymentSearchComponent, ShortPaySearchComponent, SkippedPaymentComponent, ShortPayComponent, DuplicateClaimComponent, DuplicateClaimManualComponent, CollectionBonusComponent, CollectionBonusSearchComponent, CollectionBonusListComponent
  ]
})
export class ReportsModule { }
