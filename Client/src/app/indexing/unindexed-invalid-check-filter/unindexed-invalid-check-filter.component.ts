import { Component, NgZone, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
// Services
import { DocumentManagerService } from "../../services/document-manager.service";
declare var $: any;

@Component({
  selector: 'indexing-unindexed-invalid-check-filter',
  templateUrl: './unindexed-invalid-check-filter.component.html',
  styleUrls: ['./unindexed-invalid-check-filter.component.css']
})
export class UnindexedInvalidCheckFilterComponent implements OnInit, AfterViewInit {

  date: string;
  fileName: string;
  submitted: boolean = false;
  constructor(
    public ds: DocumentManagerService,
    private zone: NgZone,
    private route: ActivatedRoute) { }

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
    this.ds.invalidChecksData.date = null
    this.ds.invalidChecksData.fileName = this.fileName || null
    this.ds.searchInvalidCheckes(); 
  }

  filter($event) {
    this.ds.invalidChecksData.archived = $event.target.checked;
  }
  clearFilters() {
    this.fileName = '';
  }


}
