import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
// Services
import { DiaryService } from '../../services/diary.service';
declare var $:any;

@Component({
  selector: 'app-diary-input',
  templateUrl: './diary-input.component.html',
  styleUrls: ['./diary-input.component.css']
})
export class DiaryInputComponent implements OnInit, AfterViewInit {

  diaryForm: FormGroup;
  startDate: String;
  endDate: String;
  constructor(
    private ds: DiaryService,
    private toast: ToastsManager,
    private fb: FormBuilder
  ) {
      
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    // Date picker
    $('#startDate').datepicker({
      autoclose: true
    });
    $('#endDate').datepicker({
      autoclose: true
    });
    $('#datemask').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' });
    $('[data-mask]').inputmask();
  }

  search() {
    this.startDate = $('#startDate').val();
    this.endDate = $('#endDate').val(); 
    console.log(this.startDate,this.endDate);
    if(this.startDate && this.endDate){
      this.ds.data.startDate =this.startDate
      this.ds.data.endDate = this.endDate
      this.ds.search();
    }else{
        this.toast.warning("Ensure you select both start date and end date");
    }
  }

}
