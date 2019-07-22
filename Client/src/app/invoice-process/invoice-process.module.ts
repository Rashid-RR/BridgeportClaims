import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { InvoiceProcessRoutes } from './invoice-process.routing';
import { SharedModule } from '../shared';
import { InvoiceProcessComponent } from './invoice-process/invoice-process.component';
import { InvoiceProcessListComponent } from './invoice-process-list/invoice-process-list.component';
import { InvoiceProcessSearchFilterComponent } from './invoice-process-search-filter/invoice-process-search-filter.component';
import { InvoiceProcessStateCellRendererComponent } from './invoice-process-list/invoice-process-states-cell-renderer.component';
import { AgDateFilterComponent } from '../components/ag-date-filter/ag-date-filter.component';
import { AgGridModule } from 'ag-grid-angular';

@NgModule({
  imports: [
    SharedModule,
    CommonModule,
    RouterModule.forChild(InvoiceProcessRoutes),
    AgGridModule.withComponents([InvoiceProcessStateCellRendererComponent, AgDateFilterComponent])
  ],
  declarations: [
   InvoiceProcessComponent,
   InvoiceProcessListComponent,
   InvoiceProcessSearchFilterComponent,
   InvoiceProcessStateCellRendererComponent
  ]
})
export class InvoiceProcessModule { }
