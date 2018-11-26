import { Component, OnInit } from '@angular/core';
import { ReportLoaderService, ShortPayService } from '../../services/services.barrel';

@Component({
  selector: 'app-short-pay',
  templateUrl: './short-pay.component.html',
  styleUrls: ['./short-pay.component.css']
})
export class ShortPayComponent implements OnInit {
  constructor(public reportloader: ReportLoaderService, public shortpay: ShortPayService) { }

  ngOnInit() {
    this.reportloader.current = 'Shortpay Report';
    this.reportloader.currentURL = 'shortpay';
  }

}
