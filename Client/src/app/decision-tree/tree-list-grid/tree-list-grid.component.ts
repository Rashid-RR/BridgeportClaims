import { Component, OnInit } from '@angular/core';
import { ToastsManager, Toast } from 'ng2-toastr';
import { Router } from '@angular/router';

// Services
import { IShContextMenuItem, BeforeMenuEvent } from 'ng2-right-click-menu/src/sh-context-menu.models';
import { DialogService } from 'ng2-bootstrap-modal/dist/dialog.service';
import { ITreeNode } from '../tree-node';
import { DecisionTreeService } from 'app/services/decision-tree.service';

@Component({
  selector: 'tree-list-grid',
  templateUrl: './tree-list-grid.component.html',
  styleUrls: ['./tree-list-grid.component.css']
})
export class TreeListGridComponent implements OnInit {

  goToPage: any = '';
  activeToast: Toast;
  items: IShContextMenuItem[];
  constructor(
    public ds: DecisionTreeService, 
    private toast: ToastsManager,
    private router: Router) { }

  ngOnInit() {
    this.items = [
      {
        label: '<span class="fa fa-eye text-success">View</span>',
        onClick: ($event) => {
          this.viewTree($event.menuItem.id);
        }
      }
    ];
  }

  onBefore(event: BeforeMenuEvent, id) {
    event.open([
      {
        id: id,
        label: '<span class="fa fa-eye text-success">View</span>',
        onClick: ($event) => {
          this.viewTree($event.menuItem.id);
        }
      }
    ]);
  };
  next() {
    this.ds.search(true);
    this.goToPage = '';
  }
  viewTree(t: ITreeNode) {
     this.router.navigate(['/main/decision-tree/construct',t.treeId])
  }
   
  goto() {
    const page = Number.parseInt(this.goToPage);
    if (!this.goToPage) {

    } else if (page > 0 && page <= this.ds.totalPages) {
      this.ds.search(false, false, page);
    } else {
      if (this.activeToast && this.activeToast.timeoutId) {
        this.activeToast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.ds.totalPages;
      } else {
        this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.ds.totalPages).then((toast: Toast) => {
          this.activeToast = toast;
        });
      }
    }
  }
  prev() {
    this.ds.search(false, true);
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

}
