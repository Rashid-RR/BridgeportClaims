import { Component, OnInit } from '@angular/core';
import { ReportLoaderService,SkippedPaymentService } from "../../services/services.barrel";

@Component({
  selector: 'app-skipped-payment',
  templateUrl: './skipped-payment.component.html',
  styleUrls: ['./skipped-payment.component.css']
})
export class SkippedPaymentComponent implements OnInit {

  constructor(public reportloader:ReportLoaderService,public skipped:SkippedPaymentService) { }

  ngOnInit() {
    this.reportloader.current = 'Skipped Payment Report';
    this.reportloader.currentURL = 'skipped-payment';
  }

}
