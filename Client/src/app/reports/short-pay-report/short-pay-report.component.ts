import { Component, OnInit } from '@angular/core';
import { ReportLoaderService, ShortPayService } from '../../services/services.barrel';
import { ConfirmComponent } from '../../components/confirm.component';
import { DialogService } from 'ng2-bootstrap-modal';
import { Toast, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-short-pay-report',
  templateUrl: './short-pay-report.component.html',
  styleUrls: ['./short-pay-report.component.css']
})
export class ShortPayReportComponent implements OnInit {

  goToPage: any = '';
  activeToast: number;
  constructor(private dialogService: DialogService, private toast: ToastrService, public shortpay: ShortPayService,
    public reportloader: ReportLoaderService) { }

  ngOnInit() {
    this.shortpay.fetchShortpayReport();
  }
  remove(prescriptionId: any) {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Remove shortpay',
      message: `Are you sure you want to remove this entry?`
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.shortpay.removeShortpay(prescriptionId);
        }
      });
  }

  next() {
    this.shortpay.fetchShortpayReport(true);
  }
  prev() {
    this.shortpay.fetchShortpayReport(false, true);
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
      this.shortpay.fetchShortpayReport(false, false, page);
    } else {
      let toast = this.toast.toasts.find(t=>t.toastId ==this.activeToast)
      if (toast) {
        toast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.reportloader.totalPages;
      } else {
        this.activeToast = this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.reportloader.totalPages).toastId
      }
    }
  }

}
