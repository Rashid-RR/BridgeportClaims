import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Payor } from '../models/payor';
import { Subject } from 'rxjs';
import { map } from 'rxjs/operators';

declare var $: any;

export interface SkippedPayment {
    lastName: string;
    firstName: string;
    rxNumber: number;
    reversedDate: Date;
    claimNumber: number;
    invoiceNumber: number;
    dateFilled: any;
    amountPaid: number;
    statusName: string;
    prescriptionPaymentId?: number;
}

@Injectable()
export class SkippedPaymentService {

    loading = false;
    deleting = false;
    goToPage: any = '';
    routes: string[] = [];
    skippedPay: SkippedPayment[] = [];
    data: any = {};
    totalRowCount: number;
    payors: Array<Payor> = [];
    pageNumber: number;
    pageSize = 50;
    payorListReady = new Subject<any>();
    archived = false;
    constructor(private router: Router, private toast: ToastrService, private http: HttpService) {
        this.data = {
            page: 1,
            archived: false,
            pageSize: 30
        };
    }
    getPayors(pageNumber: number) {
        this.loading = true;
        this.http.getPayorList()
            .pipe(map(res => { this.loading = false; return res; }))
            .subscribe((result: Payor[]) => {
            this.payors = result;
            this.pageNumber = pageNumber;
            this.payorListReady.next();
        }, err => {
            this.payorListReady.next();
        });
    }
    removeSkippedPay(id: number = undefined) {
        this.loading = true;
        this.http.removeSkippedPay({ prescriptionId: id }).subscribe(res => {
            this.loading = false;
            this.toast.success(res.message);
            this.fetchSkippedPayReport();
        }, err => {
            this.loading = false;
            const error = err.error;
            this.toast.error(error.Message || error.message);
        });
    }
    fetchSkippedPayReport(next: Boolean = false, prev: Boolean = false, page: number = undefined) {
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
            this.http.skippedPaymentList(data).subscribe(r => {
                this.skippedPay = r.results || r;
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
                this.archived = this.data.archived;
            }, err => {
                this.loading = false;
            });
        }
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
