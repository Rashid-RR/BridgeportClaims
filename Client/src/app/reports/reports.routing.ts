import { Routes } from '@angular/router';

import { ReportComponent } from './report/report.component';
import { ReportListComponent } from './report-list/report-list.component';
import { ReportSampleComponent } from './report-sample/report-sample.component';
import { ReportAccountReceivableComponent } from './report-account-receivable/report-account-receivable.component';
import { DuplicateClaimsComponent } from './duplicate-claims/duplicate-claims.component';


export const ReportsRoutes: Routes = [
    {
        path: '', component: ReportComponent,
        children: [
            { path: '', redirectTo: 'list', pathMatch: 'full' },
            { path: 'list', component: ReportListComponent },
            { path: 'revenue', component: ReportSampleComponent },
            { path: 'account-receivable', component: ReportAccountReceivableComponent },
            { path: 'sample', component: ReportSampleComponent },
            { path: 'duplicate-claims', component: DuplicateClaimsComponent }
        ]
    }
];