import { DocumentItem } from '../../models/document';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { Component, Input, OnInit, NgZone, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Toast, ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
// Services
import { DocumentManagerService } from '../../services/document-manager.service';
import { HttpService } from '../../services/http-service';
import { DialogService } from 'ng2-bootstrap-modal';
import { EventsService } from '../../services/events-service';
import { ConfirmComponent } from '../../components/confirm.component';
declare var $: any;

@Component({
  selector: 'indexing-index-file',
  templateUrl: './index-file.component.html',
  styleUrls: ['./index-file.component.css']
})
export class IndexFileComponent implements OnInit, AfterViewInit {

  @Input() file: DocumentItem;
  dropdownVisible = false;
  form: FormGroup;
  loading = false;
  linkClaim = false;
  submitted = false;
  searchText = '';
  documentId: any;
  showDropDown = new Subject<any>();
  constructor(
    private http: HttpService,
    private events: EventsService,
    private formBuilder: FormBuilder,
    public ds: DocumentManagerService,
    private dp: DatePipe,
    private dialogService: DialogService,
    private toast: ToastrService
  ) {
    this.form = this.formBuilder.group({
      documentId: [null, Validators.compose([Validators.required])],
      claimId: [null, Validators.compose([Validators.required])],
      documentTypeId: ['', Validators.compose([Validators.required])],
      rxDate: [null],
      rxNumber: [null],
      invoiceNumber: [null],
      injuryDate: [null],
      attorneyName: [null],
      claimNumber: [null],
      firstName: [null],
      groupNumber: [null],
      lastName: [null]
    });
  }

  ngOnInit() {
    this.events.on('reset-indexing-form', t => {
      this.form.reset();
    });
    this.form.patchValue({ documentId: this.file.documentId });
    this.documentId = this.file.documentId;
    this.ds.loading = false;
  }
  claimSelected($event) {
    if (this.searchText && $event.claimId) {
      this.form.patchValue({
        claimId: $event.claimId,
        claimNumber: $event.claimNumber,
        firstName: $event.firstName,
        groupNumber: $event.groupNumber,
        lastName: $event.lastName
      });
      this.toast.info($event.lastName + ' ' + $event.firstName + ' ' + $event.claimNumber + ' has been linked', 'Claim Linked', { enableHtml: true, positionClass: 'toast-top-center' });
      setTimeout(() => {
        this.searchText = undefined;
        this.dropdownVisible = false;
      }, 100);
    }
  }
  checkMatch($event) {
    this.ds.exactMatch = $event.target.checked;
    this.showDropDown.next($event.target.checked);
  }
  lastInput($event) {
    this.searchText = $event.target.value;
  }
  ngAfterViewInit() {
    // Date picker
    $('#rxDate').datepicker({
      autoclose: true
    });
    $('#injuryDate').datepicker({
      autoclose: true
    });
    $('#datemask').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' });
    $('[data-mask]').inputmask();

  }
  saveImage() {
    this.dialogService.addDialog(ConfirmComponent, {
      title: 'Save Image',
      message: 'Are you sure you would like to save the indexing of this image?'
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.save();
        }
      });
  }
  archive() {
    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: 'Archive Image',
      message: 'Are you sure you wish to archive ' + this.file.fileName + '?'
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          this.ds.archive(this.file.documentId);
        }
      });
  }
  save() {
    if (this.form.valid) {
      this.submitted = true;
      try {
        const data = this.form.value;
        data.rxDate = this.dp.transform($('#rxDate').val(), 'MM/dd/yyyy');
        data.injuryDate = this.dp.transform($('#injuryDate').val(), 'MM/dd/yyyy');
        this.ds.loading = true;
        this.http.saveDocumentIndex(this.form.value).subscribe(res => {
          this.toast.success(res.message);
          this.submitted = false;
          this.form.reset();
          this.linkClaim = false;
          this.ds.documents = this.ds.documents.delete(this.documentId);
          this.ds.totalRowCount--;
          this.ds.newIndex = false;
          this.ds.file = undefined;
          this.ds.loading = false;
        }, requestError => {
          const err = requestError.error;
          this.toast.error(err.Message);
          this.submitted = false;
          this.ds.loading = false;
        });
      } catch (e) {
        this.submitted = false;
      } finally {

      }
    } else {
      let er = '';
      if (!this.form.get('documentTypeId').valid) {
        er += '* Select a image type from the type dropdown';
      }
      if (!this.form.get('claimId').valid) {
        er += '<br/>* Link a claim';
      }
      this.submitted = false;
      this.toast.warning(er, 'Please correct the folowing:', { enableHtml: true });
    }
  }
  cancel() {
    this.form.reset();
    this.ds.cancel('image');
  }

}
