import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'indexing-images-invoice',
  templateUrl: './images-invoice.component.html',
  styleUrls: ['./images-invoice.component.css']
})
export class ImagesInvoiceComponent implements OnInit {

  active = '1a';
  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params['invoiceNumber']) {
        this.active = '1b';
      }
    });
  }


}
