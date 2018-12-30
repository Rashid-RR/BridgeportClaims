import { NgModule } from '@angular/core';
import { CurrencyPipe, DecimalPipe, CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SweetAlert2Module } from '@toverux/ngx-sweetalert2';
import { SharedModule } from '../shared';
import {ReferencesRoutes} from './references.routing';
import {ReferencesComponent} from './references/references.component';
import { ReferencesfilterComponent } from './referencesfilter/referencesfilter.component';
import { AdjustorsComponent } from './adjustors/adjustors.component';
import {MaterialComponentsModule} from '../material/material-components.module';


@NgModule({
  imports: [
    CommonModule,
    MaterialComponentsModule,
    SweetAlert2Module.forRoot(),
    SharedModule,
    RouterModule.forChild(ReferencesRoutes),
  ],
  declarations: [
    ReferencesComponent,
    ReferencesfilterComponent,
    AdjustorsComponent
  ],
  providers: [CurrencyPipe, DecimalPipe],

})
export class ReferencesModule { }
