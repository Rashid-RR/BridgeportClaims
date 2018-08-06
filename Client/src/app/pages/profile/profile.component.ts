import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { HttpService } from "../../services/http-service"
import { ClaimManager } from "../../services/claim-manager";
import { UserProfile } from "../../models/profile";
import { ProfileManager } from "../../services/profile-manager";
import { ToastsManager } from 'ng2-toastr';

@Component({
  selector: 'app-profile',
  templateUrl: "profile.component.html",
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  form: FormGroup;
  submitted: boolean = false;
  loading: boolean = false;
  registered: boolean = false;
  emailRegex = '^[A-Za-z0-9]+(\.[_A-Za-z0-9]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,15})$';
  saveLogin: boolean = false;
  saveEmail: boolean = false;
  loginError: string = '';
  emailError: string = '';
  constructor(
    private formBuilder: FormBuilder,
    public claimManager: ClaimManager,
    private http: HttpService,
    public profileManager: ProfileManager,
    private toast: ToastsManager
  ) {
    /* console.log(this.profileManager.User);
    console.log(this.profileManager.profile); */
    this.profileManager.profileChanged.subscribe(r=>{
      this.form = this.formBuilder.group({
        firstName: [this.profileManager.profile.firstName, Validators.compose([Validators.required])],
        lastName: [this.profileManager.profile.lastName, Validators.compose([Validators.required])],
        oldPassword: [''],
        extension: [this.profileManager.profile.extension],
        newPassword: [""],
        confirmPassword: [""]
      });
    })
   
  }

  ngOnInit() {
    if (this.profileManager.profile == null) {
      this.profileManager.profile = new UserProfile('', '', '', '', '');
    }
    this.form = this.formBuilder.group({
      firstName: [this.profileManager.profile.firstName, Validators.compose([Validators.required])],
      lastName: [this.profileManager.profile.lastName, Validators.compose([Validators.required])],
      oldPassword: [''],
      extension: [this.profileManager.profile.extension],
      newPassword: [""],
      confirmPassword: [""]
    });
  }

  updateUserInfo() {
    if (this.form.valid && !this.loading) {
      this.loading = true;
      try {
        this.http.changeusername(this.form.value.firstName, this.form.value.lastName, this.profileManager.profile.id,this.form.value.extension).subscribe(res => {
          this.toast.success('User name updated successfully');
          this.profileManager.profile.firstName = this.form.value.firstName;
          this.profileManager.profile.lastName = this.form.value.lastName;
          this.profileManager.profile.extension = this.form.value.extension;
          this.registered = true
          this.loading = false;
        }, error => {
          let err = error.error || ({ "Message": "Server error!" });
          error(err.Message);
          this.loading = false;
        })
      } catch (e) {
        this.loading = false;
        this.toast.error('Error in fields. Please correct to proceed!');

      }
    } else {
      this.loading = false;
      this.toast.error('Error in fields. Please correct to proceed!');
    }
  }
  submitForm(form: any): void {
    if (this.form.valid && this.form.dirty) {
      if (this.form.value.firstName != this.profileManager.profile.firstName || this.form.value.lastName != this.profileManager.profile.lastName || this.form.value.extension != this.profileManager.profile.extension) {
        this.updateUserInfo();
      }
      if (this.form.get('oldPassword').value != '' || this.form.get('newPassword').value != '' || this.form.get('confirmPassword').value != '') {
        this.updatePassword();
      }
    } else {
      console.log(this.form.valueChanges);
    }

  }
  updatePassword() {
    this.submitted = true;
    if (this.form.valid && this.form.get('newPassword').value !== this.form.get('confirmPassword').value) {
      this.form.get('confirmPassword').setErrors({ "unmatched": "Confirm password does not match password" });
    }
    if (this.form.valid && !this.loading) {
      this.loading = true;
      try {
        this.http.changepassword(this.form.value).subscribe(res => {
          this.toast.success('Password successfully changed');
          this.registered = true
          this.loading = false;
        }, error => {
          let err = error.error || ({ "Message": "Server error!" });
          this.toast.error(err.Message);
          this.loading = false;
        })
      } catch (e) {
        this.loading = false;
        this.toast.error('Error in fields. Please correct to proceed!');

      }
    } else {
      this.loading = false;
      this.toast.error('Error in fields. Please correct to proceed!');
    }
  }
}