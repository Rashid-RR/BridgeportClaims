import { NgModule } from '@angular/core';
import { CurrencyPipe, DecimalPipe, CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SweetAlert2Module } from '@toverux/ngx-sweetalert2';
import { SharedModule } from '../shared';
import {ReferencesRoutes} from './references.routing';
import {ReferencesComponent} from './references/references.component';
import { ReferencesfilterComponent } from './referencesfilter/referencesfilter.component';


@NgModule({
  imports: [
    CommonModule,
    SweetAlert2Module.forRoot(),
    SharedModule,
    RouterModule.forChild(ReferencesRoutes),
  ],
  declarations: [
    ReferencesComponent,
    ReferencesfilterComponent
  ],
  providers: [CurrencyPipe, DecimalPipe],

})
export class ReferencesModule { }
