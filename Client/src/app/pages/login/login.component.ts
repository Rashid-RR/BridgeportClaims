import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { HttpService } from "../../services/http-service";
import { AuthGuard } from "../../services/auth.guard";
import { ProfileManager } from "../../services/profile-manager";
import { UserProfile } from "../../models/profile";
import { EventsService } from "../../services/events-service";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import {Location} from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  submitted: boolean = false;
  emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  returnURL:String;
  constructor(
    private formBuilder: FormBuilder,
    private http: HttpService,
    private router: Router, private events: EventsService,
    private profileManager: ProfileManager,
    private authGuard: AuthGuard,
    private _location:Location,
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
        this.http.login('userName=' + this.form.get('email').value + '&password=' + this.form.get('password').value + "&grant_type=password", { 'Content-Type': 'x-www-form-urlencoded' }).subscribe(res => {
          let data = res.json();
          this.http.setAuth(data.access_token);
          localStorage.setItem("user", JSON.stringify(data));            
          this.http.profile().map(res => res.json()).subscribe(res => {
            let user = res;
            res.access_token = data.access_token;
            localStorage.setItem("user", JSON.stringify(res));
            this.profileManager.profile = new UserProfile(res.id || res.email, res.email, res.firstName, res.lastName, res.email, res.email, null, data.createdOn, res.roles);
            this.profileManager.setProfile(new UserProfile(res.id || res.email, res.email, res.firstName, res.lastName, res.email, res.email, null, data.createdOn, res.roles));
           if(this.returnURL){
             let url = this.returnURL.split('?');
             let p={};
             if(url[1]){
              console.log(url[1]);
              let params = url[1].split('&');
              if(params.length>0){
                console.log(params);
                params.forEach(pr=>{
                  let par = pr.split('=');
                  console.log(pr,par);
                    p[par[0]]=par[1];
                })
              }
            }
             console.log(p)
             this.router.navigate([url[0]],{queryParams:p});
           }else{
            this.router.navigate(['/main/private']);
           }
            this.events.broadcast('login', true);
            this.toast.success('Welcome back');
            this.events.broadcast("loadHistory",[]);
          }, err => console.log(err))
        }, (requestError) => {
          this.submitted = false;
          let err = requestError.json();
          this.form.get('password').setErrors({ 'auth': err.error_description })

          this.router.navigate(['/login']);
          if (err.error_description === undefined) {
            this.toast.error("An internal error has occurred. A system administrator is working to fix it A.S.A.P.");
          } else {
            this.toast.error(err.error_description);
          }
        })
      } catch (e) {
        this.submitted = false;
        this.form.get('password').setErrors({ 'auth': 'Incorrect login or password' })
        this.toast.error('Incorrect login or password');
      } finally {

      }
    } else {
      this.toast.error('Error in fields. Please correct to proceed!');
    }
  }
  ngOnInit() {
    var user = localStorage.getItem("user");  
     if(user!==null){
      this._location.back();
    }
    this.router.routerState.root.queryParams.subscribe(params => {
      if(params['returnURL']){
        this.returnURL = decodeURIComponent(params['returnURL']);
        console.log(this.returnURL);
      }
       

    });
  }


}
