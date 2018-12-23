import { Component, NgZone, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
// Services
import { DocumentManagerService } from '../../services/document-manager.service';
declare var $: any;

@Component({
  selector: 'indexing-unindexed-invalid-check-filter',
  templateUrl: './unindexed-invalid-check-filter.component.html',
  styleUrls: ['./unindexed-invalid-check-filter.component.css']
})
export class UnindexedInvalidCheckFilterComponent implements OnInit, AfterViewInit {

  date: string;
  fileName: string;
  submitted = false;
  constructor(
    private dp: DatePipe,
    public ds: DocumentManagerService,
    private zone: NgZone,
    private route: ActivatedRoute) { }

  ngOnInit() {

  }
  ngAfterViewInit() {
    // Date picker
    $('#invchecksdate').datepicker({
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
    const date = this.dp.transform($('#invchecksdate').val(), 'MM/dd/yyyy');
    this.ds.invalidChecksData.date = date || null;
    this.ds.invalidChecksData.fileName = this.fileName || null;
    this.ds.searchInvalidCheckes();
  }


  clearFilters() {
    $('#invchecksdate').val('');
    $('#CarchivedCheck').prop('checked', false);
    this.fileName = '';
  }


}
