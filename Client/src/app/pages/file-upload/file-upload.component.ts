import { Component, OnInit,AfterViewChecked } from '@angular/core';

import { FileSelectDirective,FileItem, FileDropDirective,ParsedResponseHeaders, FileUploader } from 'ng2-file-upload/ng2-file-upload';
import {HttpService} from "../../services/http-service"
import {ImportFile} from "../../models/import-file"
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DialogService } from "ng2-bootstrap-modal";
import { ConfirmComponent } from '../../components/confirm.component';

const URL = 'http://bridgeportclaims-bridgeportclaimsstaging.azurewebsites.net/api/fileupload/upload';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css'],

})
export class FileUploadComponent implements OnInit,AfterViewChecked {

  public uploaderCsv: FileUploader;
  public uploaderExcel: FileUploader;
  public hasBaseDropZoneOver = false;
  public hasAnotherDropZoneOver = false;
  public loading = false;
  importedFiles:Array<ImportFile>=[];
  get queueFiles() {
    return this.uploaderCsv.queue.concat(this.uploaderExcel.queue);
  }
  constructor(private http: HttpService,private dialogService: DialogService,private toast: ToastsManager) {
     const headers = [{
                name: 'Authorization',
                value: 'Bearer ' + this.http.token
          }];
      this.uploaderCsv =  new FileUploader({ url: '/api/fileupload/upload', headers: headers });
      this.uploaderCsv.onCompleteItem = this.onItemUploadComplete.bind(this);

      this.uploaderExcel =  new FileUploader({ url: '/api/fileupload/upload', headers: headers });
      this.uploaderExcel.onCompleteItem = this.onItemUploadComplete.bind(this);
  }
  onItemUploadComplete(item: FileItem, response: string, status: number, headers: ParsedResponseHeaders) {
        const r = JSON.parse(response);
        if (status => 200 && status < 300) {
          this.getFiles();
          this.toast.success(r.message);
        } else {
          this.toast.error(r.message);
        }
      }

  ngOnInit() {
    this.getFiles();
  }
  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public fileOverAnother(e: any): void {
    this.hasAnotherDropZoneOver = e;
  }

  getFiles(){
    this.loading = true;
    this.http.getFiles().single().map(r=>{return r.json()}).subscribe(res=>{ 
      //res.push(new ImportFile(new Date(),".png",231,"assets/that-file.png"));     
      this.importedFiles = res;
      this.loading = false;
      //console.log(this.importedFiles)
    },error=>{
      this.loading = false;
    });
  }
  deleteFile(file:ImportFile){
    let disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: "Delete File",
      message: "Do you want to delete "+file.fileName+"?"
    })
      .subscribe((isConfirmed) => {
        //We get dialog result
        if (isConfirmed) {  
          this.loading = true;          
          this.http.deleteFileById(file.importFileId).single().map(r=>{return r.json()}).subscribe(res=>{      
            this.toast.success(res.message);  
            this.loading = false;          
            this.getFiles();
          },error=>{
            this.loading = false;
          });
        }
        else {
           
        }
      });
  }
  process(file:ImportFile){
    let disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: "Process Payment",
      message: "You are about to import the Payment File "+file.fileName+" into the Payment table in the database. Would you like to proceed?"
    })
      .subscribe((isConfirmed) => {
        //We get dialog result
        if (isConfirmed) {
          this.loading = true;        
          this.http.importFile(file.fileName).single().map(r=>{return r.json()}).subscribe(res=>{      
            this.toast.success(res.message);
            this.loading = false;
            this.getFiles();
          },error=>{
            this.loading = false;
          });
        }
        else {
           
        }
      });
  }
  ngAfterViewChecked() {
    this.updateTableHeadingWidth();
     let fixedHeader = document.getElementById('fixed-header');
      if (fixedHeader.style.position !== 'fixed') {
        fixedHeader.style.position = 'fixed';
        // console.log('set fixed header to Fixed');
      }    
  }

  cloneTableHeading() {
    let cln = document.getElementById('fixed-thead').cloneNode(true);
    let fixedHeader = document.getElementById('fixed-header');
    fixedHeader.appendChild(cln);
    this.updateTableHeadingWidth();
  }

  cloneBoxHeader() {
    let cln = document.getElementById
  }


  updateTableHeadingWidth() {
    setTimeout(() => {
      let fixedHeader = document.getElementById('fixed-header');
      let fixedMaxHeader = document.getElementById('fixed-max-header');
      let mainTable = document.getElementById('maintable');
      if (fixedHeader) {
        if (mainTable) {
          let tableWidth = mainTable.clientWidth.toString();
          fixedHeader.style.width = tableWidth + 'px';
        }
      } else {
        if (mainTable) {
          let tableWidth = mainTable.clientWidth.toString();
          try{
            fixedMaxHeader.style.width = tableWidth + 'px';
          }catch(e){}
        }
      }
    }, 500)
  }
}
