import {Injectable} from '@angular/core';
import {HttpService} from './http-service';
import {adjustorItem} from '../references/dataitems/adjustors';

@Injectable()
export class ReferenceManagerService {

  public editFlag=false;

  public loading = false;
  public adjustors: Array<adjustorItem>;
  public totalAdjustors = 0;
  public totalRows = 0;
  public pageSize: number;
  public types = ['Adjustor', 'Attorney'];

  private sort: string;
  public typeSelected: string;
  public editAdjustor: any;
  private searchText: string;
  public sortColumn: string;
  public currentPage = 1;
  public currentStartedPage: number;
  private sortType = 'ASC';
  checkDisplay = 'list';

  private data = {
    'searchText': null,
    'sort': '',
    'sortDirection': 'ASC',
    'page': this.currentPage,
    'pageSize': 30
  };

  constructor(private http: HttpService) {
    this.typeSelected = this.types[0];
    this.totalAdjustors = 0;
    this.pageSize = 30;
    this.data.sort = this.typeSelected + 'Name';
    this.searchText = null;
    this.sortColumn = 'AdjustorName';
    this.sortType = 'ASC';
    this.data.sortDirection = this.sortType;
    this.data.searchText = this.searchText;
    this.data.pageSize = this.pageSize;
    this.data.sort = this.sortColumn;
    this.fetchadjustors(this.data);
  }


  editStatus(){
    return this.editFlag;
  }
  getadjustorslist() {

    this.data.searchText = this.searchText;
    this.data.pageSize = this.pageSize;
    this.data.page = this.currentPage;
    this.data.sortDirection = this.sortType;
    this.data.sort = this.sortColumn;
    // this.data.sort = this.typeSelected + 'Name';


    if (this.typeSelected === 'Adjustor') {
      this.fetchadjustors(this.data);
    }
    else if (this.typeSelected === 'Attorney') {
      this.fetchatorney(this.data);
    }
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
        }, error1 => {
          this.loading = false;

          console.log('error');
        }
      );
  }


  fetchatorney(data: any) {
    this.loading = true;
    this.http.getatorneyname(data)
      .subscribe((result: any) => {
          this.adjustors = result.results;
          console.log(this.adjustors);
          this.totalAdjustors = result.totalRows;
          this.loading = false;
        }, error1 => {
          this.loading = false;

          console.log('error');
        }
      );
  }


  getadjustors() {
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
    return Math.floor(this.currentStartedPage);
  }

  // get checkPageStart() {
  //   return this.adjustors.length > 1 ;
  // }
}
