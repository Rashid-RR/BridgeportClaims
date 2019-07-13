import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../services/http-service';
import { Payor } from '../../models/payor';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-payors',
  templateUrl: './payors.component.html',
  styleUrls: ['./payors.component.css']
})
export class PayorsComponent implements OnInit {
  payors: Array<Payor> = [];
  pageNumber: number;
  pageSize = 5;
  loading: boolean;
  constructor(private http: HttpService) {
    this.loading = false;
    this.getPayors(1);
  }

  next() {
    this.getPayors(this.pageNumber + 1);
  }
  prev() {
    if (this.pageNumber > 1) {
      this.getPayors(this.pageNumber - 1);
    }
  }

  ngOnInit() {

  }

  getPayors(pageNumber: number) {
    this.loading = true;
    this.http.getPayorList()
      .pipe(map(res => { this.loading = false; return res; }))
      .subscribe(result => {
        this.payors = result;
        this.pageNumber = pageNumber;
      }, err => {

      });
  }

}
