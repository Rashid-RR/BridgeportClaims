import { ConfirmComponent } from './../../components/confirm.component';
import { Component, OnInit } from '@angular/core';
import { DialogService } from 'ng2-bootstrap-modal';
import { InvoiceProcessService } from '../../services/services.barrel';
import { HttpService } from './../../services/http-service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-invoice-process-search-filter',
  templateUrl: './invoice-process-search-filter.component.html',
  styleUrls: ['./invoice-process-search-filter.component.css']
})
export class InvoiceProcessSearchFilterComponent implements OnInit {
  imgSrc = 'assets/images/ButtonNormal.png';
  constructor(public invoiceProcessService: InvoiceProcessService, private http: HttpService, private dialogService: DialogService,
    private toast: ToastrService) {}

  ngOnInit(): void {}

  clearFilter(): void {
    this.invoiceProcessService.filterText = '';
  }

  refreshList(): void {
    this.invoiceProcessService.filterText = '';
    this.invoiceProcessService.refreshList$.next(true);
  }

  generatePdfs(): void {
    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: `Generate PDF's`,
      message: `Are you sure you're ready to generate all PDF's?`
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.invoiceProcessService.loading = true;
          this.http.processInvoices().subscribe(res => {
            this.toast.success(res.message);
            this.invoiceProcessService.loading = false;
            this.invoiceProcessService.refreshList$.next(true);
          }, error => {
            this.toast.error(error.message);
            this.invoiceProcessService.loading = false;
          });
        }
      });
  }
}
