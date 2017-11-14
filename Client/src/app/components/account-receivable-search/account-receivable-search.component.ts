import { Component, OnInit } from '@angular/core';
declare var $:any
@Component({
  selector: 'app-account-receivable-search',
  templateUrl: './account-receivable-search.component.html',
  styleUrls: ['./account-receivable-search.component.css']
})
export class AccountReceivableSearchComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    //The Calender
    $("#calendar").datepicker();
  }

}
