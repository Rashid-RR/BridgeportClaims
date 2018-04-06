import { Component, NgZone, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DatePipe } from '@angular/common';
// Services
import { DocumentManagerService } from "../../services/document-manager.service";
declare var $: any;

@Component({
  selector: 'indexing-unindexed-invoice-filter',
  templateUrl: './unindexed-invoice-filter.component.html',
  styleUrls: ['./unindexed-invoice-filter.component.css']
})
export class UnindexedInvoiceFilterComponent implements OnInit, AfterViewInit {

  date: string;
  fileName: string;
  submitted: boolean = false;
  constructor(
    public ds: DocumentManagerService,
    private dp: DatePipe,
    private zone: NgZone,
    private route: ActivatedRoute,
    private toast: ToastsManager,
    private fb: FormBuilder) { }

  ngOnInit() {

  }
  ngAfterViewInit() {
    // Date picker

    this.route.params.subscribe(params => {
      if (params['date'] && params['date'] !='invoice') {
        this.date = params['date'].replace(/\-/g, "/");
        this.zone.run(() => { 
        })
      }
    });
  }

  search() {
    this.ds.invoiceData.date = null
    this.ds.invoiceData.fileName = this.fileName || null
    this.ds.searchInvoices(); 
  }

  clearFilters() {
    this.fileName = '';
  }


}
