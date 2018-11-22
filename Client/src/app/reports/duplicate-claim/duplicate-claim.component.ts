import { Component, OnInit } from '@angular/core';
import { ReportLoaderService } from '../../services/services.barrel';

@Component({
  selector: 'app-duplicate-claim',
  templateUrl: './duplicate-claim.component.html',
  styleUrls: ['./duplicate-claim.component.css']
})
export class DuplicateClaimComponent implements OnInit {

  constructor(public reportloader: ReportLoaderService) { }

  ngOnInit() {
    this.reportloader.current = 'Duplicate Claims Report';
    this.reportloader.currentURL = 'duplicate-claims';
  }

}
