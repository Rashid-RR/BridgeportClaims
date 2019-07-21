import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { InvoicesService } from '../../services/services.barrel';

@Component({
  selector: 'app-invoice-process-search-filter',
  templateUrl: './invoice-process-search-filter.component.html',
  styleUrls: ['./invoice-process-search-filter.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class InvoiceProcessSearchFilterComponent implements OnInit {

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
