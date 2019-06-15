import { Component, OnInit } from '@angular/core';
import { QueryBuilderService } from '../../services/services.barrel';

@Component({
  selector: 'app-claims-data-search-filter',
  templateUrl: './claims-data-search-filter.component.html',
  styleUrls: ['./claims-data-search-filter.component.css']
})
export class ClaimsDataSearchFilterComponent implements OnInit {

  constructor(public queryBuilderSvc: QueryBuilderService) { }

  ngOnInit() {
  }

  clearFilter(): void {
    this.queryBuilderSvc.filterText = '';
  }

  refreshList(): void {
    this.queryBuilderSvc.filterText = '';
    this.queryBuilderSvc.refreshList$.next(true);
  }
}
