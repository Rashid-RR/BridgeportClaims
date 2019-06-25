import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { InvoicesService } from '../../services/services.barrel';

@Component({
  selector: 'app-invoices-search-filter',
  templateUrl: './invoices-search-filter.component.html',
  styleUrls: ['./invoices-search-filter.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class InvoicesSearchFilterComponent implements OnInit {

  constructor(public invoicesService: InvoicesService) {}

  ngOnInit(): void {}

  clearFilter(): void {
    this.invoicesService.filterText = '';
  }

  refreshList(): void {
    this.invoicesService.filterText = '';
    this.invoicesService.refreshList$.next(true);
  }
}
