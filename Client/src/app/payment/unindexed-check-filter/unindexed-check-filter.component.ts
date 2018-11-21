import { Component, NgZone, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
// Services
import { DocumentManagerService } from "../../services/document-manager.service";
import { EventsService } from "../../services/events-service";
declare var $: any;

@Component({
  selector: 'payment-unindexed-check-filter',
  templateUrl: './unindexed-check-filter.component.html',
  styleUrls: ['./unindexed-check-filter.component.css']
})
export class PaymentCheckFilterComponent implements OnInit, AfterViewInit {

  date: string;
  fileName: string;
  submitted: boolean = false;
  constructor(
    public ds: DocumentManagerService,
    private dp: DatePipe,
    private zone: NgZone,
    private events: EventsService,
    private route: ActivatedRoute) { 
      this.events.on("payment-suspense", () => {
        this.search();
      });
      this.events.on("payment-closed", () => {
        this.search();
      });
    }

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
    this.ds.checksData.fileName = this.fileName || null;
    this.ds.viewPostedDetail = undefined;
    this.ds.searchCheckes(); 
  }

  filter($event) {
    console.log($event.target.value)
    switch($event.target.value){
      case 'posted':
      this.ds.postedChecks = true
        this.ds.viewPostedDetail=false

        this.search();

       break;
      case 'archived':
      this.ds.postedChecks = false
        this.ds.viewPostedDetail=false
      this.ds.archivedChecksData.archived = $event.target.checked;
        this.search();


        break;
      case 'default':
      this.ds.postedChecks = false
      this.ds.archivedChecksData.archived = null;
        this.search();


        break;

    }
  }
  clearFilters() {
    $('#checksdate').val('');
    $('#CarchivedCheck').prop('checked',false);
    $('#CarchivedCheck1').prop('checked',false);
    this.ds.viewPostedDetail = undefined;
    this.fileName = '';
  }


}
