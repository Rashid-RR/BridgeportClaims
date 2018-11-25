import { Component } from '@angular/core';
import { Router } from '@angular/router';
// Services
import { ReportLoaderService } from '../../services/services.barrel';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent {

  location: String= '';
  constructor(private router: Router, public reportloader: ReportLoaderService) {
  }
}
