import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { EventsService } from './events-service';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import {FormBuilder,FormControl, FormGroup, Validators} from "@angular/forms";
import { UUID } from 'angular2-uuid';
import * as Immutable from 'immutable';
import { SortColumnInfo } from "../directives/table-sort.directive";

import { Diary } from '../models/diary';
@Injectable()
export class DiaryService {

  loading: Boolean = false; 
  diaries: Immutable.OrderedMap<Number, Diary> = Immutable.OrderedMap<Number, Diary>();
  data:any={};
  constructor(private http: HttpService,private formBuilder: FormBuilder, private events: EventsService, private toast: ToastsManager) { 
    this.data ={
      isDefaultSort: true,
      startDate: null,
      endDate: null,
      sort: "InsuranceCarrier",
      sortDirection: "ASC",
      page: 1,
      pageSize: 5000
    };
  }

  refresh() {

  }

  get diaryList():Array<Diary>{
    return this.diaries.toArray();
  }
  onSortColumn(info: SortColumnInfo) {
    this.data.isDefaultSort = false;
    this.data.sort = info.column;
    this.data.sortDirection = info.dir;
    this.search();
  }
  
  search(){
    if (!this.data) {
      this.toast.warning('Please populate at least one search field.');
    } else {
      this.loading = true;
      this.http.diaryList(this.data).map(res => { return res.json(); })
        .subscribe((result: any) => {
          this.loading = false;
          this.diaries= Immutable.OrderedMap<Number, Diary>(); 
          result.forEach((diary:Diary)=>{
            try{
                this.diaries = this.diaries.set(diary.diaryId,diary);
            }catch(e){}           
          });
          setTimeout(()=>{
            //this.events.broadcast('payment-amountRemaining',result)
          },200);           
        }, err => {
          this.loading = false;
          try {
            const error = err.json(); 
          } catch (e) { }
        }, () => {
          this.events.broadcast('diary-list-updated');
        });
    }
  }

}
