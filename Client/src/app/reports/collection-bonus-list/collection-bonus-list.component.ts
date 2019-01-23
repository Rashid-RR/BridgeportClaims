import { Component, OnInit } from '@angular/core';
import { ReportLoaderService, ProfileManager, CollectionBonusService } from '../../services/services.barrel';
import { Toast, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-collection-bonus-list',
  templateUrl: './collection-bonus-list.component.html',
  styleUrls: ['./collection-bonus-list.component.css']
})
export class CollectionBonusListComponent implements OnInit {
  goToPage: any = '';
  activeToast: number;
  constructor(private toast: ToastrService, public bonusService: CollectionBonusService,
    public reportloader: ReportLoaderService, private profileManager: ProfileManager) { }

  ngOnInit() {
    this.bonusService.fetchBonusReport();
  }

  get userName() {
    return this.profileManager.profile ? this.profileManager.profile.firstName + ' ' + this.profileManager.profile.lastName : '';
  }
  next() {
    this.bonusService.fetchBonusReport(true);
  }
  prev() {
    this.bonusService.fetchBonusReport(false, true);
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
  showClaim(claimId: any) {
    window.open('#/main/claims?claimId=' + claimId, '_blank');
  }
  goto() {
    const page = Number.parseInt(this.goToPage);
    if (!this.goToPage || isNaN(page)) {
    } else if (page > 0 && ((this.reportloader.totalPages && page <= this.reportloader.totalPages) || this.reportloader.totalPages == null)) {
      this.bonusService.fetchBonusReport(false, false, page);
    } else {
      let toast = this.toast.toasts.find(t=>t.toastId ==this.activeToast)
      if (toast) {
        toast.message= 'Page number entered is out of range. Enter a page number between 1 and ' + this.reportloader.totalPages;
      } else {
        this.activeToast = this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.reportloader.totalPages).toastId;
      }
    }
  }

}
