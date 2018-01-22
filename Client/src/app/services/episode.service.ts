import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { EventsService } from './events-service';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import {FormBuilder,FormControl, FormGroup, Validators} from "@angular/forms";
import { UUID } from 'angular2-uuid';
import * as Immutable from 'immutable';
import { SortColumnInfo } from "../directives/table-sort.directive";
import { Episode } from '../models/episode';
import {DiariesFilterPipe} from "../components/diary-results/diary-filter.pipe"
import { Diary } from 'app/models/diary';
import { EpisodeNoteType } from 'app/models/episode-note-type';

@Injectable()
export class EpisodeService {

  loading: Boolean = false; 
  episodes: Immutable.OrderedMap<Number, Episode> = Immutable.OrderedMap<Number, Episode>();
  data:any={};
  owner:String;
  diaryNote:String;
  totalRowCount:number;
  episodeNoteTypes: Array<EpisodeNoteType> = []
  constructor(private http: HttpService,private formBuilder: FormBuilder,
      private dp:DiariesFilterPipe,
     private events: EventsService, private toast: ToastsManager) { 
    this.data ={
      isDefaultSort: true,
      startDate: null,
      endDate: null,
      closed:false,
      sort: "InsuranceCarrier",
      sortDirection: "ASC",
      page: 1,
      pageSize: 30
    };
    this.http.getEpisodesNoteTypes().map(res => { return res.json() })
    .subscribe((result: Array<any>) => {
      // console.log("Episode Notes", result)
      this.episodeNoteTypes = result;
    }, err => {
      this.loading = false;
      console.log(err);
      let error = err.json();
    });
  }

  refresh() {

  }
  get totalPages(){
    return this.totalRowCount ? Math.ceil(this.totalRowCount/this.data.pageSize): null;
  }
  /* get totalPages(){
    return this.totalRowCount ? Math.ceil(this.totalRowCount/this.data.pageSize): null;
  } */
  get episodeList():Array<Episode>{
    return this.episodes.toArray();
  }
  get episodeFilterSize():Array<any>{
    return  this.dp.transform(this.episodes.toArray() as any, this.diaryNote,this.owner);
  }
  onSortColumn(info: SortColumnInfo) {
    this.data.isDefaultSort = false;
    this.data.sort = info.column;
    this.data.sortDirection = info.dir;
    this.search();
  }
  
  search(next:Boolean=false,prev:Boolean=false,page:number = undefined){
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
      if(page){
        data.page=page;
      } 
      this.http.diaryList(data).map(res => { return res.json(); })
        .subscribe((result: any) => {
          this.loading = false;
          this.totalRowCount = result.totalRowCount;
          this.episodes= Immutable.OrderedMap<Number, Episode>(); 
          result.diaryResults.forEach((episode:Episode)=>{
            try{
                this.episodes = this.episodes.set(episode['diaryId'],episode);
            }catch(e){}           
          });
          if(next){
            this.data.page++;
          }
          if(prev && this.data.page !=data.page){
            this.data.page--;
          }      
          if(page){
            this.data.page=page;
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
          this.events.broadcast('episode-list-updated');
        });
    }
  }
  get pages():Array<any>{
    return new Array(this.data.page);
  }
  get pageStart(){
      return this.episodeList.length>1 ? ((this.data.page-1)*this.data.pageSize)+1 : null;
  }
  get pageEnd(){
    return this.episodeList.length>1 ? (this.data.pageSize>this.episodeList.length ? ((this.data.page-1)*this.data.pageSize)+this.episodeList.length : (this.data.page)*this.data.pageSize) : null;
  }

  get end():Boolean{
    return this.pageStart && this.data.pageSize>this.episodeList.length;
  }

}
