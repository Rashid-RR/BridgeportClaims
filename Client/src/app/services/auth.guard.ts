/*
 This class serves to guard routes and modules from unauthorized access
 CanActivate method of this class is used in the app routing module to determine if user has access before letting in
 */
import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot, ActivatedRoute, RouterStateSnapshot,
  CanActivate, CanActivateChild, Resolve, Router
} from '@angular/router';
import { UserProfile } from '../models/profile';
import { ProfileManager } from './profile-manager';
import { EventsService } from './events-service';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';


@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild, Resolve<UserProfile> {
  returnURL: String = '';
  constructor(private activeRoute: ActivatedRoute, private events: EventsService,
    private router: Router, private profileManager: ProfileManager) {
    this.events.on('logout', _immediately => {
      this.profileManager.profile = undefined;
      localStorage.removeItem('user');
      this.router.navigate(['/login']);
    });

  }
  canActivate(_route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    this.returnURL = state.url;
    return this.isLoggedIn.
      pipe(map(e => {
        if (e) {
          return true;
        } else {
          this.router.navigate(['/login'], { queryParams: { 'returnURL': this.returnURL } });
          return false;
        }
      }), catchError((_e) => {
        this.router.navigate(['/login'], { queryParams: { 'returnURL': this.returnURL } });
        return of(false);
      }));
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    this.returnURL = state.url;
    const route = childRoute.url[0] || childRoute.parent.url[0];
    const user = localStorage.getItem('user');
    if (user === null || user.length === 0) {
      this.router.navigate(['/login'], { queryParams: { 'returnURL': this.returnURL } });
      this.events.broadcast('logout', true);
      return of(false);
    }
    try {
      const us = JSON.parse(user);
      if (['private', 'referral', 'profile'].indexOf(route.path) === -1 && (us.roles &&
        (us.roles instanceof Array) && us.roles.indexOf('Client') > -1)) {
        return of(false);
      } else if (route.path === 'users' || route.path === 'fileupload') {
        const allowed = (us.roles && (us.roles instanceof Array) && us.roles.indexOf('Admin') > -1);
        return of(allowed);
      } else if (state.url.indexOf('/main/reports') > -1 &&
        (state.url.indexOf('/main/reports/skipped-payment') === -1 &&
        state.url.indexOf('/main/reports/shortpay') === -1 &&
        state.url.indexOf('/main/reports/claims-data') === -1 &&
        state.url.indexOf('/main/reports/address-edit') === -1 &&
        state.url.indexOf('/main/reports/invoices') === -1 &&
        state.url.indexOf('/main/reports/list') === -1)) {
        const allowed = state.url.indexOf('/main/reports/collection-bonus') > -1 ? (us.roles && us.roles instanceof Array
          && us.roles.indexOf('User') > -1) : (us.roles && us.roles instanceof Array && us.roles.indexOf('Admin') > -1);
        return of(allowed);
      } else if (route.path === 'unindexed-images' || route.path === 'indexing') {
        const allowed = (us.roles && (us.roles instanceof Array) && (us.roles.indexOf('Admin') > -1 || us.roles.indexOf('Indexer') > -1));
        return of(allowed);
      } else {
        return this.profileManager.userLoaded(us.email);
      }

    } catch (error) {
      const us = JSON.parse(user);
      if (state.url.indexOf('/main/indexing') > -1) {
        const allowed = (us.roles && (us.roles instanceof Array) && (us.roles.indexOf('Admin') > -1 || us.roles.indexOf('Indexer') > -1));
        return of(allowed);
      } else {
        return of(false);
      }
    }
  }
  get isLoggedIn(): Observable<boolean> {
    const user = localStorage.getItem('user');
    if (user === null || user.length === 0) {
      return of(false);
    }
    try {
      const us = JSON.parse(user);
      return this.profileManager.userInfo(us.email).pipe(map(res => {
        if (res.email && !this.profileManager.profile) {
          this.profileManager.profile = res;
        }
        return res.email ? true : false;
      }));
    } catch (error) {
      return of(false);
    }
  }

  get isSideBarOpen(): boolean {
    const sidebar = localStorage.getItem('sidebarOpen');
    if (sidebar === null) { return true; }

    try {
      return Boolean(sidebar);
    } catch (error) {
      return true;
    }
  }
  resolve(_route: ActivatedRouteSnapshot, _state: RouterStateSnapshot): Observable<UserProfile> {
    const user = this.profileManager.User;
    user.subscribe(_profile => {
    }, _error => {

    });
    // return user.map(profile=>{return profile});
    return this.profileManager.User;
  }

  hasRights(_module: any) {
  }
}
