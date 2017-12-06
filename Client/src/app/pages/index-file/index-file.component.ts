import { DocumentItem } from 'app/models/document';
import { Router } from "@angular/router";
import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DatePipe } from '@angular/common';
// Services
import { DocumentManagerService } from '../../services/document-manager.service';
declare var $: any;

@Component({
  selector: 'app-index-file',
  templateUrl: './index-file.component.html',
  styleUrls: ['./index-file.component.css']
})
export class IndexFileComponent implements OnInit, AfterViewInit {

  file: DocumentItem;
  form: FormGroup;

  loading: boolean = false;
  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private ds: DocumentManagerService
  ) {
    this.form = this.formBuilder.group({
      documentId: [null],
      claimId: [null],
      documentTypeId: [''],
      rxDate: [null],
      rxNumber: [null],
      invoiceNumber: [null],
      injuryDate: [null],
      attorneyName: [null],
    });
  }

  ngOnInit() {
    this.router.routerState.root.queryParams.subscribe(params => {
      if (params['id']) {
        let file = localStorage.getItem("file-" + params['id']);
        if (file) {
          this.file = JSON.parse(file) as DocumentItem;
        }
        console.log(file);
        this.form.patchValue({ documentId: this.file.documentId });
        this.ds.loading=false;
      }
    });
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

}
