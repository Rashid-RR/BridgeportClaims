import { Component, OnInit, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
// Services
import { IShContextMenuItem, BeforeMenuEvent } from 'ng2-right-click-menu/src/sh-context-menu.models';
import { ITreeNode } from '../tree-node';
import { DecisionTreeService } from '../../services/decision-tree.service';
import { ProfileManager } from '../../services/profile-manager';

@Component({
  selector: 'tree-list-grid',
  templateUrl: './tree-list-grid.component.html',
  styleUrls: ['./tree-list-grid.component.css']
})
export class TreeListGridComponent implements OnInit {

  goToPage: any = '';
  activeToast: number;
  items: IShContextMenuItem[];
  @Input() claimId: string;
  constructor(
    private profileManager: ProfileManager,
    public ds: DecisionTreeService,
    private toast: ToastrService,
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
  }
  next() {
    this.ds.search(true);
    this.goToPage = '';
  }
  viewTree(t: ITreeNode) {
    console.log(this.claimId);
     this.router.navigate([`/main/decision-tree/${this.claimId ? 'experience':'construct'}`, t.treeId, this.claimId || '']);
  }

  goto() {
    const page = Number.parseInt(this.goToPage, 10);
    if (!this.goToPage) {

    } else if (page > 0 && page <= this.ds.totalPages) {
      this.ds.search(false, false, page);
    } else {
      const toast = this.toast.toasts.find(t => t.toastId == this.activeToast);
      if (toast) {
        toast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.ds.totalPages;
      } else {
        this.activeToast = this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.ds.totalPages).toastId;
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
  get allowed(): Boolean {
    return  (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array) && this.profileManager.profile.roles.indexOf('Admin') > -1);
  }

}
