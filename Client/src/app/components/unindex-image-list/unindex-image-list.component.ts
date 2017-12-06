import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastsManager, Toast } from 'ng2-toastr/ng2-toastr';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
// Services
import { DocumentManagerService } from "../../services/document-manager.service";
import { DocumentItem } from 'app/models/document';
@Component({
  selector: 'app-unindex-image-list',
  templateUrl: './unindex-image-list.component.html',
  styleUrls: ['./unindex-image-list.component.css']
})
export class UnindexedImageListComponent implements OnInit {

  goToPage: any;
  activeToast: Toast;
  constructor(
    public ds: DocumentManagerService,
    private dp: DatePipe,
    private toast: ToastsManager,
    private router:Router,
    private fb: FormBuilder) { }

  ngOnInit() {

  }
  next() {
    this.ds.search(true);
  }
  openFile(file: DocumentItem) {
    this.ds.loading=true;
    localStorage.setItem("file-"+file.documentId,JSON.stringify(file));
    window.open("#/main/unindexed-images/file?id="+file.documentId, "_blank");    
    this.router.navigate(["/main/unindexed-images/new-index"],{queryParams:{id:file.documentId}});
  }
  goto() {
    let page = Number.parseInt(this.goToPage);
    if (!this.goToPage) {

    } else if (page > 0 && page <= this.ds.totalPages) {
      this.ds.search(false, false, page);
    } else {
      if (this.activeToast && this.activeToast.timeoutId) {
        this.activeToast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.ds.totalPages
      } else {
        this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.ds.totalPages).then((toast: Toast) => {
          this.activeToast = toast;
        })
      }
    }
  }
  prev() {
    this.ds.search(false, true);
  }
  keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;
    let inputChar = String.fromCharCode(event.charCode);
    if ((event.keyCode != 8 && !pattern.test(inputChar)) || (Number(inputChar) > this.ds.totalPages || Number(inputChar) < 1)) {
      event.preventDefault();
    }
  }

}
