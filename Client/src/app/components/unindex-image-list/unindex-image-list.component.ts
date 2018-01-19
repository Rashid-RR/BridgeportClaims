import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastsManager, Toast } from 'ng2-toastr/ng2-toastr';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';
// Services
import { DocumentManagerService } from "../../services/document-manager.service";
import { DocumentItem } from 'app/models/document';
import { IShContextMenuItem, BeforeMenuEvent } from 'ng2-right-click-menu/src/sh-context-menu.models';

@Component({
  selector: 'app-unindex-image-list',
  templateUrl: './unindex-image-list.component.html',
  styleUrls: ['./unindex-image-list.component.css']
})
export class UnindexedImageListComponent implements OnInit {

  goToPage: any = '';
  activeToast: Toast;
  items: IShContextMenuItem[];
  constructor(
    public ds: DocumentManagerService,
    private dp: DatePipe,
    private toast: ToastsManager,
    private router: Router,
    private fb: FormBuilder) { }

  ngOnInit() {
    this.items = [
      {
        label: '<span class="fa fa-trash text-red">Archive</span>',
        onClick: ($event)=>{
          this.archive($event.menuItem.id);
        }
      }
    ];
  }

  onBefore(event: BeforeMenuEvent, id) {
    event.open([
      {
        id: id,
        label: '<span class="fa fa-trash text-red">Archive</span>',
        onClick: ($event) => {
          this.archive($event.menuItem.id);
        }
      }
    ]);
  };
  next() {
    this.ds.search(true);
  }
  openFile(file: DocumentItem) {
    this.ds.loading = true;
    this.ds.file = file;
    this.ds.newIndex = true;
    this.ds.loading = false;
  }
  archive(id: number) {
    console.log("Ready to archive document with id " + id);
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
    let input = Number(this.goToPage + "" + inputChar);
    if (!pattern.test(inputChar)) {
      event.preventDefault();
    } else if (!this.isNumeric(input)) {
      event.preventDefault();
    } else if (input < 1) {
      event.preventDefault();
    }
  }
  isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
  }

}
