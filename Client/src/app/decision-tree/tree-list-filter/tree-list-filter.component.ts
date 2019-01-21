import { Component, NgZone, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
// Services
import { DecisionTreeService } from '../../services/decision-tree.service';

@Component({
  selector: 'tree-list-filter',
  templateUrl: './tree-list-filter.component.html',
  styleUrls: ['./tree-list-filter.component.css']
})
export class TreeListFilterComponent implements OnInit, AfterViewInit {

  date: string;
  constructor(
    public ds: DecisionTreeService,
    private router: Router) { }

  ngOnInit() {

  }
  ngAfterViewInit() {
    
  }

  search() {
    this.ds.search();
  }

  clearFilters() {
    this.ds.data.searchText=null
  }


}
