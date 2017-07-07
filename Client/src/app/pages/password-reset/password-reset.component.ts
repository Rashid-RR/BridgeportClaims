import { Component, OnInit } from "@angular/core";
import { HttpService } from "../../services/http-service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { warn, success, error } from "../../models/notification"
@Component({
  selector: 'app-password-reset',
  templateUrl: './password-reset.component.html',
  styleUrls: ['./password-reset.component.css']
})
export class PasswordResetComponent implements OnInit {

  form: FormGroup;
  submitted: boolean = false;
  emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  constructor(private formBuilder: FormBuilder, private http: HttpService, private router: Router) {
    this.form = this.formBuilder.group({
      email: ["", Validators.compose([Validators.required, Validators.pattern(this.emailRegex)])],
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
          this.form.get('email').setErrors({ "error": "Incorrect email address" });
          error('Incorrect email address');

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
          success('The Email to Reset your Password has been Sent Successfully');
          this.router.navigate(['/login']);
          
        }, (error) => {          
          this.submitted = false;
          warn('You must confirm your email address from your registration before confirming your password');
          // console.log(error);
          if (error.status !== 500) {            
            this.form.get('email').setErrors({ 'auth': 'Incorrect email' })
          }
        })
      } catch (e) {        
        this.submitted = false;
        warn('Please enter valid Email');
        this.form.get('email').setErrors({ 'auth': 'Incorrect email' })
      } finally {

      }
    }
  }


}

