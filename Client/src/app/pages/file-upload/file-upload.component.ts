import { Component, OnInit } from '@angular/core';

import { FileSelectDirective,FileItem, FileDropDirective,ParsedResponseHeaders, FileUploader } from 'ng2-file-upload/ng2-file-upload';
import {HttpService} from "../../services/http-service"
import {ImportFile} from "../../models/import-file"
import { ToastsManager } from 'ng2-toastr/ng2-toastr';


const URL = 'http://bridgeportclaims-bridgeportclaimsstaging.azurewebsites.net/api/fileupload/upload';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css'],

})
export class FileUploadComponent implements OnInit {

  public uploaderCsv: FileUploader;
  public uploaderExcel: FileUploader;
  public hasBaseDropZoneOver = false;
  public hasAnotherDropZoneOver = false;
  importedFiles:Array<ImportFile>=[];
  get queueFiles() {
    return this.uploaderCsv.queue.concat(this.uploaderExcel.queue);
  }
  constructor(private http: HttpService, private toast: ToastsManager) {
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
    this.http.getFiles().single().map(r=>{return r.json()}).subscribe(res=>{      
      this.importedFiles = res;
      console.log(this.importedFiles)
    },error=>{});
  }
  deleteFile(id){
    this.http.deleteFileById(id).single().map(r=>{return r.json()}).subscribe(res=>{      
      console.log(res);
      this.getFiles();
    },error=>{});
  }
}
