import {Injectable} from '@angular/core';
import {HttpService} from './http-service';
import {adjustorItem} from '../references/dataitems/adjustors';

@Injectable()
export class ReferenceManagerService {

  public loading = false;
  public adjustors: Array<adjustorItem>;
  public totalAdjustors = 0;
  public totalRows = 0;
  public pageSize: number;

  private searchText: string;
  private sortColumn: string;
  public currentPage = 1;
  public currentStartedPage: number;
  private sortType = 'ASC';
  checkDisplay = 'list';

  private data = {
    'searchText': null,
    'sort': 'AdjustorName',
    'sortDirection': 'ASC',
    'page': this.currentPage,
    'pageSize': 30
  };

  constructor(private http: HttpService) {
    this.totalAdjustors = 0;
    this.pageSize = 30;
    this.searchText = null;
    this.sortColumn = 'AdjustorName';
    this.sortType = 'ASC';
    this.data.sortDirection = this.sortType;
    this.data.searchText = this.searchText;
    this.data.pageSize = this.pageSize;
    this.data.sort = this.sortColumn;
    this.fetchadjustors(this.data)
  }


  getadjustorslist() {
    this.data.searchText = this.searchText;
    this.data.pageSize = this.pageSize;
    this.data.page = this.currentPage;
    this.data.sortDirection = this.sortType;
    this.data.sort = this.sortColumn;
    this.fetchadjustors(this.data);
  }

  onSortColumn($event) {
    console.log($event);
    this.sortColumn = $event.column;
    this.sortType = $event.dir;
    this.getadjustorslist();
  }

  setSearchText(key: any) {
    this.searchText = key;
  }

  setSortType(option: any) {
    if (this.sortType !== option) {
      this.sortType = option;
      this.getadjustorslist();
    }
  }

  fetchadjustors(data: any) {
    this.loading = true;
    this.http.getadjustorname(data)
      .subscribe((result: any) => {
          this.adjustors = result.results;
          this.totalAdjustors = result.totalRows;
          this.loading = false;
          console.log(result)
        }, error1 => {
          this.loading = false;

          console.log('error')
        }
      );
  }

  getadjustors() {
    console.log(this.adjustors);
    return this.adjustors;
  }

  getTotalAdjustors() {
    return this.totalAdjustors;
  }

  getTotalRows() {
    this.totalRows = this.totalAdjustors / this.pageSize;
    return Math.floor(this.totalRows);
  }

  getcurrentstartpage() {
    this.currentStartedPage = ((this.currentPage - 1) * this.pageSize) + 1;
    return Math.floor(this.currentStartedPage)
  }

  // get checkPageStart() {
  //   return this.adjustors.length > 1 ;
  // }
}
