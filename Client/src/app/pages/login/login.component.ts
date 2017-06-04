import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {HttpService} from "../../services/http-service";
import {ProfileManager} from "../../services/profile-manager";
import {EventsService} from "../../services/events-service";

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
        this.http.login('username='+this.form.get('email').value+'&password='+this.form.get('password').value+"&grant_type=password",{'Content-Type':'x-www-form-urlencoded'}).subscribe(res => {
          let data = res.json(); 
          localStorage.setItem("user", JSON.stringify(data));
          this.router.navigate(['/main/private']);
          this.events.broadcast('login', true);
          this.events.broadcast('profile', res.json());
          this.http.setAuth(data.access_token);
        }, (error) => {
          if (error.status !== 500) {
            this.form.get('password').setErrors({'auth': 'Incorrect login or password'})
          }
        })
      } catch (e) {
        this.form.get('password').setErrors({'auth': 'Incorrect login or password'})
      } finally {

      }
    }
  }
  ngOnInit() {

  }


}
