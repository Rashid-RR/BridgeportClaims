import { Component, OnInit } from '@angular/core';
import { ReportLoaderService, SkippedPaymentService } from "../../services/services.barrel";
import { ConfirmComponent } from '../../components/confirm.component';
import { DialogService } from 'ng2-bootstrap-modal';
import { Toast, ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
  selector: 'app-skipped-payment-list',
  templateUrl: './skipped-payment-list.component.html',
  styleUrls: ['./skipped-payment-list.component.css']
})
export class SkippedPaymentListComponent implements OnInit {
  
  goToPage: any = '';
  activeToast: Toast;
  constructor(private dialogService: DialogService, private toast: ToastsManager, public skipped: SkippedPaymentService,public reportloader: ReportLoaderService) { }

  ngOnInit() {
    this.skipped.fetchSkippedPayReport();
  }
  remove(item) {
    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: 'Remove skipped payment',
      message: `Remove skipped payment with id ${item.prescriptionPaymentId}`
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.skipped.removeskippedPay(item.id);
        }
      });
  }
  next() {
    this.skipped.fetchSkippedPayReport(true);
  }
  prev() {
    this.skipped.fetchSkippedPayReport(false, true);
  }

  goto() {
    let page = Number.parseInt(this.goToPage);
    if (!this.goToPage || isNaN(page)) {
    } else if (page > 0 && ((this.reportloader.totalPages && page <= this.reportloader.totalPages) || this.reportloader.totalPages == null)) {
      this.skipped.fetchSkippedPayReport(false, false, page);
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