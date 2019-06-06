import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpService } from '../../services/http-service';
import { ToastrService } from 'ngx-toastr';
import { DatePipe } from '@angular/common';
import { GenderItem } from './gender-item.model';

declare var $: any;

@Component({
  selector: 'client-referral',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.css']
})
export class ReferralDefaultComponent implements OnInit, AfterViewInit {
  form: FormGroup;
  submitted = false;
  states: any[] = [];
  genders: GenderItem[] = [
    { genderId: 1, genderName: 'Male' },
    { genderId: 2, genderName: 'Female' },
    { genderId: 3, genderName: 'Not Specified' }
  ];
  constructor(
    private dp: DatePipe,
    private formBuilder: FormBuilder,
    private http: HttpService,
    private toast: ToastrService
  ) {
    this.form = this.formBuilder.group({
      claimNumber: [null, Validators.compose([Validators.required])],
      jurisdictionStateId: [null, Validators.compose([Validators.required])],
      lastName: [null, Validators.compose([Validators.required])],
      firstName: [null, Validators.compose([Validators.required])],
      dateOfBirth: [null, Validators.compose([Validators.required])],
      injuryDate: [null, Validators.compose([Validators.required])],
      notes: [null],
      eligibilityStart: [null],
      eligibilityEnd: [null],
      address1: [null, Validators.compose([Validators.required])],
      address2: [null],
      city: [null, Validators.compose([Validators.required])],
      stateId: [null, Validators.compose([Validators.required])],
      postalCode: [null, Validators.compose([Validators.required])],
      patientPhone: [null],
      adjustorName: [null],
      adjustorPhone: [null],
      personCode: [null],
      genderId: [null, Validators.compose([Validators.required])],
      genderName: [null],
      insuranceCarrierName: [null, Validators.compose([Validators.required, Validators.minLength(2)])],
    });
  }

  reset() {
    this.form.reset();
    $('#dateOfBirth').val('');
    $('#injuryDate').val('');
  }
  ngAfterViewInit() {
    // Date picker
    $('#eligibilityStart').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
      .on('change', (ev) => {
        this.form.controls.eligibilityStart.setValue(ev.target.value);
      });
    $('#eligibilityEnd').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
      .on('change', (ev: { target: { value: any; }; }) => {
        this.form.controls.eligibilityEnd.setValue(ev.target.value);
      });
    $('#dateOfBirth').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
      .on('change', (ev: { target: { value: any; }; }) => {
        this.form.controls.dateOfBirth.setValue(ev.target.value);
      });
    $('#injuryDate').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
      .on('change', (ev: { target: { value: any; }; }) => {
        this.form.controls.injuryDate.setValue(ev.target.value);
      });
    $('#patientPhone').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.form.controls.patientPhone.setValue(val);
    });
    $('#adjustorPhone').inputmask().on('change', (ev) => {
      const val = ev.target.value.replace(/[()-\s]/g, '');
      this.form.controls.adjustorPhone.setValue(val);
    });
  }
  ngOnInit() {
    this.http.states({}).subscribe(res => {
      this.states = res;
    }, () => {});
  }
  updateDate(field: string) {
    this.form.controls[field].setValue(this.dp.transform($(`#${field}`).val(), 'MM/dd/yyyy'));
  }
  submit() {
    if (this.form.valid) {
      this.submitted = true;
      const gender =  this.form.get('genderId').value;
      if (gender == 1) {
        this.form.controls.genderName.setValue('Male');
      } else if ( gender == 2) {
        this.form.controls.genderName.setValue('Female');
      } else {
        this.form.controls.genderName.setValue('Not Specified');
      }

      try {
        this.http.insertReferral(this.form.value).subscribe((res) => {
          this.toast.success(res.message || 'Referral successfully added', null,
            { timeOut: 10000 });
          this.submitted = false;
          this.reset();
        }, re => {
          const err = re.error;
          this.toast.error(err.message);
          this.submitted = false;
        });
      } catch (e) {
        this.submitted = false;
      } finally {

      }
    } else {
      this.submitted = false;
      this.toast.warning('Invalid field value(s). Please correct to proceed.');
    }
  }
}
