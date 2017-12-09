import { DocumentItem } from 'app/models/document';
import { Router } from "@angular/router";
import { Component, OnInit,NgZone, AfterViewInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DatePipe } from '@angular/common';
// Services
import { DocumentManagerService } from '../../services/document-manager.service';
import {HttpService} from "../../services/http-service";
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
  linkClaim: boolean = false;
  submitted: boolean = false;
  searchText:string='';
  documentId:any;
  constructor(
    private router: Router,
    private http: HttpService,
    private formBuilder: FormBuilder,
    public ds: DocumentManagerService,
    private dp: DatePipe,
    private ngZone:NgZone, 
    private toast: ToastsManager
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
      claimNumber :[null],
      firstName :[null], 
      groupNumber :[null],
      lastName :[null]
    });
  }

  ngOnInit() {
    this.router.routerState.root.queryParams.subscribe(params => {
      if (params['id']) {
        let file = localStorage.getItem("file-" + params['id']);
        if (file) {
          this.file = JSON.parse(file) as DocumentItem;
        } 
        this.form.patchValue({ documentId: this.file.documentId });
        this.documentId = this.file.documentId;
        this.ds.loading=false;
      }
    });
  }
  claimSelected($event){
    this.form.patchValue({
      claimId: $event.claimId,
      claimNumber:$event.claimNumber,
      firstName :$event.firstName , 
      groupNumber:$event.groupNumber,
      lastName:$event.lastName
     });
     setTimeout(()=>{
      $("#searchText").val(this.searchText);
     },300);
  }
  lastInput($event){
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
  save(){
    //console.log(this.form.value);
   /*  if (this.form.valid && this.form.get('Password').value !== this.form.get('ConfirmPassword').value) {
      this.form.get('ConfirmPassword').setErrors({"unmatched": "The password and confirmation password do not match."});
      this.toast.warning( 'The password and confirmation password do not match.');
    }
     */
    if (this.form.valid) {
      this.submitted = true;      
      try {
        let data = this.form.value;
        data.rxDate = this.dp.transform($('#rxDate').val(), "dd/M/yyyy");
        data.injuryDate = this.dp.transform($('#injuryDate').val(), "dd/M/yyyy");
        this.http.saveDocumentIndex(this.form.value).map(r=>{return r.json()}).subscribe(res => {     
            this.toast.success(res.message); 
            this.submitted = false;
            this.form.patchValue({
              claimId: '',
              claimNumber:'',
              firstName: '' , 
              groupNumber:'',
              documentTypeId:'',
              lastName:''
             });
             this.linkClaim  = false;
             this.ds.documents = this.ds.documents.delete(this.documentId);
             this.ds.totalRowCount--;
             this.router.navigate(['/main/unindexed-images/list']);
        },requestError => {
            let err = requestError.json();            
            this.toast.error(err.Message);
            this.submitted = false;
        })
      } catch (e) {
          this.submitted = false;
      } finally {

      }
    }else{
      this.submitted = false;      
       this.toast.warning('Invalid field value(s). Please correct to proceed.');
    }
  }

}
