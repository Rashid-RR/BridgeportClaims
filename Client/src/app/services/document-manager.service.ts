import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { EventsService } from './events-service';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import {FormBuilder,FormControl, FormGroup, Validators} from "@angular/forms";
import { UUID } from 'angular2-uuid';
import * as Immutable from 'immutable';
import { SortColumnInfo } from "../directives/table-sort.directive";
import { DocumentItem } from '../models/document';
 
@Injectable()
export class DocumentManagerService {
    loading: Boolean = false; 
    documents: Immutable.OrderedMap<any, DocumentItem> = Immutable.OrderedMap<any, DocumentItem>();
    data:any={}; 
    display:string='list';
    totalRowCount:number;
    constructor(private http: HttpService,private formBuilder: FormBuilder, 
        private events: EventsService, private toast: ToastsManager) { 
        this.data ={
            date: null,
            sort: "DocumentID",
            sortDirection: "ASC",
            page: 1,
            pageSize: 500
        };
        this.search();
    }
    get pages():Array<any>{
      return new Array(this.data.page);
    }
    get documentList():Array<DocumentItem>{
      return this.documents.toArray();
    }
    get pageStart(){
        return this.documentList.length>1 ? ((this.data.page-1)*this.data.pageSize)+1 : null;
    }
    get pageEnd(){
      return this.documentList.length>1 ? (this.data.pageSize>this.documentList.length ? ((this.data.page-1)*this.data.pageSize)+this.documentList.length : (this.data.page)*this.data.pageSize) : null;
    }
    get totalPages(){
      return this.totalRowCount ? Math.ceil(this.totalRowCount/this.data.pageSize): 0;
    }
  
    get end():Boolean{
      console.log(this.documentList);
      return this.pageStart && this.data.pageSize>this.documentList.length;
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
          this.http.getDocuments(data).map(res => { return res.json(); })
            .subscribe((result: any) => {
                console.log(result);
              this.loading = false;
              this.totalRowCount = result.totalRowCount;
              this.documents= Immutable.OrderedMap<any, DocumentItem>(); 
              result.documentResults.forEach((doc:DocumentItem)=>{
                try{
                    this.documents = this.documents.set(doc.documentId,doc);
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
            }, err => {
              this.loading = false;
              try {
                const error = err.json(); 
              } catch (e) { }
            }, () => {
              this.events.broadcast('document-list-updated');
            });
        }
      }
  

}
