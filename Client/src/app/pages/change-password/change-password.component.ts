import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../services/http-service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, NavigationEnd, NavigationStart } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  form: FormGroup;
  submitted = false;
  code: any;
  user: any;
  constructor(private formBuilder: FormBuilder, private http: HttpService, private router: Router, private route: ActivatedRoute, private toast: ToastrService) {
    this.form = this.formBuilder.group({
      Password: ['', Validators.compose([Validators.required])],
      ConfirmPassword: ['', Validators.compose([Validators.required])]
    });

    router.routerState.root.queryParams.subscribe(
      data => {
        this.code = data['code']; this.user = data['userId'];
      });
  }

  ngOnInit() {
  }

  submit() {
    if (!this.form.valid || (this.form.get('Password').value !== this.form.get('ConfirmPassword').value)) {
      this.form.get('ConfirmPassword').setErrors({ 'unmatched': 'Confirm password does not match password' });
      this.submitted = false;
    } else {
      this.submitted = true;
      try {
        let data;
        data = {userId: this.user, code: this.code, password: this.form.value.Password, confirmPassword: this.form.value.ConfirmPassword};
        this.http.resetpassword(data).subscribe(res => {
           this.submitted = false;
          this.toast.success('You may now login with your new password.');
          this.router.navigate(['/login']);

        }, (error) => {
          this.submitted = false;
           const err = error.error;
          this.toast.error(err.Message);
        });
      } catch (e) {
        this.submitted = false;
        this.toast.error('An error occured. Please contact your system administrator.');
      } finally {

      }
    }
  }

}
