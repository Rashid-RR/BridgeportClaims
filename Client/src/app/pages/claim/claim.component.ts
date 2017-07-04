import { Component, OnInit } from '@angular/core';
import {HttpService} from "../../services/http-service"
import {ClaimManager} from "../../services/claim-manager";

@Component({
  selector: 'app-claim',
  templateUrl: './claim.component.html',
  styleUrls: ['./claim.component.css']
})
export class ClaimsComponent implements OnInit {
  
  expanded:Boolean=false
  expandedBlade:Number=0;
  constructor(public claimManager:ClaimManager,private http:HttpService) {
     
   }

  expand(expanded:Boolean,expandedBlade:Number){
    this.expanded = expanded;
    this.expandedBlade = expandedBlade;
  }
  minimize(){
    this.expanded = false;
    this.expandedBlade = 0;
  }
  ngOnInit() {
     window['jQuery']('body').addClass('sidebar-collapse');
  }

}
