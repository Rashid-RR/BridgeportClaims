import { Component, OnInit,AfterViewInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DatePipe } from '@angular/common';
// Services
import { DocumentManagerService } from "../../services/document-manager.service";
declare var $:any;

@Component({
  selector: 'app-unindex-image-filter',
  templateUrl: './unindex-image-filter.component.html',
  styleUrls: ['./unindex-image-filter.component.css']
})
export class UnindexedImageFilterComponent implements OnInit, AfterViewInit {

  date:string;
  fileName:string;
  submitted:boolean=false;
  constructor(
    public ds:DocumentManagerService,
    private dp: DatePipe,
    private toast: ToastsManager,
    private fb: FormBuilder) { }

  ngOnInit() {

  }
  ngAfterViewInit() {
    // Date picker
    $('#date').datepicker({
      autoclose: true
    });
  }

  search() {
    let date = this.dp.transform($('#date').val(), "dd/M/yyyy");  
    //if(this.startDate && this.endDate){
      this.ds.data.date = date || null 
      this.ds.data.fileName = this.fileName || null 
      this.ds.search();
   /*  }else{
        this.toast.warning("Ensure you select both start date and end date");
    } */
  }

 
  clearDates(){
    $('#date').val(''); 
    this.fileName = '';
  }


}
