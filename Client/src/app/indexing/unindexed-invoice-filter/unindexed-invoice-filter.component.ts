import { Component, NgZone, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
// Services
import { DocumentManagerService } from '../../services/document-manager.service';
declare var $: any;

@Component({
  selector: 'indexing-unindexed-invoice-filter',
  templateUrl: './unindexed-invoice-filter.component.html',
  styleUrls: ['./unindexed-invoice-filter.component.css']
})
export class UnindexedInvoiceFilterComponent implements OnInit, AfterViewInit {

  date: string;
  fileName: string;
  submitted = false;
  constructor(
    public ds: DocumentManagerService,
    private dp: DatePipe,
    private zone: NgZone,
    private route: ActivatedRoute,
    private toast: ToastrService,
    private fb: FormBuilder) { }

  ngOnInit() {

  }
  ngAfterViewInit() {
    // Date picker
    $('#invoicechecksdate').datepicker({
      autoclose: true
    });
    this.route.params.subscribe(params => {
      if (params['date'] && params['date'] != 'invoice') {
        this.date = params['date'].replace(/\-/g, '/');
        this.zone.run(() => {
        });
      }
    });
  }

  search() {
    const date = this.dp.transform($('#invoicechecksdate').val(), 'MM/dd/yyyy');
    this.ds.invoiceData.date = date || null;
    this.ds.invoiceData.fileName = this.fileName || null;
    this.ds.searchInvoices();
  }

  filter($event) {
    this.ds.invoiceData.archived = $event.target.checked;
  }
  clearFilters() {
    $('#invoicechecksdate').val('');
    $('#IarchivedCheck').prop('checked', false);
    this.fileName = '';
  }


}
