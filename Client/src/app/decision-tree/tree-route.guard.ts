/*
 This class serves to guard routes and modules from unauthorized access
 CanActivate method of this class is used in the app routing module to determine if user has access before letting in
 */
import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot, RouterStateSnapshot, CanActivate, Router
} from '@angular/router';
import { ProfileManager } from '../services/profile-manager';
import { EventsService } from '../services/events-service';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';


@Injectable()
export class TreeAuthGuard implements CanActivate {
  returnURL: String = '';
  constructor(private events: EventsService,
    private router: Router, private profileManager: ProfileManager) {
    this.events.on('logout', _ => {
      this.profileManager.profile = undefined;
      localStorage.removeItem('user');
      this.router.navigate(['/login']);
    });

  }
  get userIsAdmin(): Boolean {
    return  (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
      && this.profileManager.profile.roles.indexOf('Admin') > -1);
  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    this.returnURL = state.url;
    console.log(route.url);
    return of(this.userIsAdmin).
      pipe(map(e => {
        if (e) {
          return true;
        } else {
          this.router.navigate(['/main/private']);
          return false;
        }
      }), catchError((e) => {
        this.router.navigate(['/main/private']);
        return of(false);
      }));
  }
}
