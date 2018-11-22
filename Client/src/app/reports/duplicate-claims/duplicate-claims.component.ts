import { Component, OnInit } from '@angular/core';
import { ReportLoaderService } from '../../services/services.barrel';

@Component({
  selector: 'app-duplicate-claims',
  templateUrl: './duplicate-claims.component.html',
  styleUrls: ['./duplicate-claims.component.css']
})
export class DuplicateClaimsComponent implements OnInit {

  constructor(public reportloader: ReportLoaderService) { }

  ngOnInit() {
  }

}
