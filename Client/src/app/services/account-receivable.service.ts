import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { EventsService } from './events-service';
import { ReportLoaderService } from './report-loader.service';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import {FormBuilder,FormControl, FormGroup, Validators} from "@angular/forms";
import { UUID } from 'angular2-uuid';
import * as Immutable from 'immutable';
import { SortColumnInfo } from "../directives/table-sort.directive";
import { Diary } from '../models/diary';
 
@Injectable()
export class AccountReceivableService {
 
  report: Array<any> = [];
  data:any={};
  columns:Array<String>=[];
  totalRowCount:number;
  constructor(private http: HttpService,private events: EventsService, private toast: ToastsManager,public reportLoader:ReportLoaderService) { 

  }   

  onSortColumn(info: SortColumnInfo) {
    this.data.isDefaultSort = false;
    this.data.sort = info.column;
    this.data.sortDirection = info.dir;
    this.search();
  }
  setColumns(data:Object){
    this.columns = Object.keys(data); 
  }
  capitalizeFirstLetter(string) {
    let str = string.slice(1).replace(/[0-9]/g, '');
    let num = string.slice(1).replace(/[A-Za-z]/g, '');
    return string.charAt(0).toUpperCase() + str+' '+num;
  }
  search(next:Boolean=false,prev:Boolean=false,page:number = undefined){     
      this.reportLoader.loading = true;
      let data = JSON.parse(JSON.stringify(this.data)); //copy data instead of memory referencing
       
      if(next){
        data.page++;
      }
      if(prev && data.page>1){
        data.page--;
      }   
      if(page){
        data.page=page;
      } 
      this.http.accountReceivable(data).map(res => { return res.json(); })
        .subscribe((result: Array<any>) => {
          this.reportLoader.loading = false; 
          this.report= result; 
          if(result.length>0){
            this.setColumns(result[0]);
          }
          if(next){
            this.data.page++;
          }
          if(prev && this.data.page !=data.page){
            this.data.page--;
          }      
          if(page){
            this.data.page=page;
          }           
        }, err => {
          this.reportLoader.loading = false;
          try {
            const error = err.json(); 
          } catch (e) { }
        }, () => {
          this.events.broadcast('account-receivable-report-updated');
        }); 
  }

}
