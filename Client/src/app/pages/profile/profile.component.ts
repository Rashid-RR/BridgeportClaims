import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {HttpService} from '../../services/http-service';
import {ClaimManager} from '../../services/claim-manager';
import {UserProfile} from '../../models/profile';
import {ProfileManager} from '../../services/profile-manager';
import {ToastsManager} from 'ng2-toastr';
import { StringService } from '../../services/string.service';

@Component({
  selector: 'app-profile',
  templateUrl: 'profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  form: FormGroup;
  submitted = false;
  loading = false;
  registered = false;
  emailRegex = '^[A-Za-z0-9]+(\.[_A-Za-z0-9]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,15})$';
  saveLogin = false;
  saveEmail = false;
  loginError = '';
  emailError = '';

  constructor(
    private formBuilder: FormBuilder,
    public claimManager: ClaimManager,
    private http: HttpService,
    public profileManager: ProfileManager,
    private toast: ToastsManager,
    private stringService: StringService
  ) {
    this.profileManager.profileChanged.subscribe(r => {
      if (this.profileManager.profile.extension === undefined ) {
        this.profileManager.profile.extension = '';
      }

      this.form = this.formBuilder.group({
        firstName: [this.profileManager.profile.firstName, Validators.compose([Validators.required, Validators.minLength(2)])],
        lastName: [this.profileManager.profile.lastName, Validators.compose([Validators.required, Validators.minLength(2)])],
        oldPassword: [''],
        extension: [this.profileManager.profile.extension],
        newPassword: [''],
        confirmPassword: ['']
      });
    });

  }

  ngOnInit() {
    if (this.profileManager.profile == null) {
      this.profileManager.profile = new UserProfile('', '', '', '', '');
    }
    this.form = this.formBuilder.group({
      firstName: [this.profileManager.profile.firstName],
      lastName: [this.profileManager.profile.lastName],
      oldPassword: [''],
      extension: [this.profileManager.profile.extension],
      newPassword: [''],
      confirmPassword: ['']
    });
  }

  updateUserInfo() {
    if (this.form.valid && !this.loading) {
      let returnNow = false;
      if (this.stringService.isNullOrWhitespace(this.form.value.firstName)) {
        this.toast.warning('The first name field must be populated.');
        returnNow = true;
      }
      if (this.stringService.isNullOrWhitespace(this.form.value.lastName)) {
        this.toast.warning('The last name field must be populated.');
        returnNow = true;
      }
      if (returnNow) {
        return;
      }
      if (this.form.value.firstName.length < 2 || this.form.value.lastName.length < 2) {
        this.toast.warning('The first and last name must be at least 2 characters long.');
        return;
      }
      this.loading = true;
      try {
        this.http.changeusername(this.form.value.firstName, this.form.value.lastName,
          this.profileManager.profile.id, this.form.value.extension).subscribe(res => {
          this.toast.success('User name updated successfully.');
          const user = localStorage.getItem('user');
          if (user !== null && user.length > 0) {
            try {
              const us = JSON.parse(user);
              us.firstName = this.form.value.firstName;
              us.lastName = this.form.value.lastName;
              us.extension = this.form.value.extension;
              localStorage.removeItem('user');
              localStorage.setItem('user', JSON.stringify(us));
            } catch (e) {
            }
          }
          this.profileManager.profile.firstName = this.form.value.firstName;
          this.profileManager.profile.lastName = this.form.value.lastName;
          this.profileManager.profile.extension = this.form.value.extension;
          this.registered = true;
          this.loading = false;
        }, error => {
          const err = error.error || ({'Message': 'Server error.'});
          error(err.Message);
          this.loading = false;
        });
      } catch (e) {
        this.loading = false;
        this.toast.error('Error in fields. Please correct to proceed.');
      }
    } else {
      this.loading = false;
      this.toast.error('Error in fields. Please correct to proceed.');
    }
  }

  submitForm(form: any): void {
    if (this.form.valid && this.form.dirty) {
      /*if (this.form.value.firstName !== this.profileManager.profile.firstName || this.form.value.lastName !==
        this.profileManager.profile.lastName || this.form.value.extension !== this.profileManager.profile.extension) {
        this.updateUserInfo();
      }
      if (!this.stringService.isNullOrWhitespace(this.form.get('oldPassword').value) ||
        !this.stringService.isNullOrWhitespace(this.form.get('newPassword').value) ||
        !this.stringService.isNullOrWhitespace(this.form.get('confirmPassword').value)) {
        this.updatePassword();*/
        this.changeUserNameAndPassword();
    }
  }

  changeUserNameAndPassword() {
      if (this.form.valid && !this.loading) {
        let returnNow = false;
        if (this.stringService.isNullOrWhitespace(this.form.value.firstName)) {
          this.toast.warning('The first name field must be populated.');
          returnNow = true;
        }
        if (this.stringService.isNullOrWhitespace(this.form.value.lastName)) {
          this.toast.warning('The last name field must be populated.');
          returnNow = true;
        }
        if (returnNow) {
          return;
        }
        if (this.form.value.firstName.length < 2 || this.form.value.lastName.length < 2) {
          this.toast.warning('The first and last name must be at least 2 characters long.');
          return;
        }
      this.loading = false;
      this.submitted = true;
      if (this.form.valid && !this.loading) {
        this.loading = true;
        try {
          this.http.changeUserNameAndPassword(this.form.value).subscribe(res => {
            this.toast.success(res.message);
            const user = localStorage.getItem('user');
            if (user !== null && user.length > 0) {
              try {
                const us = JSON.parse(user);
                us.firstName = this.form.value.firstName;
                us.lastName = this.form.value.lastName;
                us.extension = this.form.value.extension;
                localStorage.removeItem('user');
                localStorage.setItem('user', JSON.stringify(us));
                } catch (e) {
              }
            }
            this.registered = true;
            this.loading = false;
          }, error => {
            const err = error.error || ({'Message': 'Server error.'});
            this.toast.error(err.Message);
            this.loading = false;
          });
        } catch (e) {
          this.loading = false;
          this.toast.error('Error in fields. Please correct to proceed.');
        }
      } else {
        this.loading = false;
        this.toast.error('Error in fields. Please correct to proceed.');
      }
    }
  }

  updatePassword() {
    if (this.form.get('oldPassword').value === '' || this.form.get('newPassword').value === '' ||
      this.form.get('confirmPassword').value === '') {
      this.toast.error('Please fillout all password fields.');
      return;
    }

    this.submitted = true;
    if (this.form.valid && this.form.get('newPassword').value !== this.form.get('confirmPassword').value) {
      this.form.get('confirmPassword').setErrors({'unmatched': 'Confirm password does not match password'});
    }
    this.loading = false;
    if (this.form.valid && !this.loading) {
      this.loading = true;
      try {
        this.http.changepassword(this.form.value).subscribe(res => {
          this.toast.success('Password successfully changed');
          this.registered = true;
          this.loading = false;
        }, error => {
          const err = error.error || ({'Message': 'Server error.'});
          this.toast.error(err.Message);
          this.loading = false;
        });
      } catch (e) {
        this.loading = false;
        this.toast.error('Error in fields. Please correct to proceed.');

      }
    } else {
      this.loading = false;
      this.toast.error('Error in fields. Please correct to proceed.');
    }
  }
}
