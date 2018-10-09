import { Component, NgZone, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
// Services
import { DocumentManagerService } from "../../services/document-manager.service";
declare var $: any;

@Component({
  selector: 'indexing-unindexed-check-filter',
  templateUrl: './unindexed-check-filter.component.html',
  styleUrls: ['./unindexed-check-filter.component.css']
})
export class UnindexedCheckFilterComponent implements OnInit, AfterViewInit {

  date: string;
  fileName: string;
  submitted: boolean = false;
  constructor(
    public ds: DocumentManagerService,
    private dp: DatePipe,
    private zone: NgZone,
    private route: ActivatedRoute) { }

  ngOnInit() {

  }
  ngAfterViewInit() {
    // Date picker
    $('#checksdate').datepicker({
      autoclose: true
    });
    this.route.params.subscribe(params => {
      if (params['date'] && params['date'] !='invoice') {
        this.date = params['date'].replace(/\-/g, "/");
        this.zone.run(() => { 
        })
      }
    });
  }

  search() {
    let date = this.dp.transform($('#checksdate').val(), "MM/dd/yyyy");
    this.ds.checksData.date = date||null
    this.ds.checksData.fileName = this.fileName || null
    this.ds.searchCheckes(); 
  }

  filter($event) {
    this.ds.checksData.archived = $event.target.checked;
  }
  clearFilters() {
    $('#checksdate').val('');
    $('#CarchivedCheck').prop('checked',false);
    this.fileName = '';
  }


}
