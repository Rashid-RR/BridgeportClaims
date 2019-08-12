import { InvoiceProcessService } from './../../services/invoice-process.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-invoice-process',
  templateUrl: './invoice-process.component.html',
  styleUrls: ['./invoice-process.component.css']
})
export class InvoiceProcessComponent implements OnInit {

  constructor(public invoiceProcessService: InvoiceProcessService) {}

  ngOnInit() {
  }

}
