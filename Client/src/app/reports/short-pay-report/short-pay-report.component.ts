import { Component, OnInit } from '@angular/core';
import { ReportLoaderService, ShortPayService } from "../../services/services.barrel";
import { ConfirmComponent } from '../../components/confirm.component';
import { DialogService } from 'ng2-bootstrap-modal';
import { Toast, ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
  selector: 'app-short-pay-report',
  templateUrl: './short-pay-report.component.html',
  styleUrls: ['./short-pay-report.component.css']
})
export class ShortPayReportComponent implements OnInit {

  goToPage: any = '';
  activeToast: Toast;
  constructor(private dialogService: DialogService, private toast: ToastsManager, public shortpay: ShortPayService,public reportloader: ReportLoaderService) { }

  ngOnInit() {
    this.shortpay.fetchShortpayReport();
  }
  remove(item) {
    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: 'Remove shortpay',
      message: `Are you sure you want to remove this entry?`
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.shortpay.removeShortpay(item.prescriptionPaymentId);
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
  goto() {
    let page = Number.parseInt(this.goToPage);
    if (!this.goToPage || isNaN(page)) {
    } else if (page > 0 && ((this.reportloader.totalPages && page <= this.reportloader.totalPages) || this.reportloader.totalPages == null)) {
      this.shortpay.fetchShortpayReport(false, false, page);
    } else {
      if (this.activeToast && this.activeToast.timeoutId) {
        this.activeToast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.reportloader.totalPages
      } else {
        this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.reportloader.totalPages).then((toast: Toast) => {
          this.activeToast = toast;
        })
      }
    }
  }

}
