import { Injectable } from '@angular/core';
import { HttpService } from './http-service';
import { EventsService } from './events-service';
import { ReportLoaderService } from './report-loader.service';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { FormBuilder,FormControl, FormGroup, Validators} from "@angular/forms";
import { UUID } from 'angular2-uuid';
import * as Immutable from 'immutable';
import { SortColumnInfo } from "../directives/table-sort.directive";
import { Diary } from '../models/diary';
import { Response } from '@angular/http/src/static_response';
import * as FileSaver from 'file-saver'; 

@Injectable()
export class AccountReceivableService {
 
  report: Array<any> = [];
  data:any={};
  columns:Array<String>=[];
  totalRowCount:number;
  autoCompleteGroupName:string;
  autoCompletePharmacyName:string;
  groupName:any;
  groupNameParameter:any;
  pharmacyName:any;
  pharmacyNameParameter:any;
  public filteredList: any[] = [];
  public pharmacyList: any[] = [];

  constructor(private http: HttpService,private events: EventsService, private toast: ToastsManager,public reportLoader:ReportLoaderService,
    ) { 
    this.autoCompleteGroupName = this.http.baseUrl + "/reports/group-name/?groupName=:keyword";    
    this.autoCompletePharmacyName = this.http.baseUrl + "/reports/pharmacy-name/?pharmacyName=:keyword";    
    
  }   
  runReport(){
    //this.toast.info('Hold tight... this will take several seconds...');
    if(this.groupName && this.filteredList.length==0){
      this.toast.warning('Please clear the Group Name field or Search for a Group Name and pick from the drop down list');
    }else if(this.pharmacyName && this.pharmacyList.length==0){
      this.toast.warning('Please clear the Pharmacy Name field or Search for a Pharmacy Name and pick from the drop down list');
    }else{
      let item = this.filteredList.find(l=>l.groupName==this.groupName);
      let ph = this.pharmacyList.find(l=>l.pharmacyName==this.pharmacyName);
       if(item){
        this.groupNameParameter = this.groupName.groupName ?  this.groupName.groupName :  this.groupName;
      }else{
        this.groupNameParameter = undefined;
      }
      if(ph){
        this.pharmacyNameParameter = this.pharmacyName.pharmacyName ? this.pharmacyName.pharmacyName : this.pharmacyName;
      }else{
        this.pharmacyNameParameter = undefined;
      }
      if(this.groupName && this.filteredList.length>0 && !item){
        this.toast.warning('Please clear the Group Name field or Search for a Group Name and pick from the drop down list');
      }else if(this.pharmacyName && this.pharmacyList.length>0 && !ph){
        this.toast.warning('Please clear the Pharmacy Name field or Search for a Pharmacy Name and pick from the drop down list');
      }else {
        this.search();      
      }
    }
  }
  export(){
    //this.toast.info('Hold tight... your report and Excel are generating....');
    this.search();
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
    let str = string.slice(1).replace(/[0-9_]/g, '');
    let num = string.slice(1).replace(/[A-Za-z]/g, '');
    return string.charAt(0).toUpperCase() + str+' '+num;
  }
  getExport(data:any){
    this.reportLoader.loading = true;
    this.http.getExport(data).map(res => { return res.json(); })
    .subscribe((result: Array<any>) => {
      this.reportLoader.loading = false; 
    }, error => {
      this.reportLoader.loading = false; 
      this.events.broadcast('account-receivable-report-updated');
    });    
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
      if(this.groupNameParameter){
        data.groupName=this.groupNameParameter;
      } 
      if(this.pharmacyNameParameter){
        data.pharmacyName=this.pharmacyNameParameter;
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
  exportFile(next:Boolean=false,prev:Boolean=false,page:number = undefined){     
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
      if(this.groupNameParameter){
        data.groupName=this.groupNameParameter;
      } 
      if(this.pharmacyNameParameter){
        data.pharmacyName=this.pharmacyNameParameter;
      }
      this.http.getExport(data)
        /* .map(response => {
          let fileBlob = response.blob();
            let blob = new Blob([fileBlob], { 
              type: response.headers.get('content-type') // must match the Accept type
            });
            return fileBlob;
        }) */
        .subscribe((result) => {
          this.reportLoader.loading = false;   
          this.downloadFile(result);
          //console.log(result);
        }, err => {
          this.reportLoader.loading = false;
          try {
            const error = err.json(); 
          } catch (e) { }
        }); 
  }
  downloadFile(data: Response){
    let blob = new Blob([data.blob()], { 
      type:data.headers.get('content-type')// 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' // must match the Accept type
   }); 
   let filename = data.headers.get('content-disposition').replace('attachment; filename=','');
   filename = filename.replace(/"/g,'');
   FileSaver.saveAs(blob, filename);
  }

}
