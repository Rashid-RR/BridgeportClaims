import { Component, OnInit, AfterViewInit } from '@angular/core'; 
import { DecisionTreeService } from '../../services/decision-tree.service'; 


@Component({
  selector: 'tree-list',
  templateUrl: './tree-list.component.html',
  styleUrls: ['./tree-list.component.css'],
})
export class TreeListComponent implements OnInit, AfterViewInit {

   
  constructor(public ds: DecisionTreeService) {
    this.ds.search();
  }
  ngOnInit() {
    
  }
  ngAfterViewInit() {

  }
  

}

