import { Routes } from '@angular/router';
import { PaymentComponent } from './payment/payment.component';
import { PostedCheckDetailComponent } from './posted-check-detail/posted-check-detail.component';

export const PaymentRoutes: Routes = [
    {path: '', component: PaymentComponent},
    {path: ':documentId', component: PaymentComponent},
    {path: 'posted/detail/:id', component: PostedCheckDetailComponent},
    {path: ':documentId/:checkNumber', component: PaymentComponent},
];
