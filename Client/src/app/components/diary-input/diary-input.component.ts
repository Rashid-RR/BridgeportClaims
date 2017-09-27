import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

// Services
import { DiaryService } from '../../services/diary.service';

@Component({
  selector: 'app-diary-input',
  templateUrl: './diary-input.component.html',
  styleUrls: ['./diary-input.component.css']
})
export class DiaryInputComponent implements OnInit, AfterViewInit {

  diaryForm: FormGroup;

  constructor(
    private ds: DiaryService,
    private fb: FormBuilder
  ) {
    this.diaryForm = this.fb.group({
      startDate: [null],
      endDate: [null]
    });
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    // Date picker
    window['jQuery']('#startDate').datepicker({
      autoclose: true
    });
    window['jQuery']('#endDate').datepicker({
      autoclose: true
    });
    window['jQuery']('#datemask').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' });
    window['jQuery']('[data-mask]').inputmask();
  }

  textChange(controlName: string) {
    if (this.diaryForm.get(controlName).value === 'undefined' || this.diaryForm.get(controlName).value === '') {
      this.diaryForm.get(controlName).setValue(null);
    }
  }

  refresh() {
  }

}
