import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ToastrService, Toast } from 'ngx-toastr';

import { ConfirmComponent } from '../../components/confirm.component';
import { DocumentManagerService } from '../../services/document-manager.service';
import { DocumentItem } from '../../models/document';
import { IShContextMenuItem, BeforeMenuEvent } from 'ng2-right-click-menu/src/sh-context-menu.models';
import { DialogService } from 'ng2-bootstrap-modal/dist/dialog.service';

@Component({
  selector: 'indexing-unindexed-invalid-check-list',
  templateUrl: './unindexed-invalid-check-list.component.html',
  styleUrls: ['./unindexed-invalid-check-list.component.css']
})
export class UnindexedInvalidCheckListComponent implements OnInit, AfterViewInit {

  goToPage: any = '';
  activeToast: number;
  items: IShContextMenuItem[];
  constructor(
    public ds: DocumentManagerService,
    private toast: ToastrService,
    private dialogService: DialogService) { }

  ngOnInit() {
    this.items = [
      /* {
        label: '<span class="fa fa-trash text-red">Archive</span>',
        onClick: ($event)=>{
          this.archive($event.menuItem.id);
        }
      } */
    ];
  }

  onBefore(event: BeforeMenuEvent, id) {
    /* event.open([
      {
        id: id,
        label: '<span class="fa fa-trash text-red">Archive</span>',
        onClick: ($event) => {
          this.archive($event.menuItem.id);
        }
      }
    ]); */
  };
  next() {
    this.ds.searchInvalidCheckes(true);
    this.goToPage = '';
  }
  openFile(file: DocumentItem) {
    this.ds.loading = true;
    this.ds.checksFile = file;
    this.ds.newCheck = true;
    this.ds.loading = false;
    this.ds.indexNewCheck = false;
  }
  archive(file: DocumentItem) {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Archive Check',
      message: 'Are you sure you wish to archive ' + file.fileName + '?'
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.ds.archive(file.documentId, false, 'searchInvalidCheckes');
        }
      });
  }
  goto() {
    const page = Number.parseInt(this.goToPage);
    if (!this.goToPage) {

    } else if (page > 0 && page <= this.ds.invalidCheckTotalPages) {
      this.ds.searchInvalidCheckes(false, false, page);
    } else {
      let toast = this.toast.toasts.find(t=>t.toastId ==this.activeToast)
      if (toast) {
        toast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.ds.invalidCheckTotalPages;
      } else {
        this.activeToast = this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.ds.invalidCheckTotalPages).toastId
      }
    }
  }
  prev() {
    this.ds.searchInvalidCheckes(false, true);
    this.goToPage = '';
  }
  keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;
    const inputChar = String.fromCharCode(event.charCode);
    const input = Number(this.goToPage + '' + inputChar);
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

  ngAfterViewInit() {

  }


}
