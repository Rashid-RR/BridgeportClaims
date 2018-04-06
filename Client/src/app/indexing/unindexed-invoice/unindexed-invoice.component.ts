import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { EventsService } from "../../services/events-service"
import { DocumentManagerService } from "../../services/document-manager.service";
import { DocumentItem } from 'app/models/document';
import { HttpService } from "../../services/http-service";
import { Toast, ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DatePipe } from '@angular/common';
import { UUID } from 'angular2-uuid';
declare var $: any;


@Component({
  selector: 'indexing-unindexed-invoice',
  templateUrl: './unindexed-invoice.component.html',
  styleUrls: ['./unindexed-invoice.component.css'],
})
export class UnindexedInvoiceComponent implements OnInit, AfterViewInit {

  file: DocumentItem;
  form: FormGroup;
  constructor(
    private dp: DatePipe,
    private http: HttpService, private route: ActivatedRoute,
    private toast: ToastsManager, private formBuilder: FormBuilder, public ds: DocumentManagerService, private events: EventsService) {
    this.form = this.formBuilder.group({
      invoiceNumber: [null, Validators.compose([Validators.required])]
    });

  }
  ngOnInit() {

    this.route.params.subscribe(params => {
      console.log("Dabo..")
      if (params['invoiceNumber']) {
        console.log(params['invoiceNumber']);
        this.form.patchValue({ invoiceNumber: params['invoiceNumber'] });
        let file = localStorage.getItem('file-' + params['id']); 
        this.ds.newInvoice = true;
        this.ds.invoiceFile = JSON.parse(file) as DocumentItem;
      }else{
        this.events.broadcast("reset-indexing-form", true);
        this.ds.cancel('invoice');
      }
    });
    
    this.events.on("archived-image", (id: any) => {
      this.ds.cancel('invoice');
    });

  }
  ngAfterViewInit() {

  }

  saveInvoice() {
    if (this.form.valid) {
      this.ds.loading = true;
      try {
        this.http.checkInvoiceNumber(this.form.value).map(r => { return r.text() }).subscribe(check => {
          if (check == null || check == 'null') {
            let data = this.form.value;
            data.documentId = this.ds.invoiceFile.documentId;
            this.ds.loading = true;
            this.http.saveInvoiceIndex(data).map(r => { return r.json() }).subscribe(res => {
              this.toast.success(res.message);
              this.ds.loading = false;
              this.form.reset();
              this.ds.invoices = this.ds.invoices.delete(this.ds.invoiceFile.documentId);
              this.ds.totalInvoiceRowCount--;
              this.ds.newInvoice = false;
              this.ds.invoiceFile = undefined;
              this.ds.loading = false;
            }, requestError => {
              let err = requestError.json();
              this.toast.error(err.Message);
              this.ds.loading = false;
            })
          } else {
            let id = UUID.UUID();
            let doc: any = {};
            doc.documentId = id
            doc.fileUrl=check.replace(/"/g,"");
            let file = doc as any
            localStorage.setItem('file-' + id, JSON.stringify(file));
            window.open('#/main/indexing/invoice/'+this.form.value.invoiceNumber+'/' + id, '_blank');
            this.ds.newInvoice = false;
            this.ds.invoiceFile = undefined;
            this.ds.loading = false;
          }
        })
      } catch (e) {
        this.ds.loading = false;
      } finally {

      }
    } else {
      let er = '';
      this.ds.loading = false;
      this.toast.warning(er, 'Please correct the folowing:', { enableHTML: true });
    }
  }
  cancel() {
    this.form.reset();
    this.ds.cancel('invoice');
  }
}

