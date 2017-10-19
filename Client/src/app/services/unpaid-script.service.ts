import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { EventsService } from './events-service';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { FormBuilder,FormControl, FormGroup, Validators} from "@angular/forms";
import { UUID } from 'angular2-uuid';
import * as Immutable from 'immutable';
import { SortColumnInfo } from "../directives/table-sort.directive";

import { UnpaidScript } from '../models/unpaid-script';
@Injectable()
export class UnpaidScriptService {

  loading: Boolean = false; 
  unpaidscripts: Immutable.OrderedMap<Number, UnpaidScript> = Immutable.OrderedMap<Number, UnpaidScript>();
  data:any={};
  constructor(private http: HttpService,private formBuilder: FormBuilder, private events: EventsService, private toast: ToastsManager) { 
    this.data ={
      isDefaultSort: true,
      startDate: null,
      endDate: null,
      sort: 'RxDate',
      sortDirection: 'ASC',
      page: 1,
      pageSize: 500
    }; 
    //this.search();
  }

  refresh() {

  }
  get pages():Array<any>{
    return new Array(this.data.page);
  }
  get pageStart(){
      return this.unpaidScriptList.length>1 ? ((this.data.page-1)*this.data.pageSize)+1 : null;
  }
  get pageEnd(){
    return this.unpaidScriptList.length>1 ? (this.data.page)*this.data.pageSize : null;
  }
  get unpaidScriptList():Array<UnpaidScript>{
    return this.unpaidscripts.toArray();
  }
  onSortColumn(info: SortColumnInfo) {
    this.data.isDefaultSort = false;
    this.data.sort = info.column;
    this.data.sortDirection = info.dir.toUpperCase();
    this.search();
  }
  
  search(next:Boolean=false,prev:Boolean=false,first:Boolean=false,last:Boolean=false){
    if (!this.data) {
      this.toast.warning('Please populate at least one search field.');
    } else {
      this.loading = true;
      let data = JSON.parse(JSON.stringify(this.data)); //copy data instead of memory referencing
      if(first){
        data.page=1;
      }
      if(next){
        data.page++;
      }
      if(prev && data.page>1){
        data.page--;
      }
       this.http.unpaidScriptsList(data).map(res => { return res.json(); })
        .subscribe((result: any) => {
          this.loading = false;
          this.unpaidscripts= Immutable.OrderedMap<Number, UnpaidScript>(); 
          result.forEach((script:UnpaidScript)=>{
            try{
              this.unpaidscripts = this.unpaidscripts.set(script.claimId,script);
            }catch(e){}           
          }); 
          if(next){
            this.data.page++;
          }
          if(prev && this.data.page !=data.page){
            this.data.page--;
          }         
        }, err => {
          this.loading = false;
          try {
            const error = err.json(); 
          } catch (e) { }
        }, () => {
          this.events.broadcast('unpaid-script-list-updated');
        }); 
    }
  }

}
