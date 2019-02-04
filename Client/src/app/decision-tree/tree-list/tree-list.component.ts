import { Component, Input, OnInit, AfterViewInit } from '@angular/core';
import { DecisionTreeService } from '../../services/decision-tree.service';
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: 'tree-list',
  templateUrl: './tree-list.component.html',
  styleUrls: ['./tree-list.component.css'],
})
export class TreeListComponent implements OnInit, AfterViewInit {

  @Input() claimId: string
  constructor(public ds: DecisionTreeService, private route: ActivatedRoute) {
    this.route.params.subscribe(s => {
      this.claimId = s.claimId;
      this.ds.search();
    })
  }
  ngOnInit() {

  }
  ngAfterViewInit() {

  }


}

