import { Routes } from '@angular/router';
import { CustomerInformationComponent } from './information/information.component';
import { CustomerLayoutComponent } from './layout/layout.component';
import { CustomerPreferenceComponent } from './preference/preference.component';
import { CustomerPromotionComponent } from './promotion/promotion.component';
import { CustomerCreditCardComponent } from './credit-card/credit-card.component';
import { CustomerAccountReceivablesComponent } from './account-receivables/account-receivables.component';
import { CustomerNotificationComponent } from './notification/notification.component';
import { CustomerSettingComponent } from './setting/setting.component';
import { CustomerLookupComponent } from './lookup/lookup.component';
import { CustomerCrmComponent } from './crm/crm.component';
import { CustomerPricingComponent } from './pricing/pricing.component';

export const CustomerRoutes: Routes = [
    {
        path: '',
        component: CustomerLayoutComponent,
        children: [
            { path: '', component: CustomerInformationComponent },
            { path: 'preference', component: CustomerPreferenceComponent },
            { path: 'promotion', component: CustomerPromotionComponent },
            { path: 'credit-card', component: CustomerCreditCardComponent },
            { path: 'referral', component: CustomerPreferenceComponent },
            { path: 'account-receivable', component: CustomerAccountReceivablesComponent },
            { path: 'notification', component: CustomerNotificationComponent },
            { path: 'setting', component: CustomerSettingComponent },
            { path: 'lookup', component: CustomerLookupComponent },
            { path: 'pricing', component: CustomerPricingComponent },
            { path: 'crm', component: CustomerCrmComponent },
          
        ]
      }
    
]