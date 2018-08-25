import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AutoCompleteModule } from '../auto-complete';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PaymentRoutes } from './payment.routing';
import { PaymentInvoiceComponent } from './payment-invoice/payment-invoice.component';
import { PaymentInputComponent } from './payment-input/payment-input.component';
import { PaymentResultComponent } from './payment-result/payment-result.component';
import { PaymentClaimResultComponent } from './payment-claim-result/payment-claim-result.component';
import { PaymentDetailedResultComponent } from './payment-detail-result/payment-detail-result.component';
import { PaymentComponent } from './payment/payment.component';
import { SweetAlert2Module } from '@toverux/ngx-sweetalert2';
import { AddScriptModalComponent } from './add-script-modal/add-script-modal.component';
import { SharedModule } from '../shared';

@NgModule({
  imports: [
    CommonModule,
    SweetAlert2Module.forRoot(),
    SharedModule,
    RouterModule.forChild(PaymentRoutes),
  ],
  declarations: [    
  PaymentInvoiceComponent, PaymentInputComponent, PaymentResultComponent, PaymentClaimResultComponent,
  PaymentDetailedResultComponent,PaymentComponent,AddScriptModalComponent
  ]
})
export class PaymentModule { }
