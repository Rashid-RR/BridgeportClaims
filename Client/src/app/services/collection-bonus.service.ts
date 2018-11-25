
import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { SortColumnInfo } from '../directives/table-sort.directive';
import { ToastsManager } from 'ng2-toastr';

export interface CollectionBonus {
    patientName: string;
    datePosted: any;
    amountPaid: number;
    bonusAmount: any;
    claimId:number;
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
    totalAmountPaid: number;
    totalBonusAmount: number;
    reportMonth: number;
    reportYear: number;
    yearRange: number[];
    constructor(private toast: ToastsManager, private http: HttpService) {
        this.data = {
            sort: 'RxNumber',
            sortDirection: 'DESC',
            page: 1,
            pageSize: 30
        };
        const reportDate = new Date();
        this.reportMonth = (reportDate.getMonth() + 1);
        this.reportYear = reportDate.getFullYear();
        this.yearRange = this.range(2018, this.reportYear);
    }

    get years() {
        return this.yearRange;
    }
    range(start, end): number[] {
        return Array.from({ length: (end + 1 - start) }, (v, k) => k + start);
    }
    get currentReportDate(): any {
        return `${this.reportMonth}/02/${this.reportYear}`;
    }
    fetchBonusReport(next: Boolean = false, prev: Boolean = false, page: number = undefined) {
        this.loading = true;
        this.http.collectionBonus({ month: this.reportMonth, year: this.reportYear }).single().subscribe(r => {
            this.bonus = r.results || r;
            this.totalRowCount = r.totalRowCount || r.length;
            this.totalBonusAmount = r.totalBonusAmount;
            this.totalAmountPaid = r.totalAmountPaid;
            this.loading = false;
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
