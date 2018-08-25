import { Component, OnInit } from '@angular/core';
import { ReportLoaderService } from "../../services/services.barrel";

@Component({
  selector: 'app-duplicate-claim-search',
  templateUrl: './duplicate-claim-search.component.html',
  styleUrls: ['./duplicate-claim-search.component.css']
})
export class DuplicateClaimSearchComponent implements OnInit {

  constructor(public reportloader:ReportLoaderService) { }

  ngOnInit() {
    this.reportloader.fetchDuplicateClaims();
  }

}
