import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {HttpService} from "../../services/http-service";
import {ProfileManager} from "../../services/profile-manager";
import {UserProfile} from "../../models/profile";
import {EventsService} from "../../services/events-service";
import {warn,success,error} from "../../models/notification"

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  submitted: boolean = false;
  emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
   constructor(private formBuilder: FormBuilder, private http: HttpService, private router: Router, private events: EventsService,private profileManager:ProfileManager) {
    this.form = this.formBuilder.group({
      email: ['', Validators.compose([Validators.pattern(this.emailRegex)])],
      password: ['', Validators.compose([Validators.required])],
      grant_type: ['password'],
      rememberMe: [false],

    });
  }

  reset() {
    this.router.navigate(['/recover-lost-password']);          
  }
  register() {
    this.router.navigate(['/register']);          
  }
  login() {
    this.submitted = true;
    if (this.form.valid) {
      try {
        this.http.login('userName='+this.form.get('email').value+'&password='+this.form.get('password').value+"&grant_type=password",{'Content-Type':'x-www-form-urlencoded'}).subscribe(res => {
            let data = res.json(); 
            this.events.broadcast('login', true);
            this.http.setAuth(data.access_token);   
            this.http.profile().map(res=>res.json()).subscribe(res=>{
                this.profileManager.profile = new UserProfile(data.id || res.email,res.email,res.firstName,res.lastName,res.email,res.email,null,data.createdOn);
                this.profileManager.setProfile(new UserProfile(data.id || res.email,res.email,res.firstName,res.lastName,res.email,res.email,null,data.createdOn));
                let user = res;
                res.access_token = data.access_token;
                localStorage.setItem("user", JSON.stringify(res));
                console.log(this.profileManager.profile)
                this.router.navigate(['/main/private']);
                success('Welcome back');
            },err=>console.log(err))
        }, (error) => {
         // if (error.status !== 500) {
           let err = error.json();
            this.form.get('password').setErrors({'auth': 'Incorrect login or password'})
            error( err.error_description);
         // }
        })
      } catch (e) {
        this.form.get('password').setErrors({'auth': 'Incorrect login or password'})
        error( 'Incorrect login or password');
      } finally {

      }
    }else{
       error('Error in fields. Please correct to proceed!');
    }
  }
  ngOnInit() {

  }


}
