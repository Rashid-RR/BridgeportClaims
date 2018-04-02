/*
 This class serves to guard routes and modules from unauthorized access
 CanActivate method of this class is used in the app routing module to determine if user has access before letting in
 */
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, ActivatedRoute, RouterStateSnapshot, CanActivate, CanActivateChild, Resolve, Router } from "@angular/router";

import { HttpService } from "../services/http-service";
import { UserProfile } from "../models/profile";
import { ProfileManager } from "../services/profile-manager";
import { EventsService } from "../services/events-service";
import { Observable } from "rxjs/Observable"
import 'rxjs/add/operator/first' // in imports

@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild, Resolve<UserProfile>{

  returnURL: String = '';
  constructor(private activeRoute: ActivatedRoute, private events: EventsService, private router: Router, private profileManager: ProfileManager) {
    this.events.on("logout", immediately => {
      this.profileManager.profile = undefined;
      localStorage.removeItem("user");
      this.router.navigate(['/login']);
    });

  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    this.returnURL = state.url;
    return this.isLoggedIn.map(e => {
      if (e) {
        return true;
      } else {
        this.router.navigate(['/login'], { queryParams: { 'returnURL': this.returnURL } });
        return false;
      }
    }).catch((e) => {
      this.router.navigate(['/login'], { queryParams: { 'returnURL': this.returnURL } });
      return Observable.of(false);
    });
  }
  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
    this.returnURL = state.url;
    let route = childRoute.url[0] || childRoute.parent.url[0];
    var user = localStorage.getItem("user");
    if (user === null || user.length == 0) {
      this.router.navigate(['/login'], { queryParams: { 'returnURL': this.returnURL } });
      this.events.broadcast("logout", true);
      return Observable.of(false);
    }
    try {
      let us = JSON.parse(user);
      if (route.path == 'users' || route.path == 'reports' || route.path == 'fileupload') {
        var allowed = (us.roles && (us.roles instanceof Array) && us.roles.indexOf('Admin') > -1);
        return Observable.of(allowed)
      } else if (route.path == 'unindexed-images' || route.path == 'indexing') {
        var allowed = (us.roles && (us.roles instanceof Array) && (us.roles.indexOf('Admin') > -1 || us.roles.indexOf('Indexer') > -1));
        return Observable.of(allowed)
      } else {
        return this.profileManager.userLoaded(us.email).single();
      }
    } catch (error) {
      let us = JSON.parse(user);
      console.log(error); 
      if (state.url.indexOf('/main/indexing')>-1) {
        var allowed = (us.roles && (us.roles instanceof Array) && (us.roles.indexOf('Admin') > -1 || us.roles.indexOf('Indexer') > -1));
        return Observable.of(allowed)
      }else{
        return Observable.of(false);
      }
    }
  }
  get isLoggedIn(): Observable<boolean> {
    var user = localStorage.getItem("user");
    if (user === null || user.length == 0) { return Observable.of(false); }
    try {
      let us = JSON.parse(user);
      //console.log(this.profileManager.userProfile(us.userName));
      return this.profileManager.userInfo(us.email).single().map(res => {
        if (res.email && !this.profileManager.profile) {
          this.profileManager.profile = res;
        }
        return res.email ? true : false;
      })
    } catch (error) {
      return Observable.of(false);
    }
  }

  get isSideBarOpen(): boolean {
    var sidebar = localStorage.getItem("sidebarOpen");
    if (sidebar === null) { return true }
    console.log("gets here..");
    try {
      return Boolean(sidebar);
    } catch (error) {
      console.log(error);
      return true;
    }
  }
  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<UserProfile> {
    let user = this.profileManager.User;
    user.subscribe(profile => {
      //console.log(profile);
    }, error => {
      console.log(error);
    });
    //return user.map(profile=>{console.log(profile);return profile});
    return this.profileManager.User;
  }

  hasRights(module) {

  }
}