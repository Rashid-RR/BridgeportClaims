import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService, Toast } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { Router } from '@angular/router';

import { ConfirmComponent } from '../../components/confirm.component';
// Services
import { DocumentManagerService } from '../../services/document-manager.service';
import { DocumentItem } from '../../models/document';
import { IShContextMenuItem, BeforeMenuEvent } from 'ng2-right-click-menu/src/sh-context-menu.models';
import { DialogService } from 'ng2-bootstrap-modal/dist/dialog.service';

@Component({
  selector: 'payment-unindexed-check-list',
  templateUrl: './unindexed-check-list.component.html',
  styleUrls: ['./unindexed-check-list.component.css']
})
export class PaymentCheckListComponent implements OnInit, AfterViewInit {

  goToPage: any = '';
  activeToast: number;
  items: IShContextMenuItem[];
  constructor(
    public ds: DocumentManagerService,
    private toast: ToastrService,
    private router: Router,
    private dialogService: DialogService) { }

  ngOnInit() {
    this.items = [
      {
        label: '<span class="fa fa-trash text-red">Archive</span>',
        onClick: ($event) => {
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
    this.ds.searchCheckes(true);
    this.goToPage = '';
  }
  openFile(file: DocumentItem) {
    this.ds.checksFile = file;
    this.ds.newCheck = true;
    localStorage.setItem('file-' + this.ds.checksFile.documentId, JSON.stringify(this.ds.checksFile));
    this.router.navigate([`/main/payments/${this.ds.checksFile.documentId}`]);
  }
  archive(file: DocumentItem) {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Archive Check',
      message: 'Are you sure you wish to archive ' + file.fileName + '?'
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.ds.archive(file.documentId, false, 'searchCheckes');
        }
      });
  }
  goto() {
    const page = Number.parseInt(this.goToPage);
    if (!this.goToPage) {

    } else if (page > 0 && page <= this.ds.checkTotalPages) {
      this.ds.searchCheckes(false, false, page);
    } else {
      let toast = this.toast.toasts.find(t=>t.toastId ==this.activeToast)
      if (toast) {
        toast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.ds.checkTotalPages;
      } else {
       this.activeToast =  this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.ds.checkTotalPages).toastId;
      }
    }
  }
  prev() {
    this.ds.searchCheckes(false, true);
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
