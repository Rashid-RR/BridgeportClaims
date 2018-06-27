import { Component, OnInit } from '@angular/core';
import { ReportLoaderService } from "../../services/services.barrel";

@Component({
  selector: 'app-report-list',
  templateUrl: './report-list.component.html',
  styleUrls: ['./report-list.component.css']
})
export class ReportListComponent implements OnInit {

  constructor(public reportloader:ReportLoaderService) { }

  ngOnInit() {
    this.reportloader.current = 'Menu';
    this.reportloader.currentURL = 'list';
    this.reportloader.loading = false;
  }

}
