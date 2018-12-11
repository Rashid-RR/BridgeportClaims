import {Injectable} from '@angular/core';
import {HttpService} from './http-service';
import {adjustorItem} from '../references/dataitems/adjustors';

@Injectable()
export class ReferenceManagerService {

  public loading=false;
  public adjustors: Array<adjustorItem>;
  private totalAdjustors: any;
  private pageSize: number;
  private searchText: string;
  private currentPage=1;
  private sortType='ASC';
  checkDisplay = 'list';

  private data = {
    'searchText': null,
    'sort': 'AdjustorName',
    'sortDirection': 'ASC',
    'page': this.currentPage,
    'pageSize': 30
  };

  constructor(private http: HttpService) {
    this.searchText=null;
    this.pageSize=30;
    this.data.searchText=this.searchText;
    this.data.pageSize=this.pageSize;
    this.fetchadjustors(this.data)
  }


  getadjustorslist(){
    this.data.searchText=this.searchText;
    this.data.pageSize=this.pageSize;
    this.fetchadjustors(this.data)
  }
  setSearchText(key:any){
    this.searchText=key;
  }
  fetchadjustors(data: any) {
    this.loading=true;
    this.http.getadjustorname(data)
      .subscribe((result: any) => {
          this.adjustors = result.results;
          this.totalAdjustors = result.totalRows;
          this.loading=false;
          console.log(result)
        }, error1 => {
        this.loading=false;

        console.log('error')
        }
      );
  }

  getadjustors() {
    // console.log(this.adjustors)
    return this.adjustors;
  }

  getTotalAdjustors() {
    return this.totalAdjustors;
  }

  // get checkPageStart() {
  //   return this.adjustors.length > 1 ;
  // }
}
