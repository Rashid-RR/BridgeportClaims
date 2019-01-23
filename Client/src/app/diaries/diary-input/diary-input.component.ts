import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
// Services
import { DiaryService } from '../../services/diary.service';
import { Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
 
declare var $: any;

@Component({
  selector: 'app-diary-input',
  templateUrl: './diary-input.component.html',
  styleUrls: ['./diary-input.component.css']
})
export class DiaryInputComponent implements OnInit, AfterViewInit, OnDestroy {

  diaryForm: FormGroup;
  startDate: String;
  endDate: String;
  open = true;
  closed: Boolean = false;
  submitted = false;
  isClosed: Boolean = false;
  owner = new FormControl();
  diaryNote = new FormControl();
  ownerCtrlSub: Subscription;
  noteCtrlSub: Subscription;
  resizeSub: Subscription;
  constructor(
    public ds: DiaryService,
    private dp: DatePipe,
    private toast: ToastrService,
    private fb: FormBuilder
  ) {

  }

  ngOnInit() {
    this.noteCtrlSub = this.diaryNote.valueChanges
      .pipe(debounceTime(1000))
      .subscribe(newValue => {
        this.ds.data.diaryNote = newValue || undefined;
        this.search();
      });
    this.ownerCtrlSub = this.owner.valueChanges
      .pipe(debounceTime(1000))
      .subscribe(newValue => {
        this.ds.data.owner = newValue || undefined;
        this.search();
      });
  }

  ngOnDestroy() {
    this.ownerCtrlSub.unsubscribe();
    this.noteCtrlSub.unsubscribe();
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

  setClosed($event, value) {
    if ($event.target.checked) {
      this.isClosed = value;
      this.search();
    }
  }
  search() {
    const startDate = this.dp.transform($('#startDate').val(), 'MM/dd/yyyy');
    const endDate = this.dp.transform($('#endDate').val(), 'MM/dd/yyyy');
    this.ds.data.closed = this.isClosed;
    // if(this.startDate && this.endDate){
    this.ds.data.startDate = startDate || null;
    this.ds.data.endDate = endDate || null;
    this.ds.data.ownerId = this.ds.data.ownerId || null;
    this.ds.data.noteText = this.ds.data.noteText || null;
    this.ds.data.userId = this.ds.data.ownerId || null;
    this.ds.search();
    /*  }else{
         this.toast.warning("Ensure you select both start date and end date");
     } */
  }

  clearText() {
    this.ds.owner = null;
    this.ds.diaryNote = null;
  }
  clearDates() {
    $('#startDate').val('');
    $('#endDate').val('');
  }
  reset() {
    $('#startDate').val('');
    $('#endDate').val('');
    this.ds.owner = null;
    this.ds.diaryNote = null;
  }

}
