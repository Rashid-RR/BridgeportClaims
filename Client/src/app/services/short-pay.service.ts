
import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { SortColumnInfo } from '../directives/table-sort.directive';
import { Router, NavigationEnd } from '@angular/router';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

export interface ShortPay {
    lastName: string;
    firstName: string;
    rxNumber: number;
    rxDate: Date;
    claimNumber: number;
    billedAmount: any;
    amountPaid: number;
    prescriptionStatus: string;
    prescriptionPaymentId?: number;
}

@Injectable()
export class ShortPayService {

    loading = false;
    deleting = false;
    goToPage: any = '';
    routes: string[] = [];
    shortpay: ShortPay[] = [];
    data: any = {};
    totalRowCount: number;
    constructor(private router: Router, private toast: ToastsManager, private http: HttpService) {
        this.data = {
            sort: 'RxNumber',
            sortDirection: 'DESC',
            page: 1,
            pageSize: 30
        }
    }
    fetchShortpayReport(next: Boolean = false, prev: Boolean = false, page: number = undefined) {
        if (!this.data) {
            this.toast.warning('Please populate at least one search field.');
        } else {
            this.loading = true;
            const data = JSON.parse(JSON.stringify(this.data)); // copy data instead of memory referencing

            if (next) {
                data.page++;
            }
            if (prev && data.page > 1) {
                data.page--;
            }
            if (page) {
                data.page = page;
            }
            this.http.shortPayList(data).single().map(r => r.json()).subscribe(r => {
                this.shortpay = r.results || r;
                this.totalRowCount = r.totalRowCount || r.length;
                this.loading = false;
                if (next) {
                    this.data.page++;
                }
                if (prev && this.data.page !== data.page) {
                    this.data.page--;
                }
                if (page) {
                    this.data.page = page;
                }
            }, err => {
                this.loading = false;
                this.shortpay = [{}as any];
            });
        }
    }

    removeShortpay(id: number = undefined) {
        this.loading = true;
        this.http.removeShortPay({ prescriptionPaymentId: id }).single().map(r => r.json()).subscribe(res => {
            this.loading = false;
            this.toast.success(res.message);
            this.fetchShortpayReport();
        }, err => {
            this.loading = false;
            const error = err.json();
            this.toast.error(error.Message || error.message);
        });
    }
    onSortColumn(info: SortColumnInfo) {
        this.data.isDefaultSort = false;
        this.data.sort = info.column;
        this.data.sortDirection = info.dir;
        this.data.page = 1;
        this.data.goToPage = '';
        this.fetchShortpayReport();
    }
    get totalPages() {
        return this.totalRowCount ? Math.ceil(this.totalRowCount / this.data.pageSize) : null;
    }
    get shortpayList(): ShortPay[] {
        return this.shortpay || [];
    }
    get pages(): Array<any> {
        return new Array(this.data.page);
    }
    get pageStart() {
        return this.shortpay.length > 1 ? ((this.data.page - 1) * this.data.pageSize) + 1 : null;
    }
    get pageEnd() {
        return this.shortpay.length > 1 ? (this.data.pageSize > this.shortpay.length ? ((this.data.page - 1) * this.data.pageSize)
        + this.shortpay.length : (this.data.page) * this.data.pageSize) : null;
    }

    get end(): Boolean {
        return this.pageStart && this.data.pageSize > this.shortpay.length;
    }
}
