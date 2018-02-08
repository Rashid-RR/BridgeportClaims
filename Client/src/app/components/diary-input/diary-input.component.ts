import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DatePipe } from '@angular/common';
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
  open:boolean=true;
  closed:Boolean=false;
  submitted:boolean=false;
  isClosed:Boolean=false;
  constructor(
    public ds: DiaryService,
    private dp: DatePipe,
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

  setClosed($event,value){
    if($event.target.checked){
      this.isClosed=value;
      this.search();
    }
  }
  search() {
    let startDate = this.dp.transform($('#startDate').val(), "dd/MM/yyyy");
    let endDate = this.dp.transform($('#endDate').val(), "dd/MM/yyyy");
    this.ds.data.closed=this.isClosed;
    //if(this.startDate && this.endDate){
      this.ds.data.startDate = startDate || null
      this.ds.data.endDate = endDate || null
      this.ds.search();
   /*  }else{
        this.toast.warning("Ensure you select both start date and end date");
    } */
  }

  clearText(){
    this.ds.owner=null;
    this.ds.diaryNote=null;
  }
  clearDates(){
    $('#startDate').val('');
    $('#endDate').val(''); 
  }
  reset(){
    $('#startDate').val('');
    $('#endDate').val('');
    this.ds.owner=null;
    this.ds.diaryNote=null;
  }

}
