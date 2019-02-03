import {Component, OnInit} from '@angular/core';
import {ReferenceManagerService} from '../../services/reference-manager.service';
import {ToastrService} from 'ngx-toastr';

declare var $: any;

@Component({
  selector: 'app-references-grid',
  templateUrl: './references-grid.component.html',
  styleUrls: ['./references-grid.component.css']
})
export class ReferencesGridComponent implements OnInit {
  goToPage: any = '';
  toastId: number;
  toastIsActive = false;
  constructor(public rs: ReferenceManagerService,
              private toast: ToastrService) {
  }

  ngOnInit() {
  }

  goto() {
    const page = Number.parseInt(this.goToPage);
    if (!this.goToPage) {
    } else if (page > 0 && page <= this.rs.getLastPage()) {
      this.rs.currentPage = page;
      this.rs.getReferencesList();
    } else {
      const toast = this.toast.toasts.find(t => t.toastId === this.toastId);
      if (toast) {
        toast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.rs.getLastPage();
      } else {
        this.toastId = this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.rs.getLastPage()).toastId;
      }
    }
  }

  next() {
    this.rs.currentPage = this.rs.currentPage + 1;
    this.rs.getReferencesList();
  }

  prev() {
    this.rs.currentPage = this.rs.currentPage - 1;
    this.rs.getReferencesList();
  }

  keyPress(event: any) {
    // const pattern = /[0-9\+\-\ ]/;
    // const inputChar = String.fromCharCode(event.charCode);
    // const input = Number(this.goToPage + '' + inputChar);
    // if (!pattern.test(inputChar)) {
    //   event.preventDefault();
    // } else if (!this.isNumeric(input)) {
    //   event.preventDefault();
    // } else if (input < 1) {
    //   event.preventDefault();
    // }
  }

  edit(entity: any) {
    this.rs.editFlag = true;
    this.rs.editedEntity = entity;
    this.rs.openModal(true);
  }
}