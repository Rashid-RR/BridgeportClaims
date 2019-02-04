import { Component, OnInit, AfterViewInit, Input } from '@angular/core';
import { Router } from '@angular/router';
// Services
import { DecisionTreeService } from '../../services/decision-tree.service';
import { ProfileManager } from '../../services/profile-manager';

@Component({
  selector: 'tree-list-filter',
  templateUrl: './tree-list-filter.component.html',
  styleUrls: ['./tree-list-filter.component.css']
})
export class TreeListFilterComponent implements OnInit, AfterViewInit {

  date: string;
  @Input() claimId:string
  constructor(
    private profileManager:ProfileManager,
    public ds: DecisionTreeService,
    private router: Router) { }

  ngOnInit() {

  }
  ngAfterViewInit() {
    
  }
  get allowed(): Boolean {
    return  (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array) && this.profileManager.profile.roles.indexOf('Admin') > -1);
  }

  search() {
    this.ds.search();
  }

  clearFilters() {
    this.ds.data.searchText=null
  }


}
