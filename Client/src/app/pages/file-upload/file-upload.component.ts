import { Component, OnInit } from '@angular/core';

import { FileSelectDirective, FileDropDirective, FileUploader } from 'ng2-file-upload/ng2-file-upload';


const URL = 'http://bridgeportclaims-bridgeportclaimsstaging.azurewebsites.net/api/fileupload/upload';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css'],

})
export class FileUploadComponent implements OnInit {

  public uploader: FileUploader = new FileUploader({ url: "/api/fileupload/upload" });
  public hasBaseDropZoneOver: boolean = false;
  public hasAnotherDropZoneOver: boolean = false;

  constructor() { }

  ngOnInit() {
  }


  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  public fileOverAnother(e: any): void {
    this.hasAnotherDropZoneOver = e;
  }

}
