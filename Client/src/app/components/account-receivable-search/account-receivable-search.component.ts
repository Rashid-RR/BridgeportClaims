import { Component, OnInit } from '@angular/core';
import {HttpService} from "../../services/http-service"
declare var $:any
@Component({
  selector: 'app-account-receivable-search',
  templateUrl: './account-receivable-search.component.html',
  styleUrls: ['./account-receivable-search.component.css']
})
export class AccountReceivableSearchComponent implements OnInit {

  groupName:any
  autoCompleteGroupName:string;
  constructor(public http:HttpService) {
    this.autoCompleteGroupName = this.http.baseUrl + "/reports/group-name/?groupName=:keyword";
    
   }

  ngOnInit() {
    //The Calender
   // $("#calendar").datepicker();
  }

  myListFormatter(data: any): string {
    console.log(data);
    return "";
  }
  searchGroups(){
    
  }

}
