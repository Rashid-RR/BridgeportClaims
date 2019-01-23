import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {HttpService} from '../../services/http-service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  form: FormGroup;
  submitted = false;
  registered = false;
  emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  constructor(
    private formBuilder: FormBuilder,
    private http: HttpService,
    private router: Router,
    private toast: ToastrService
  ) {
    this.form = this.formBuilder.group({
      firstname: ['', Validators.compose([Validators.required])],
      lastname: ['', Validators.compose([Validators.required])],
      Email: ['', Validators.compose([Validators.pattern(this.emailRegex)])],
      Password: ['', Validators.compose([Validators.required])],
      ConfirmPassword: ['', Validators.compose([Validators.required])]
    });
  }

  ngOnInit() {

  }
  login() {
    this.router.navigate(['/login']);
  }
  register() {
    if (this.form.valid && this.form.get('Password').value !== this.form.get('ConfirmPassword').value) {
      this.form.get('ConfirmPassword').setErrors({'unmatched': 'The password and confirmation password do not match.'});
      this.toast.warning( 'The password and confirmation password do not match.');
    }
    if (this.form.valid) {
      this.submitted = true;
      try {
        this.http.register(this.form.value).subscribe(_ => {
            this.toast.success('You have registered successfully. Now, please check your email to confirm it before logging in...', null,
            {timeOut: 10000});
            this.registered = true;
            this.submitted = false;
            this.router.navigate(['/login']);
        }, requestError => {
            const err = requestError.error;
            this.toast.error(err.Message);
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
