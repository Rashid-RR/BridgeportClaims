import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReferralDefaultComponent } from './default/default.component';
import { RouterModule } from '@angular/router';
import { ReferralRoutes } from './referral.routing';
import { SharedModule } from '../shared';
import { MaterialComponentsModule } from '../material/material-components.module';

@NgModule({
  imports: [
    SharedModule,
    CommonModule,
    MaterialComponentsModule,
    RouterModule.forChild(ReferralRoutes),
  ],
  declarations: [ReferralDefaultComponent]
})
export class ReferralModule { }
