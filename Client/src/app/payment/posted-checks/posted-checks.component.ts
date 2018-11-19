import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ToastsManager, Toast } from 'ng2-toastr';
import { CurrencyPipe } from '@angular/common';
import { DocumentManagerService } from '../../services/document-manager.service';
import { DocumentItem } from '../../models/document';
import { IShContextMenuItem, BeforeMenuEvent } from 'ng2-right-click-menu/src/sh-context-menu.models';
import { DialogService } from 'ng2-bootstrap-modal/dist/dialog.service';
import { DeleteIndexConfirmationComponent } from '../delete-index-confirmation.component';

@Component({
  selector: 'app-posted-checks',
  templateUrl: './posted-checks.component.html',
  styleUrls: ['./posted-checks.component.css']
})
export class PostedChecksComponent implements OnInit, AfterViewInit {

  goToPage: any = '';
  activeToast: Toast;
  items: IShContextMenuItem[];
  constructor(
    private cp:CurrencyPipe,
    public ds: DocumentManagerService,
    private toast: ToastsManager,
    private dialogService: DialogService) { }

  ngOnInit() {
    this.items = [
      {
        label: '<span class="fa fa-trash text-red">View Details</span>',
        onClick: ($event) => {
          this.view($event.menuItem.id);
        }
      },
      {
        label: '<span class="fa fa-trash text-red">Remove</span>',
        onClick: ($event) => {
          this.remove($event.menuItem.id);
        }
      }
    ];
  }

  onBefore(event: BeforeMenuEvent, id) {
    event.open([
      {
        id: id,
        label: '<span class="text-primary">View Details</span>',
        onClick: ($event) => {
          this.view($event.menuItem.id);
        }
      },
      {
        id: id,
        label: '<span class="fa fa-trash text-red">Remove</span>',
        onClick: ($event) => {
          this.remove($event.menuItem.id);
        }
      }
    ]);
  };
  next() {
    this.ds.searchCheckes(true);
    this.goToPage = '';
  }
  openFile(file: DocumentItem) {
    localStorage.setItem('file-' +file.documentId,JSON.stringify(file));
    window.open(`#/main/indexing/indexed-image/${file.documentId}`);
  }
  view(file:DocumentItem) {
     this.ds.viewPosted(file.documentId);
  }
  remove(file:DocumentItem) {
    console.log(file);

    let amount = this.cp.transform(file['totalAmountPaid'],'USD',true);

    console.log(amount);

    this.dialogService.addDialog(DeleteIndexConfirmationComponent, {
      title: "Delete Indexed Check Confirmation",
      message: `Deleting this check will delete all ${file['numberOfPayments']||''} payment(s) associated with this check totalling ${amount}. Are you sure you wish to un-index this check, and remove ALL payments associated with this check? Or do you wish to un-index this check while keeping all of the existing payments?`
    }).subscribe((isConfirmed) => {
      console.log('isCOnfirmed box')
      console.log(isConfirmed)
        if (isConfirmed) {
          this.ds.deleteAndKeep(file.documentId,isConfirmed);
        }
      });
  }
  goto() {
    let page = Number.parseInt(this.goToPage);
    if (!this.goToPage) {

    } else if (page > 0 && page <= this.ds.checkTotalPages) {
      this.ds.searchCheckes(false, false, page);
    } else {
      if (this.activeToast && this.activeToast.timeoutId) {
        this.activeToast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.ds.checkTotalPages
      } else {
        this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.ds.checkTotalPages).then((toast: Toast) => {
          this.activeToast = toast;
        })
      }
    }
  }
  prev() {
    this.ds.searchCheckes(false, true);
    this.goToPage ='';
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

  ngAfterViewInit() {

  }
}
