import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CustomerRoutes } from './customer.routing';
import { CustomerInformationComponent } from './information/information.component';
import { SharedModule } from '../shared/shared.module';
import { CustomerAddressModalComponent } from './address-modal/address-modal.component';
import { CustomerPreferenceComponent } from './preference/preference.component';
import { CustomerCreditCardComponent } from './credit-card/credit-card.component';
import { CustomerReferralComponent } from './referral/referral.component';
import { CustomerPromotionComponent } from './promotion/promotion.component';
import { CustomerAccountReceivablesComponent } from './account-receivables/account-receivables.component';
import { CustomerNotificationComponent } from './notification/notification.component';
import { CustomerSettingComponent } from './setting/setting.component';
import { CustomerLookupComponent } from './lookup/lookup.component';
import { CustomerPricingComponent } from './pricing/pricing.component';
import { CustomerCrmComponent } from './crm/crm.component';
import { CustomerLayoutComponent } from './layout/layout.component';


@NgModule({
  imports: [
    CommonModule, SharedModule,
    RouterModule.forChild(CustomerRoutes)
  ],
  declarations: [CustomerInformationComponent, CustomerAddressModalComponent, CustomerPreferenceComponent, CustomerCreditCardComponent, CustomerReferralComponent, CustomerPromotionComponent, CustomerAccountReceivablesComponent, CustomerNotificationComponent, CustomerSettingComponent, CustomerLookupComponent, CustomerPricingComponent, CustomerCrmComponent, CustomerLayoutComponent]
})
export class CustomerModule { }
