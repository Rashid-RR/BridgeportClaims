import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../services/http-service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-password-reset',
  templateUrl: './password-reset.component.html',
  styleUrls: ['./password-reset.component.css']
})
export class PasswordResetComponent implements OnInit {

  form: FormGroup;
  submitted = false;
  emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  constructor(private formBuilder: FormBuilder, private http: HttpService, private router: Router,
    private toast: ToastrService) {
    this.form = this.formBuilder.group({
      email: ['', Validators.compose([Validators.required, Validators.pattern(this.emailRegex)])],
    });
  }

  ngOnInit() {

  }

  resetPassword() {
    if (this.form.valid) {
      this.submitted = true;
      this.http.changepassword({ email: this.form.get('email').value })
        .subscribe(res => {
          this.submitted = false;
        }, error => {
          this.submitted = false;
          this.form.get('email').setErrors({ 'error': 'The email address entered is incorrect.' });
          this.toast.error('The email address entered is incorrect.');

        });
    }

  }

  register() {
    this.router.navigate(['/register']);
  }
  login() {
    this.router.navigate(['/login']);
  }
  submit() {
    this.submitted = true;
    if (this.form.valid) {
      try {
        this.http.forgotpassword(this.form.value).subscribe(res => {
          this.toast.success('An email has been sent for you to reset your password.');
          this.router.navigate(['/login']);

        }, (error) => {
          this.submitted = false;
          const err = error.error;
          this.toast.error(err.Message);
          if (error.status !== 500) {
            this.form.get('email').setErrors({ 'auth': 'The email address entered is incorrect.' });
          }
        });
      } catch (e) {
        this.submitted = false;
        this.toast.warning('Please enter a valid email address.');
        this.form.get('email').setErrors({ 'auth': 'The email address entered is incorrect.' });
      } finally {

      }
    }
  }


}

