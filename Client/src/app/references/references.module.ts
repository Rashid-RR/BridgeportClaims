import { NgModule } from '@angular/core';
import { CurrencyPipe, DecimalPipe, CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { SharedModule } from '../shared';
import { ReferencesRoutes } from './references.routing';
import { ReferencesfilterComponent } from './referencesfilter/referencesfilter.component';
import { ReferencesGridComponent } from './references-grid/references-grid.component';
import { MaterialComponentsModule } from '../material/material-components.module';
import { ReferencesContainerComponent } from './references-container/references-container.component';
import { EmailValidatorDirective } from '../directives/email-validator.directive';

@NgModule({
  imports: [
    CommonModule,
    MaterialComponentsModule,
    SweetAlert2Module.forRoot(),
    SharedModule,
    RouterModule.forChild(ReferencesRoutes),
  ],
  declarations: [
    ReferencesContainerComponent,
    ReferencesfilterComponent,
    ReferencesGridComponent,
    EmailValidatorDirective
  ],
  providers: [CurrencyPipe, DecimalPipe],
  exports: [EmailValidatorDirective]
})
export class ReferencesModule { }

