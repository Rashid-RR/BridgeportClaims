import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {HttpService} from "../../services/http-service";
import {ProfileManager} from "../../services/profile-manager";
import {UserProfile} from "../../models/profile";
import {EventsService} from "../../services/events-service";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  submitted: boolean = false;
  emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  constructor(
    private formBuilder: FormBuilder, 
    private http: HttpService, 
    private router: Router, private events: EventsService,
    private profileManager: ProfileManager,
    private toast: ToastsManager
  ) {    
    this.form = this.formBuilder.group({
      email: ['', Validators.compose([Validators.pattern(this.emailRegex)])],
      password: ['', Validators.compose([Validators.required])],
      grant_type: ['password'],
      rememberMe: [false]
    });
  }

  reset() {
    this.router.navigate(['/recover-lost-password']);          
  }
  register() {
    this.router.navigate(['/register']);          
  }
  login() {    
    if (this.form.valid) {
      try {
        this.submitted = true;
        this.http.login('userName='+this.form.get('email').value+'&password='+this.form.get('password').value+"&grant_type=password",{'Content-Type':'x-www-form-urlencoded'}).subscribe(res => {
            let data = res.json(); 

            this.events.broadcast('login', true);
            this.http.setAuth(data.access_token);   
            this.http.profile().map(res=>res.json()).subscribe(res=>{

                this.profileManager.profile = new UserProfile(res.id || res.email,res.email,res.firstName,res.lastName,res.email,res.email,null,data.createdOn,res.roles);
                this.profileManager.setProfile(new UserProfile(res.id || res.email,res.email,res.firstName,res.lastName,res.email,res.email,null,data.createdOn,res.roles));
                let user = res;
                res.access_token = data.access_token;
                localStorage.setItem("user", JSON.stringify(res));
                this.router.navigate(['/main/private']);
                this.toast.success('Welcome back');
            },err=>console.log(err))
        }, (requestError) => {
            this.submitted = false;
           this.submitted = false;
           let err = requestError.json();
            this.form.get('password').setErrors({'auth': err.error_description})
            this.toast.error( err.error_description);
            this.router.navigate(['/login']);         
        })
      } catch (e) {
        this.submitted = false;
        this.form.get('password').setErrors({'auth': 'Incorrect login or password'})
        this.toast.error( 'Incorrect login or password');
      } finally {

      }
    }else{
       this.toast.error('Error in fields. Please correct to proceed!');
    }
  }
  ngOnInit() {

  }


}
