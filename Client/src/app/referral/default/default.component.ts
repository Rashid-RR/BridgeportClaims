import { Component, OnInit, AfterViewInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { HttpService } from "../../services/http-service";
import { ToastsManager } from 'ng2-toastr';
import { DatePipe } from '@angular/common';

declare var $: any;

@Component({
  selector: 'client-referral',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.css']
})
export class ReferralDefaultComponent implements OnInit, AfterViewInit {
  form: FormGroup;
  submitted: boolean = false;
  states: any[] = [];
  referralTypes: any[] = [];
  constructor(
    private dp: DatePipe,
    private formBuilder: FormBuilder,
    private http: HttpService,
    private router: Router,
    private toast: ToastsManager
  ) {
    this.form = this.formBuilder.group({
      claimNumber: [null, Validators.compose([Validators.required])],
      jurisdictionStateId: [null, Validators.compose([Validators.required])],
      lastName: [null, Validators.compose([Validators.required])],
      firstName: [null, Validators.compose([Validators.required])],
      dateOfBirth: [null, Validators.compose([Validators.required])],
      injuryDate: [null, Validators.compose([Validators.required])],
      notes: [null],
      referralTypeId: [null, Validators.compose([Validators.required])],
      eligibilityStart: [null],
      eligibilityEnd: [null],
      address1: [null, Validators.compose([Validators.required])],
      address2: [null],
      city: [null, Validators.compose([Validators.required])],
      stateId: [null, Validators.compose([Validators.required])],
      postalCode: [null, Validators.compose([Validators.required])],
      patientPhone: [null],
      adjustorName: [null],
      adjustorPhone: [null]
    });
  }

  reset() {
    this.form.reset();
    $('#dateOfBirth').val('');
    $('#injuryDate').val('');
  }
  ngAfterViewInit() {
    // Date picker
    $('#eligibilityStart').inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" })
    .on('change', (ev) => {
      this.form.controls.eligibilityStart.setValue(ev.target.value);
    });
    $('#eligibilityEnd').inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" })
    .on('change', (ev) => {
      this.form.controls.eligibilityEnd.setValue(ev.target.value);
    });
    $('#dateOfBirth').inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" })
    .on('change', (ev) => {
      this.form.controls.dateOfBirth.setValue(ev.target.value);
    });
    $('#injuryDate').inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" })
    .on('change', (ev) => {
      this.form.controls.injuryDate.setValue(ev.target.value);
    });
    $('#patientPhone').inputmask().on('change', (ev) => {
      let val=ev.target.value.replace(/[()-\s]/g,'');
      this.form.controls.patientPhone.setValue(val);
    });
    $('#adjustorPhone').inputmask().on('change', (ev) => {
      let val=ev.target.value.replace(/[()-\s]/g,'');
      this.form.controls.adjustorPhone.setValue(val);
    });
  }
  ngOnInit() {
    this.http.referralTypes({}).single().subscribe(res => {
      this.referralTypes = res;
    }, () => { });
    this.http.states({}).single().subscribe(res => {
      this.states = res;
    }, () => { });
  }
  updateDate(field: string) {
    this.form.controls[field].setValue(this.dp.transform($(`#${field}`).val(), "MM/dd/yyyy"));
  }
  submit() {
    if (this.form.valid) {
      this.submitted = true;
      try {
        this.http.insertReferral(this.form.value).subscribe((res) => {
          this.toast.success(res.message || "Referral successfully added", null,
            { toastLife: 10000 });
          this.submitted = false;
          this.reset();
        }, re => {
          let err = re.error;
          this.toast.error(err.message);
          this.submitted = false;
        })
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
