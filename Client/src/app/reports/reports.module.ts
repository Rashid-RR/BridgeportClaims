import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SweetAlert2Module } from '@toverux/ngx-sweetalert2';
import { AgGridModule } from 'ag-grid-angular';
import { SharedModule } from '../shared';
import { AccountReceivableResultComponent } from './account-receivable-result/account-receivable-result.component';
import { AccountReceivableSearchComponent } from './account-receivable-search/account-receivable-search.component';
import { ClaimsDataListComponent } from './claims-data-list/claims-data-list.component';
import { ClaimsDataComponent } from './claims-data/claims-data.component';
import { CollectionBonusListComponent } from './collection-bonus-list/collection-bonus-list.component';
import { CollectionBonusSearchComponent } from './collection-bonus-search/collection-bonus-search.component';
import { CollectionBonusComponent } from './collection-bonus/collection-bonus.component';
import { DuplicateClaimListComponent } from './duplicate-claim-list/duplicate-claim-list.component';
import { DuplicateClaimManualComponent } from './duplicate-claim-manual/duplicate-claim-manual.component';
import { DuplicateClaimSearchComponent } from './duplicate-claim-search/duplicate-claim-search.component';
import { DuplicateClaimComponent } from './duplicate-claim/duplicate-claim.component';
import { DuplicateClaimsComponent } from './duplicate-claims/duplicate-claims.component';
import { ReportAccountReceivableComponent } from './report-account-receivable/report-account-receivable.component';
import { ReportListComponent } from './report-list/report-list.component';
import { ReportSampleComponent } from './report-sample/report-sample.component';
import { ReportComponent } from './report/report.component';
import { ReportsRoutes } from './reports.routing';
import { ShortPayReportComponent } from './short-pay-report/short-pay-report.component';
import { ShortPaySearchComponent } from './short-pay-search/short-pay-search.component';
import { ShortPayComponent } from './short-pay/short-pay.component';
import { SkippedPaymentListComponent } from './skipped-payment-list/skipped-payment-list.component';
import { SkippedPaymentSearchComponent } from './skipped-payment-search/skipped-payment-search.component';
import { SkippedPaymentComponent } from './skipped-payment/skipped-payment.component';
import 'ag-grid-enterprise';
import { ClaimsDataSearchFilterComponent } from './claims-data-search-filter/claims-data-search-filter.component';
import { AddressEditComponent } from './address-edit/address-edit.component';
import { AddressEditListComponent } from './address-edit-list/address-edit-list.component';
import { AddressEditSearchFilterComponent } from './address-edit-search-filter/address-edit-search-filter.component';
import { StateCellRendererComponent } from './address-edit/states-cell-renderer.component';
import { InvoicesComponent } from './invoices/invoices.component';
import { InvoicesSearchFilterComponent } from './invoices-search-filter/invoices-search-filter.component';
import { InvoicesListComponent } from './invoices-list/invoices-list.component';
import { AgDateFilterComponent } from '../components/ag-date-filter/ag-date-filter.component';

@NgModule({
  imports: [
    CommonModule,
    SweetAlert2Module.forRoot(),
    SharedModule,
    RouterModule.forChild(ReportsRoutes),
    AgGridModule.withComponents([StateCellRendererComponent, AgDateFilterComponent])
  ],
  declarations: [
    AccountReceivableSearchComponent, AccountReceivableResultComponent,
    ReportComponent, ReportListComponent, ReportSampleComponent, ReportAccountReceivableComponent, DuplicateClaimsComponent,
    DuplicateClaimSearchComponent, DuplicateClaimListComponent, ShortPayReportComponent, SkippedPaymentListComponent,
    SkippedPaymentSearchComponent, ShortPaySearchComponent, SkippedPaymentComponent, ShortPayComponent,
    DuplicateClaimComponent, DuplicateClaimManualComponent, CollectionBonusComponent, CollectionBonusSearchComponent,
    CollectionBonusListComponent,
    ClaimsDataComponent,
    ClaimsDataListComponent,
    ClaimsDataSearchFilterComponent,
    AddressEditComponent,
    AddressEditListComponent,
    AddressEditSearchFilterComponent,
    StateCellRendererComponent,
    InvoicesComponent,
    InvoicesSearchFilterComponent,
    InvoicesListComponent,
    AgDateFilterComponent
  ]
})
export class ReportsModule { }
