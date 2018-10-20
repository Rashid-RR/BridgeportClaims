import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { HttpService } from "../../services/http-service";
import { ToastsManager } from 'ng2-toastr';

@Component({
  selector: 'client-referral',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.css']
})
export class ReferralDefaultComponent implements OnInit {
  form: FormGroup;
  submitted: boolean = false;
  registered: boolean = false;
  emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  constructor(
    private formBuilder: FormBuilder,
    private http: HttpService,
    private router: Router,
    private toast: ToastsManager
  ) {
    this.form = this.formBuilder.group({
        claimNumber: ['', Validators.compose([Validators.required])],
        jurisdictionStateId: ['', Validators.compose([Validators.required])],
        lastName: ['', Validators.compose([Validators.required])],
        firstName: ['', Validators.compose([Validators.required])],
        dateOfBirth: ['', Validators.compose([Validators.required])],
        injuryDate: ['', Validators.compose([Validators.required])],
        notes: ['', Validators.compose([Validators.required])],
        referralTypeId: ['', Validators.compose([Validators.required])],
        eligibilityStart: ['', Validators.compose([Validators.required])],
        eligibilityEnd: ['', Validators.compose([Validators.required])],
        address1: ['', Validators.compose([Validators.required])],
        address2: ['', Validators.compose([Validators.required])],
        city: ['', Validators.compose([Validators.required])],
        stateId: ['', Validators.compose([Validators.required])],
        postalCode: ['', Validators.compose([Validators.required])],
        patientPhone: ['', Validators.compose([Validators.required])],
        adjustorName: ['', Validators.compose([Validators.required])],
        adjustorPhone: ['', Validators.compose([Validators.required])]
    });
  }

  ngOnInit() {

  }
  register() {
    console.log(this.form.value);
    if (this.form.valid && this.form.get('Password').value !== this.form.get('ConfirmPassword').value) {
      this.form.get('ConfirmPassword').setErrors({ "unmatched": "The password and confirmation password do not match." });
      this.toast.warning('The password and confirmation password do not match.');
    }
    if (this.form.valid) {
      this.submitted = true;
      try {
        this.http.insertReferral(this.form.value).subscribe(res => {
          this.toast.success("You have registered successfully. Now, please check your email to confirm it before logging in...", null,
            { toastLife: 10000 });
          this.registered = true
          this.submitted = false;
          this.router.navigate(['/login']);
        }, requestError => {
          let err = requestError.error;
          this.toast.error(err.Message);
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
