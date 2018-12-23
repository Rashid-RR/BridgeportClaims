import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReferralDefaultComponent } from './default/default.component';
import { RouterModule } from '@angular/router';
import { ReferralRoutes } from './referral.routing';
import { SharedModule } from '../shared';

@NgModule({
  imports: [
    SharedModule,
    CommonModule,
    RouterModule.forChild(ReferralRoutes),
  ],
  declarations: [ReferralDefaultComponent]
})
export class ReferralModule { }
