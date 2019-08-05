import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { InvoiceProcessService } from '../../services/services.barrel';

@Component({
  selector: 'app-invoice-process-search-filter',
  templateUrl: './invoice-process-search-filter.component.html',
  styleUrls: ['./invoice-process-search-filter.component.css']
})
export class InvoiceProcessSearchFilterComponent implements OnInit {
  imgSrc = 'assets/images/ButtonNormal.png';
  constructor(public invoiceProcessService: InvoiceProcessService) {}

  ngOnInit(): void {}

  clearFilter(): void {
    this.invoiceProcessService.filterText = '';
  }

  refreshList(): void {
    this.invoiceProcessService.filterText = '';
    this.invoiceProcessService.refreshList$.next(true);
  }

  generatePdfs() { }
}
