
import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { SortColumnInfo } from '../directives/table-sort.directive';
import { Router, NavigationEnd } from '@angular/router';
import { ToastsManager } from 'ng2-toastr';

export interface CollectionBonus {
    patientName: string;
    datePosted: any;
    amountPaid: number;
    bonusAmount: any;
}

@Injectable()
export class CollectionBonusService {

    loading = false;
    deleting = false;
    goToPage: any = '';
    routes: string[] = [];
    bonus: CollectionBonus[] = [];
    data: any = {};
    totalRowCount: number;
    reportMonth: number;
    reportYear: number;
    yearRange: number[];
    constructor(private router: Router, private toast: ToastsManager, private http: HttpService) {
        this.data = {
            sort: 'RxNumber',
            sortDirection: 'DESC',
            page: 1,
            pageSize: 30
        };
        let reportDate = new Date();
        this.reportMonth = (reportDate.getMonth() + 1);
        this.reportYear = reportDate.getFullYear();
        this.yearRange = this.range(2018, this.reportYear);
    }

    get years() {
        return this.yearRange;
    }
    range(start, end): number[] {
        return Array.from({ length: (end + 1 - start) }, (v, k) => k + start)
    }
    get currentReportDate(): any {
        return `${this.reportMonth}/02/${this.reportYear}`;
    }
    fetchBonusReport(next: Boolean = false, prev: Boolean = false, page: number = undefined) {
        this.loading = true;
        this.http.collectionBonus({ month: this.reportMonth, year: this.reportYear }).single().subscribe(r => {
            this.bonus = r.results || r;
            this.totalRowCount = r.totalRowCount || r.length;
            this.loading = false;
            if (this.bonus.length == 0) {
                this.bonus = [
                    { patientName: 'TARA DALE', datePosted: '2018-06-11T00:00:00.0000000', amountPaid: 359.08, bonusAmount: 5.39 },
                    { patientName: 'ALAMASA DUALE', datePosted: '2018-06-25T00:00:00.0000000', amountPaid: 272.79, bonusAmount: 4.09 },
                    { patientName: 'KEN WARIO', datePosted: '2018-06-25T00:00:00.0000000', amountPaid: 416.15, bonusAmount: 6.24 },
                    { patientName: 'BEN KITILI', datePosted: '2018-06-25T00:00:00.0000000', amountPaid: 176.97, bonusAmount: 2.65 },
                    { patientName: 'CYNTHIA ROCK DALE', datePosted: '2018-06-11T00:00:00.0000000', amountPaid: 359.08, bonusAmount: 5.39 },
                    { patientName: 'MAN BUDALE', datePosted: '2018-06-11T00:00:00.0000000', amountPaid: 359.08, bonusAmount: 5.39 },
                    { patientName: 'OTARA LANALE', datePosted: '2018-06-11T00:00:00.0000000', amountPaid: 359.08, bonusAmount: 5.39 }
                ]
            }
        }, err => {
            this.loading = false;
            this.bonus = [{} as any];
        });
    }

    removeBonus(id: number = undefined) {
        this.loading = true;
        this.http.removeShortPay({ prescriptionId: id }).single().subscribe(res => {
            this.loading = false;
            this.toast.success(res.message);
            this.fetchBonusReport();
        }, err => {
            this.loading = false;
            const error = err.error;
            this.toast.error(error.Message || error.message);
        });
    }
    onSortColumn(info: SortColumnInfo) {
        this.data.isDefaultSort = false;
        this.data.sort = info.column;
        this.data.sortDirection = info.dir;
        this.data.page = 1;
        this.data.goToPage = '';
        this.fetchBonusReport();
    }
    get totalPages() {
        return this.totalRowCount ? Math.ceil(this.totalRowCount / this.data.pageSize) : null;
    }
    get bonusList(): CollectionBonus[] {
        return this.bonus || [];
    }
    get pages(): Array<any> {
        return new Array(this.data.page);
    }
    get pageStart() {
        return this.bonus.length > 1 ? ((this.data.page - 1) * this.data.pageSize) + 1 : null;
    }
    get pageEnd() {
        return this.bonus.length > 1 ? (this.data.pageSize > this.bonus.length ? ((this.data.page - 1) * this.data.pageSize)
            + this.bonus.length : (this.data.page) * this.data.pageSize) : null;
    }

    get end(): Boolean {
        return this.pageStart && this.data.pageSize > this.bonus.length;
    }
}
