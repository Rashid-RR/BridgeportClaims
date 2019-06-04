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

  // onFilterTextBoxChanged() {
  //   gridOptions.api.setQuickFilter(document.getElementById('filter-text-box').value);
  // }
}
