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
      message: `Remove shortpay with id ${item.prescriptionPaymentId}`
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.shortpay.removeShortpay(item.id);
        }
      });
  }

  next() {
    this.shortpay.fetchShortpayReport(true);
  }
  prev() {
    this.shortpay.fetchShortpayReport(false, true);
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
