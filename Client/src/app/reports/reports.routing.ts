import { Routes } from '@angular/router';
import { AddressEditComponent } from './address-edit/address-edit.component';
import { ClaimsDataComponent } from './claims-data/claims-data.component';
import { CollectionBonusComponent } from './collection-bonus/collection-bonus.component';
import { DuplicateClaimManualComponent } from './duplicate-claim-manual/duplicate-claim-manual.component';
import { DuplicateClaimsComponent } from './duplicate-claims/duplicate-claims.component';
import { ReportAccountReceivableComponent } from './report-account-receivable/report-account-receivable.component';
import { ReportListComponent } from './report-list/report-list.component';
import { ReportSampleComponent } from './report-sample/report-sample.component';
import { ReportComponent } from './report/report.component';
import { ShortPayComponent } from './short-pay/short-pay.component';
import { SkippedPaymentComponent } from './skipped-payment/skipped-payment.component';
import { InvoicesComponent } from './invoices/invoices.component';

export const ReportsRoutes: Routes = [
    {
        path: '', component: ReportComponent,
        children: [
            { path: '', redirectTo: 'list', pathMatch: 'full' },
            { path: 'list', component: ReportListComponent },
            { path: 'revenue', component: ReportSampleComponent },
            { path: 'account-receivable', component: ReportAccountReceivableComponent },
            { path: 'sample', component: ReportSampleComponent },
            { path: 'shortpay', component: ShortPayComponent },
            { path: 'collection-bonus', component: CollectionBonusComponent },
            { path: 'claims-data', component: ClaimsDataComponent },
            { path: 'address-edit', component: AddressEditComponent },
            { path: 'skipped-payment', component: SkippedPaymentComponent },
            {
              path: 'duplicate-claims',
              children: [
                { path: '', component: DuplicateClaimsComponent },
                { path: 'auto', component: DuplicateClaimsComponent },
                { path: 'manual', component: DuplicateClaimManualComponent }
              ]
            },
            { path: 'invoices', component: InvoicesComponent },
        ]
    }
];
