import { Component, OnInit } from '@angular/core';
import { AccountReceivableService } from '../../services/services.barrel';

@Component({
  selector: 'app-account-receivable-result',
  templateUrl: './account-receivable-result.component.html',
  styleUrls: ['./account-receivable-result.component.css']
})
export class AccountReceivableResultComponent implements OnInit {

  constructor(public ar: AccountReceivableService) { }

  ngOnInit() {
  }

}
