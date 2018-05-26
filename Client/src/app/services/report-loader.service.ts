import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { SortColumnInfo } from "../directives/table-sort.directive";
import { Router, NavigationEnd } from '@angular/router';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

export interface DuplicateClaim {
  lastName: string,
  firstName: string,
  claimId: number,
  dateOfBirth: Date,
  claimNumber: number,
  personCode: any,
  groupName: string
}
@Injectable()
export class ReportLoaderService {
  loading: Boolean = false;
  current: String = 'List';
  currentURL: String = 'List';
  routes: string[] = [];
  duplicates: DuplicateClaim[] = [];
  data: any = {};
  totalRowCount: number;
  constructor(private router: Router, private toast: ToastsManager, private http: HttpService) {
    this.router.events.subscribe(e => {
      if (e instanceof NavigationEnd) {
        this.routes = this.router.url.split("/");
      }
    });
    this.data = {
      sort: "lastName",
      sortDirection: "ASC",
      page: 1,
      pageSize: 30
    }
  }

  fetchDuplicateClaims(next: Boolean = false, prev: Boolean = false, page: number = undefined) {
    if (!this.data) {
      this.toast.warning('Please populate at least one search field.');
    } else {
      this.loading = true;
      let data = JSON.parse(JSON.stringify(this.data)); //copy data instead of memory referencing

      if (next) {
        data.page++;
      }
      if (prev && data.page > 1) {
        data.page--;
      }
      if (page) {
        data.page = page;
      }
      this.loading = true;
      this.http.duplicateClaims(this.data).single().map(r => r.json()).subscribe(r => {
        this.duplicates = r.claimResults || r;
        this.totalRowCount = r.totalRowCount || r.length;
        this.loading = false;
        if (next) {
          this.data.page++;
        }
        if (prev && this.data.page != data.page) {
          this.data.page--;
        }
        if (page) {
          this.data.page = page;
        }
      }, err => {
        this.loading = false;
      })
    }
  }
  get duplicateClaims() {
    return this.duplicates;
  }
  formatDate(input: String) {
    if (!input) return null;
    if (input.indexOf("-") > -1) {
      let date = input.split("T");
      let d = date[0].split("-");
      return d[1] + "/" + d[2] + "/" + d[0];
    } else {
      return input;
    }
  }
  onSortColumn(info: SortColumnInfo) {
    this.data.isDefaultSort = false;
    this.data.sort = info.column;
    this.data.sortDirection = info.dir;
    //this.fetchDuplicateClaims();
  }
  get totalPages() {
    return this.totalRowCount ? Math.ceil(this.totalRowCount / this.data.pageSize) : null;
  }
  get pages(): Array<any> {
    return new Array(this.data.page);
  }
  get pageStart() {
    return this.duplicates.length > 1 ? ((this.data.page - 1) * this.data.pageSize) + 1 : null;
  }
  get pageEnd() {
    return this.duplicates.length > 1 ? (this.data.pageSize > this.duplicates.length ? ((this.data.page - 1) * this.data.pageSize) + this.duplicates.length : (this.data.page) * this.data.pageSize) : null;
  }

  get end(): Boolean {
    return this.pageStart && this.data.pageSize > this.duplicates.length;
  }

}
