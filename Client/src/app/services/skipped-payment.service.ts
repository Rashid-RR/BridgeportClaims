import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { SortColumnInfo } from "../directives/table-sort.directive";
import { Router } from '@angular/router';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

export interface SkippedPayment {
    lastName: string,
    firstName: string,
    rxNumber: number,
    reversedDate: Date,
    claimNumber: number,
    invoiceNumber: number,
    dateFilled: any,
    amountPaid: number,
    statusName: string,
    prescriptionPaymentId?: number
}

@Injectable()
export class SkippedPaymentService {

    loading: boolean = false;
    deleting: boolean = false; 
    goToPage: any = '';
    routes: string[] = [];
    skippedPay: SkippedPayment[] = [];
    data: any = {};
    totalRowCount: number;
    constructor(private router: Router, private toast: ToastsManager, private http: HttpService) {
        this.data = {
            sort: "RxNumber",
            sortDirection: "DESC",
            page: 1,
            pageSize: 30
        }
    }
    fetchSkippedPayReport(next: Boolean = false, prev: Boolean = false, page: number = undefined) {
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
            this.http.skippedPaymentList(data).single().map(r => r.json()).subscribe(r => {
                this.skippedPay = r.results || r;
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
    removeskippedPay(id: number = undefined) {
        this.loading = true;
         
    }
    onSortColumn(info: SortColumnInfo) {
        this.data.isDefaultSort = false;
        this.data.sort = info.column;
        this.data.sortDirection = info.dir;
        this.data.page = 1;
        this.data.goToPage = '';
        this.fetchSkippedPayReport();
    }
    get totalPages() {
        return this.totalRowCount ? Math.ceil(this.totalRowCount / this.data.pageSize) : null;
    }
    get skippedPayList(): SkippedPayment[] {
        return this.skippedPay || [];
    }
    get pages(): Array<any> {
        return new Array(this.data.page);
    }
    get pageStart() {
        return this.skippedPay.length > 1 ? ((this.data.page - 1) * this.data.pageSize) + 1 : null;
    }
    get pageEnd() {
        return this.skippedPay.length > 1 ? (this.data.pageSize > this.skippedPay.length ? ((this.data.page - 1) * this.data.pageSize) + this.skippedPay.length : (this.data.page) * this.data.pageSize) : null;
    }

    get end(): Boolean {
        return this.pageStart && this.data.pageSize > this.skippedPay.length;
    }
}