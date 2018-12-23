import { Component } from '@angular/core';
// Services
import { ReportLoaderService } from '../../services/services.barrel';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent {

  location: String = '';
  constructor(public reportloader: ReportLoaderService) {
  }
}
