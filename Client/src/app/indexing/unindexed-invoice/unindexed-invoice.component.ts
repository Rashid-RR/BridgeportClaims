import { Component, Input, ViewChild, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EventsService } from '../../services/events-service';
import { DocumentManagerService } from '../../services/document-manager.service';
import { DocumentItem } from '../../models/document';
import { HttpService } from '../../services/http-service';
import { Toast, ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { DialogService } from 'ng2-bootstrap-modal';
import { ConfirmComponent } from '../../components/confirm.component';
import { SwalComponent, SwalPartialTargets } from '@toverux/ngx-sweetalert2';
declare var $: any;


@Component({
  selector: 'indexing-unindexed-invoice',
  templateUrl: './unindexed-invoice.component.html',
  styleUrls: ['./unindexed-invoice.component.css'],
})
export class UnindexedInvoiceComponent implements OnInit, AfterViewInit {

  file: DocumentItem;
  form: FormGroup;
  @Input() invoiceNumber;
  @ViewChild('invoiceSwal') private invoiceSwal: SwalComponent;
  constructor(
    private dialogService: DialogService,
    public readonly swalTargets: SwalPartialTargets,
    private http: HttpService, private route: ActivatedRoute,
    private toast: ToastrService, private formBuilder: FormBuilder, public ds: DocumentManagerService, private events: EventsService) {
    this.form = this.formBuilder.group({
      invoiceNumber: [null, Validators.compose([Validators.required])]
    });

  }
  ngOnInit() {
    this.form.patchValue({ invoiceNumber: this.invoiceNumber });
    if (!this.invoiceNumber) {
      this.events.broadcast('reset-indexing-form', true);
      this.ds.cancel('invoice');
    }
    this.events.on('archived-image', (id: any) => {
      this.ds.cancel('invoice');
    });
  }
  ngAfterViewInit() {

  }

  saveInvoice() {
    if (this.form.valid) {
      this.ds.loading = true;
      try {
        this.http.checkInvoiceNumber(this.form.value).subscribe(check => {
          if (!check || (!check.documentId && !check.invoiceNumberIsAlreadyIndexed)) {
            const data = this.form.value;
            data.documentId = this.ds.invoiceFile.documentId;
            this.ds.loading = true;
            this.http.saveInvoiceIndex(data).subscribe(res => {
              this.toast.success(res.message);
              this.ds.loading = false;
              this.form.reset();
              this.ds.invoices = this.ds.invoices.delete(this.ds.invoiceFile.documentId);
              this.ds.totalInvoiceRowCount--;
              this.ds.newInvoice = false;
              this.ds.invoiceFile = undefined;
              this.ds.loading = false;
              this.ds.closeModal();
            }, requestError => {
              const err = requestError.error;
              this.toast.error(err.Message);
              this.ds.loading = false;
            });
          } else {
            const doc: any = check;
            this.ds.invoiceFile = doc;
            localStorage.setItem('file-' + doc.documentId, JSON.stringify(doc));
            this.ds.newInvoice = true;
            this.invoiceSwal.show().then((r) => { });
            this.toast.warning('The Invoice # ' + this.form.controls['invoiceNumber'].value + ' has already been used. Here is the Invoice that it has been indexed with ' + this.form.controls['invoiceNumber'].value + '. If you Archive this invoice, then #' + this.form.controls['invoiceNumber'].value + ' will be free to use for another invoice', null, { timeOut: 1210000, closeButton: true, positionClass: 'toast-top-center' });
            this.ds.loading = false;
          }
        });
      } catch (e) {
        this.ds.loading = false;
      } finally {

      }
    } else {
      const er = '';
      this.ds.loading = false;
      this.toast.warning(er, 'Please correct the folowing:', { enableHtml: true });
    }
  }
  archive() {
    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: 'Archive Image',
      message: 'Are you sure you wish to archive ' + (this.ds.invoiceFile.fileName || '') + '?'
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.ds.archive(this.ds.invoiceFile.documentId, false, 'searchInvoices');
        }
      });
  }
  cancel() {
    this.form.reset();
    this.ds.cancel('invoice');
  }
}

