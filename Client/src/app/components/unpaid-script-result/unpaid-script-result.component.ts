import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { UnpaidScriptService, HttpService } from '../../services/services.barrel';
import { ConfirmComponent } from '../confirm.component';
import { DialogService } from 'ng2-bootstrap-modal';
import { Toast, ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-unpaid-script-result',
  templateUrl: './unpaid-script-result.component.html',
  styleUrls: ['./unpaid-script-result.component.css']
})
export class UnpaidScriptResultsComponent implements OnInit {
  goToPage: any = '';
  activeToast: number;
  constructor(private dialogService: DialogService,  public uss: UnpaidScriptService, private http: HttpService,
    public viewContainerRef: ViewContainerRef, private toast: ToastrService) {

  }

  ngOnInit() {
    this.uss.getPayors(1);
  }
  archive(u: any) {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Archive Script',
      message: `Are you sure you wish to archive this unpaid script?`
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.http.archivePrescription({ prescriptionId: u.prescriptionId })
            .subscribe(r => {
              this.toast.success(r.message);
              this.uss.search();
            }, err => {
              this.toast.warning('Could not archive script');
            });
        }
      });
  }
  next() {
    this.uss.search(true);
  }
  goto() {
    const page = Number.parseInt(this.goToPage);
    if (!this.goToPage) {

    } else if (page > 0 && page <= this.uss.totalPages) {
      this.uss.search(false, false, page);
    } else {
      let toast = this.toast.toasts.find(t=>t.toastId ==this.activeToast)
      if (toast) {
        toast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.uss.totalPages;
      } else {
        this.activeToast = this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.uss.totalPages).toastId
      }
    }
  }
  prev() {
    this.uss.search(false, true);
  }

  openClaim(unpaidScript: any) {
    if (unpaidScript.claimId) {
      window.open('#/main/claims?claimId=' + unpaidScript.claimId, '_blank');
    }
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
