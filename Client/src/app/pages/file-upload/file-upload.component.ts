import { AfterViewChecked, Component, OnDestroy, OnInit } from '@angular/core';
import { DialogService } from 'ng2-bootstrap-modal';
import { FileItem, FileUploader, ParsedResponseHeaders } from 'ng2-file-upload/ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { ConfirmComponent } from '../../components/confirm.component';
import { ImportFile } from '../../models/import-file';
import { HttpService } from '../../services/http-service';

const noLaker: String = 'No Laker Files were found to import.';
const noEnvision: String = 'No Envision File was found to process.';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css'],

})
export class FileUploadComponent implements OnInit, AfterViewChecked, OnDestroy {
  private sub!: Subscription;
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
    private toast: ToastrService
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
    if (status => {
      return 200 && status < 300;
    }) {
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
    this.http.getFiles().subscribe(res => {
      this.importedFiles = res;
      this.loading = false;
    }, error => {
      this.loading = false;
    });
  }

  deleteFile(file: ImportFile) {
    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: 'Delete File',
      message: 'Do you want to delete ' + file.fileName + '?'
    })
      .subscribe((isConfirmed) => {
        // We get dialog result
        if (isConfirmed) {
          this.loading = true;
          this.http.deleteFileById(file.importFileId).subscribe(res => {
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
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Process Laker File',
      message: 'You are about to import the Laker File ' + file.fileName + ' into the database. Would you like to proceed?'
    }).subscribe((isConfirmed) => {
      if (isConfirmed) {
        this.loading = true;
        this.http.importLakerFile(file.fileName).subscribe(res => {
          if (res.message === noLaker) {
            this.toast.info(res.message);
          } else {
            this.toast.info(res.message, null, { timeOut: 10000 });
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

  importEnvision(file: ImportFile) {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Process Envision File',
      message: 'You are about to import the Envision File ' + file.fileName + ' into the database. Would you like to proceed?'
    }).subscribe((isConfirmed) => {
      // We get dialog result
      if (isConfirmed) {
        this.loading = true;
        this.sub = this.http.importEnvision(file.importFileId).subscribe(res => {
          if (res.message === noEnvision) {
            this.toast.info(res.message);
          } else {
            this.toast.info(res.message, null, { timeOut: 10000 });
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
          this.http.importFile(file.fileName).subscribe(res => {
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

  ngOnDestroy(): void {
    if (this.sub) {
      this.sub.unsubscribe();
    }
  }
}
