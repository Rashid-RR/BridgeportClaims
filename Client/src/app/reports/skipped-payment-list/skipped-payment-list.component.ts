import { Component, OnInit } from '@angular/core';
import { ReportLoaderService, SkippedPaymentService, HttpService } from '../../services/services.barrel';
import { ConfirmComponent } from '../../components/confirm.component';
import { DialogService } from 'ng2-bootstrap-modal';
import { Toast, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-skipped-payment-list',
  templateUrl: './skipped-payment-list.component.html',
  styleUrls: ['./skipped-payment-list.component.css']
})
export class SkippedPaymentListComponent implements OnInit {

  goToPage: any = '';
  activeToast: number;
  constructor(private dialogService: DialogService, private toast: ToastrService, public skipped: SkippedPaymentService, public reportloader: ReportLoaderService) { }

  ngOnInit() {
    this.skipped.getPayors(1);
  }

  archive(prescriptionId: any) {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Archive skipped payment',
      message: `Are you sure you want to archive this entry`
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.skipped.removeSkippedPay(prescriptionId);
        }
      });
  }
  next() {
    this.skipped.fetchSkippedPayReport(true);
  }
  isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
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
  prev() {
    this.skipped.fetchSkippedPayReport(false, true);
  }

  goto() {
    const page = Number.parseInt(this.goToPage);
    if (!this.goToPage || isNaN(page)) {
    } else if (page > 0 && ((this.reportloader.totalPages && page <= this.reportloader.totalPages) || this.reportloader.totalPages == null)) {
      this.skipped.fetchSkippedPayReport(false, false, page);
    } else {
      let toast = this.toast.toasts.find(t=>t.toastId ==this.activeToast)
      if (toast) {
        toast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.reportloader.totalPages;
      } else {
        this.activeToast = this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.reportloader.totalPages).toastId;
      }
    }
  }

  showClaim(claimId: any) {
    window.open('#/main/claims?claimId=' + claimId, '_blank');
  }

}
