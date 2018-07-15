import { Component, OnInit, AfterViewChecked } from '@angular/core';

import { FileSelectDirective, FileItem, FileDropDirective, ParsedResponseHeaders, FileUploader } from 'ng2-file-upload/ng2-file-upload';
import { HttpService } from "../../services/http-service"
import { ImportFile } from "../../models/import-file"
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DialogService } from "ng2-bootstrap-modal";
import { ConfirmComponent } from '../../components/confirm.component';

const URL = 'http://bridgeportclaims-bridgeportclaimsstaging.azurewebsites.net/api/fileupload/upload';
const noLaker: String = "No Laker Files were found to import.";

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css'],

})
export class FileUploadComponent implements OnInit, AfterViewChecked {

  public uploaderCsv: FileUploader = new FileUploader({});
  public uploaderExcel: FileUploader = new FileUploader({});
  public uploaderMisc: FileUploader = new FileUploader({});

  public hasBaseDropZoneOver = false;
  public hasAnotherDropZoneOver = false;
  public loading = false;

  importedFiles: Array<ImportFile> = [];

  get queueFiles() {
    return this.uploaderCsv.queue.concat(this.uploaderExcel.queue).concat(this.uploaderMisc.queue);
  }

  constructor(
    private http: HttpService,
    private dialogService: DialogService,
    private toast: ToastsManager
  ) {
    const headers = [{
      name: 'Authorization',
      value: 'Bearer ' + this.http.token
    }];

    this.uploaderCsv = new FileUploader({ url: '/api/fileupload/upload', headers: headers });
    this.uploaderCsv.onCompleteItem = this.onItemUploadComplete.bind(this);

    this.uploaderExcel = new FileUploader({ url: '/api/fileupload/upload', headers: headers, additionalParameter: [{ test: 'This one is excel' }] });
    this.uploaderExcel.onCompleteItem = this.onItemUploadComplete.bind(this);

    this.uploaderMisc = new FileUploader({ url: '/api/fileupload/upload', headers: headers, additionalParameter: [{ test: 'This one is Misc' }] });
    this.uploaderMisc.onCompleteItem = this.onItemUploadComplete.bind(this);
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

  getFiles() {
    this.loading = true;
    this.http.getFiles().single().subscribe(res => {
      //res.push(new ImportFile(new Date(),".png",231,"assets/that-file.png"));     
      this.importedFiles = res;
      this.loading = false;
      //console.log(this.importedFiles)
    }, error => {
      this.loading = false;
    });
  }

  deleteFile(file: ImportFile) {
    let disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: "Delete File",
      message: "Do you want to delete " + file.fileName + "?"
    })
      .subscribe((isConfirmed) => {
        //We get dialog result
        if (isConfirmed) {
          this.loading = true;
          this.http.deleteFileById(file.importFileId).single().subscribe(res => {
            this.toast.success(res.message);
            this.loading = false;
            this.getFiles();
          }, error => {
            this.loading = false;
          });
        } else {
        }
      });
  }

  importLaker(file: ImportFile) {
    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: 'Process Laker File',
      message: 'You are about to import the Laker File ' + file.fileName + ' into the database. Would you like to proceed?'
    }).subscribe((isConfirmed) => {
      // We get dialog result
      if (isConfirmed) {
        this.loading = true;
        this.http.importLakerFile(file.fileName).single().subscribe(res => {
          if (res.message == noLaker) {
            this.toast.info(res.message);
          } else {
            this.toast.info(res.message, null, { toastLife: 10000 });
          }
          this.loading = false;
          this.getFiles();
        }, error => {
          this.toast.info(error.message);
          this.loading = false;
        });
      } else {
      }
    });
  }

  process(file: ImportFile) {
    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: 'Process Payment',
      message: 'You are about to import the Payment File ' + file.fileName + ' into the Payment table ' +
      'in the database. Would you like to proceed?'
    })
      .subscribe((isConfirmed) => {
        // We get dialog result
        if (isConfirmed) {
          this.loading = true;
          this.http.importFile(file.fileName).single().subscribe(res => {
            this.toast.success(res.message);
            this.loading = false;
            this.getFiles();
          }, error => {
            this.loading = false;
          });
        } else {
        }
      });
  }

  ngAfterViewChecked() {
    this.updateTableHeadingWidth();
    const fixedHeader = document.getElementById('fixed-header');
    if (fixedHeader.style.position !== 'fixed') {
      fixedHeader.style.position = 'fixed';
      // console.log('set fixed header to Fixed');
    }
  }

  cloneTableHeading() {
    const cln = document.getElementById('fixed-thead').cloneNode(true);
    const fixedHeader = document.getElementById('fixed-header');
    fixedHeader.appendChild(cln);
    this.updateTableHeadingWidth();
  }

  cloneBoxHeader() {
    const cln = document.getElementById;
  }


  updateTableHeadingWidth() {
    setTimeout(() => {
      const fixedHeader = document.getElementById('fixed-header');
      const fixedMaxHeader = document.getElementById('fixed-max-header');
      const mainTable = document.getElementById('maintable');
      if (fixedHeader) {
        if (mainTable) {
          const tableWidth = mainTable.clientWidth.toString();
          fixedHeader.style.width = tableWidth + 'px';
        }
      } else {
        if (mainTable) {
          const tableWidth = mainTable.clientWidth.toString();
          // tslint:disable-next-line:one-line
          try {
            fixedMaxHeader.style.width = tableWidth + 'px';
          } catch (e) { }
        }
      }
    }, 500);
  }
}
