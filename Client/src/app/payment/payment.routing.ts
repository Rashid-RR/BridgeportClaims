import { Routes } from '@angular/router';
import { PaymentComponent } from './payment/payment.component';

export const PaymentRoutes: Routes = [   
    {path: '', component: PaymentComponent},
    {path: ':documentId', component: PaymentComponent},
    {path: ':documentId/:checkNumber', component: PaymentComponent},
];