import { Component, OnInit } from '@angular/core';
import { AccountReceivableService,ReportLoaderService } from "../../services/services.barrel";

@Component({
  selector: 'app-report-account-receivable',
  templateUrl: './report-account-receivable.component.html',
  styleUrls: ['./report-account-receivable.component.css']
})
export class ReportAccountReceivableComponent implements OnInit {

  constructor(public ar:AccountReceivableService,public reportloader:ReportLoaderService) { }

 
  ngOnInit() {
    this.reportloader.current = 'Account Receivable';
    this.reportloader.currentURL = 'account-receivable';
    //this.ar.search();
  }

}
