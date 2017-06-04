import {Component, OnInit} from "@angular/core";
import {HttpService} from "../../services/http-service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";

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
      email: ["", Validators.compose([Validators.required,Validators.pattern(this.emailRegex)])],
    });
  }

  ngOnInit() {

  }

  resetPassword() {
     if(this.form.valid){
      this.submitted = true;
       this.http.changepassword({email:this.form.get('email').value})
        .subscribe(res=>{
            this.submitted = false;
        },error=>{
              this.form.get('email').setErrors({"error":"Incorrect email address"});
              this.submitted = false;
        });
    }

  }

  register() {
    this.router.navigate(['/register']);          
  }
  login() {
    this.router.navigate(['/login']);
  }
  submit(){
    this.submitted = true;
    if (this.form.valid) {
      try {
        this.http.passwordreset(this.form.value).subscribe(res => {
           this.router.navigate(['/main/login']); 
        }, (error) => {
          if (error.status !== 500) {
            this.form.get('email').setErrors({'auth': 'Incorrect email'})
          }
        })
      } catch (e) {
        this.form.get('email').setErrors({'auth': 'Incorrect email'})
      } finally {

      }
    }
  } 


}

