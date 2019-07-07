import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpService } from '../../services/services.barrel';
import { AuthGuard } from '../../services/auth.guard';
import { ProfileManager } from '../../services/profile-manager';
import { UserProfile } from '../../models/profile';
import { EventsService } from '../../services/events-service';
import { ToastrService } from 'ngx-toastr';
import { Location } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  submitted = false;
  emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  returnURL: String;
  constructor(
    private formBuilder: FormBuilder,
    private http: HttpService,
    private router: Router, private events: EventsService,
    private profileManager: ProfileManager,
    private authGuard: AuthGuard,
    private _location: Location,
    private toast: ToastrService
  ) {
    this.form = this.formBuilder.group({
      email: ['', Validators.compose([Validators.required, Validators.pattern(this.emailRegex)])],
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
        this.http.login('userName=' + this.form.get('email').value + '&password=' + this.form.get('password').value + '&rememberMe=' + this.form.get('rememberMe').value + '&grant_type=password', { 'Content-Type': 'x-www-form-urlencoded' }).subscribe(data => {
          this.http.setAuth(data.access_token);
          localStorage.setItem('user', JSON.stringify(data));
          this.http.profile().subscribe(user => {
            user.access_token = data.access_token;
            localStorage.setItem('user', JSON.stringify(user));
            this.profileManager.profile = new UserProfile(user.id || user.email, user.email, user.firstName, user.lastName, user.email, user.email, null, data.createdOn, user.roles);
            this.profileManager.setProfile(new UserProfile(user.id || user.email, user.email, user.firstName, user.lastName, user.email, user.email, null, data.createdOn, user.roles));
            this.profileManager.profileChanged.next();
            if (this.returnURL) {
              const url = this.returnURL.split('?');
              const p = {};
              if (url[1]) {
                const params = url[1].split('&');
                if (params.length > 0) {
                  params.forEach(pr => {
                    const par = pr.split('=');
                    p[par[0]] = par[1];
                  });
                }
              }
              if (user.roles.indexOf('Client') > -1) {
                this.clientLogin({ queryParams: p });
              } else {
                this.router.navigate([url[0]], { queryParams: p });
              }
            } else {
              if (user.roles.indexOf('Client') > -1) {
                this.clientLogin();
              } else {
                this.router.navigate(['/main/private']);
              }
            }
            this.events.broadcast('login', true);
            this.toast.success('Welcome back');
            this.events.broadcast('loadHistory', []);
          }, err => null);
        }, (errors) => {
          this.submitted = false;
          const err = errors.error;
          this.form.get('password').setErrors({ 'auth': err.error_description });
          this.router.navigate(['/login']);
          if (err.error_description === undefined) {
            this.toast.error('An internal error has occurred. A system administrator is working to fix it A.S.A.P.');
          } else {
            this.toast.error(err.error_description);
          }
        });
      } catch (e) {
        this.submitted = false;
        this.form.get('password').setErrors({ 'auth': 'Incorrect login or password' });
        this.toast.error('Incorrect login or password');
      } finally {

      }
    } else {
      this.toast.error('Error in fields. Please correct to proceed.');
    }
  }
  clientLogin(params?: any) {
    this.router.navigate(['/main/referral'], params);
  }
  ngOnInit() {
    const user = localStorage.getItem('user');
    if (user !== null) {
      this._location.back();
    }
    this.router.routerState.root.queryParams.subscribe(params => {
      if (params['returnURL']) {
        this.returnURL = decodeURIComponent(params['returnURL']);
      }
    });
  }
}
