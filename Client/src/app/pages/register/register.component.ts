import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {HttpService} from "../../services/http-service";
import {warn,success} from "../../models/notification"


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  form: FormGroup;
  submitted: boolean = false;
  registered: boolean = false;
  emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  constructor(private formBuilder: FormBuilder, private http: HttpService, private router: Router) {
    this.form = this.formBuilder.group({
      firstname: ['', Validators.compose([Validators.required])],
      lastname: ['', Validators.compose([Validators.required])],
      Email: ['', Validators.compose([Validators.pattern(this.emailRegex)])],
      Password: ["", Validators.compose([Validators.required])],
      ConfirmPassword: ["", Validators.compose([Validators.required])]
    });
  }

  ngOnInit() {

  }
  login() {
    this.router.navigate(['/login']);          
  }
  register() {
    console.log(this.form.value);
    if (this.form.valid && this.form.get('Password').value !== this.form.get('ConfirmPassword').value) {
      this.form.get('ConfirmPassword').setErrors({"unmatched": "Repeat password does not match password"});
      warn( 'Password and Confirmed Password did not match password');
    }
    if (this.form.valid) {
      this.submitted = true;
      try {
        this.http.register(this.form.value).subscribe(res => {
            console.log("Successful registration");
            success("You have been signup successfully");
            success("Please go check your email – you’ll need to confirm your email address before you login.");            
            this.registered = true
            this.router.navigate(['/login']);
            // this.router.navigate(['/logon']);
            //console.log(res.json());
            
        },error => {
          this.submitted = false;
            let err = error.json();            
            warn( err.Message);
        })
      } catch (e) {
          this.submitted = false;
      } finally {

      }
    }else{
       warn('Error in fields. Please correct to proceed!');
    }

  }

}
