import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CustomerRoutes } from './customer.routing';
import { CustomerInformationComponent } from './information/information.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  imports: [
    CommonModule, SharedModule,
    RouterModule.forChild(CustomerRoutes)
  ],
  declarations: [CustomerInformationComponent]
})
export class CustomerModule { }
