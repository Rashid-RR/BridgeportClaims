import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { EventsService } from './events-service';
import { ToastsManager } from 'ng2-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { UUID } from 'angular2-uuid';
import * as Immutable from 'immutable';
import { SortColumnInfo } from "../directives/table-sort.directive";
import { ArraySortPipe } from '../pipes/sort.pipe';
import { Firewall } from '../interfaces/firewall';
declare var $: any;
@Injectable()
export class FirewallService {

  loading: boolean = false;
  firewalls: Immutable.OrderedMap<any, Firewall> = Immutable.OrderedMap<Number, Firewall>();
  data: any = {};
  display: string = "list";
  goToPage: any = '';
  totalRowCount: number;
  form: FormGroup;
  constructor(private http: HttpService, private formBuilder: FormBuilder, private sortPipe: ArraySortPipe,
    private events: EventsService, private toast: ToastsManager) {
    this.data = {
      startDate: null,
      endDate: null,
      FirewallCategoryId: null,
      FirewallTypeId: null,
      OwnerID: null,
      resolved: false,
      sortColumn: "Created",
      sortDirection: "DESC",
      pageNumber: 1,
      pageSize: 30
    };
    this.form = this.formBuilder.group({
      endIpAddress: [null, Validators.required],
      ruleName: [null, Validators.compose([Validators.required, Validators.pattern(new RegExp(/^[a-z0-9]+$/i))])],
      startIpAddress: [null, Validators.required]
    });
    this.search();
  }

  refresh() {

  }
  deleteFirewall(fw: Firewall) {
    this.loading = true;
    this.http.deleteFirewallSetting(fw)
      .subscribe((result: any) => {
        this.loading = false;
        this.firewalls = this.firewalls.delete(fw.ruleName);
        this.toast.success(result.message);
      }, err => {
        this.loading = false;
        try {
          const error = err.error;
        } catch (e) { }
      }, () => {
        this.events.broadcast('Firewall-list-updated');
      });
  }

  get totalPages() {
    return this.totalRowCount ? Math.ceil(this.totalRowCount / this.data.pageSize) : null;
  }
  get firewallList(): Array<Firewall> {
    return this.firewalls.toArray();
  }
  onSortColumn(info: SortColumnInfo) {
    this.data.isDefaultSort = false;
    this.data.sortColumn = info.column;
    this.data.sortDirection = info.dir;
    this.search();
  }

  search(next: Boolean = false, prev: Boolean = false, page: number = undefined) {
    if (!this.data) {
      this.toast.warning('Please populate at least one search field.');
    } else {
      this.loading = true;
      let data = JSON.parse(JSON.stringify(this.data)); //copy data instead of memory referencing

      if (next) {
        data.pageNumber++;
      }
      if (prev && data.pageNumber > 1) {
        data.pageNumber--;
      }
      if (page) {
        data.pageNumber = page;
      }
      this.http.getFirewallSettings(data)
        .subscribe((result: any) => {
          this.loading = false;
          this.totalRowCount = result.totalRowCount;
          this.firewalls = Immutable.OrderedMap<any, Firewall>();
          result.forEach((firewall: Firewall) => {
            try {
              this.firewalls = this.firewalls.set(firewall.ruleName, firewall);
            } catch (e) { }
          });
          if (next && result && result.length > 0) {
            this.data.pageNumber++;
          }
          if (prev) {
            this.data.pageNumber--;
          }
          if (page && result && result.length > 0) {
            this.data.pageNumber = page;
          }
          if (!prev && !next && !page) {
            this.data.pageNumber = this.totalRowCount == 0 ? null : 1;
            this.goToPage = this.totalRowCount == 0 ? null : this.data.pageNumber;
          }
        }, err => {
          this.loading = false;
          try {
            const error = err.error;
          } catch (e) { }
        }, () => {
          this.events.broadcast('Firewall-list-updated');
        });
    }
  }
  save() {
    this.form.controls['startIpAddress'].setValue($('#startIpAddress').val())
    this.form.controls['endIpAddress'].setValue($('#endIpAddress').val())
    if (!this.form.valid) {
      if (this.form.controls['ruleName'].errors && this.form.controls['ruleName'].errors.pattern) {
        this.toast.warning('Rule Name must be alphanumeric only');
      } else {
        this.toast.warning('Please fill in all the form fields');
      }
    } else {
      this.loading = true;
      this.http.createFirewallSetting(this.form.value)
        .subscribe((result: any) => {
          let form = this.form.value;
          this.firewalls = this.firewalls.set(form.ruleName, form);
          this.loading = false;
          this.toast.success(result.message);
          this.form.reset();
        }, err => {
          this.loading = false;
          try {
            const error = err.error;
          } catch (e) { }
        });
    }
  }
  get pages(): Array<any> {
    return new Array(this.data.pageNumber);
  }
  get pageStart() {
    return this.totalRowCount == 0 ? 0 : (this.totalRowCount > 0 && this.firewallList.length > 1 ? ((this.data.pageNumber - 1) * this.data.pageSize) + 1 : null);
  }
  get pageEnd() {
    return this.firewallList.length > 1 ? (this.data.pageSize > this.firewallList.length ? ((this.data.pageNumber - 1) * this.data.pageSize) + this.firewallList.length : (this.data.pageNumber) * this.data.pageSize) : null;
  }

  get end(): Boolean {
    return this.pageStart && this.data.pageSize > this.firewallList.length;
  }

}
