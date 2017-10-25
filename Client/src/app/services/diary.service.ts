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
  owner:String;
  diaryNote:String;
  constructor(private http: HttpService,private formBuilder: FormBuilder, private events: EventsService, private toast: ToastsManager) { 
    this.data ={
      isDefaultSort: true,
      startDate: null,
      endDate: null,
      closed:false,
      sort: "InsuranceCarrier",
      sortDirection: "ASC",
      page: 1,
      pageSize: 500
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
  
  search(next:Boolean=false,prev:Boolean=false){
    if (!this.data) {
      this.toast.warning('Please populate at least one search field.');
    } else {
      this.loading = true;
      let data = JSON.parse(JSON.stringify(this.data)); //copy data instead of memory referencing
       
      if(next){
        data.page++;
      }
      if(prev && data.page>1){
        data.page--;
      }
      this.http.diaryList(data).map(res => { return res.json(); })
        .subscribe((result: any) => {
          this.loading = false;
          this.diaries= Immutable.OrderedMap<Number, Diary>(); 
          result.forEach((diary:Diary)=>{
            try{
                this.diaries = this.diaries.set(diary.diaryId,diary);
            }catch(e){}           
          });
          if(next){
            this.data.page++;
          }
          if(prev && this.data.page !=data.page){
            this.data.page--;
          }   
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
  get pages():Array<any>{
    return new Array(this.data.page);
  }
  get pageStart(){
      return this.diaryList.length>1 ? ((this.data.page-1)*this.data.pageSize)+1 : null;
  }
  get pageEnd(){
    return this.diaryList.length>1 ? (this.data.pageSize>this.diaryList.length ? ((this.data.page-1)*this.data.pageSize)+this.diaryList.length : (this.data.page)*this.data.pageSize) : null;
  }

  get end():Boolean{
    return this.pageStart && this.data.pageSize>this.diaryList.length;
  }

}
