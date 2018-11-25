
import * as Immutable from 'immutable';
import {Observable} from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import {UserProfile} from '../models/profile';
import {Injectable} from '@angular/core';
import {HttpService} from './http-service';
import {EventsService} from './events-service';
import {Router} from '@angular/router';

@Injectable()
export class ProfileManager {
  profileChanged = new Subject<UserProfile>();
  private userCache: Immutable.OrderedMap<String, UserProfile> = Immutable.OrderedMap<String, UserProfile>();
  profile: UserProfile = null;

  constructor(private router: Router, private http: HttpService, private events: EventsService) {
      this.events.on('profile', (profile) => {
        this.profile = profile as UserProfile;
      });
      this.events.on('logout', (v) => {
        this.clearUsers();
    });
  }
  userInfo(userId: String): Observable<UserProfile> {
    const v = this.userCache.get(userId);
    if (v) {
      return Observable.of(v);
    } else {
      const s = this.http.userFromId(userId);
      s.subscribe(res => {
        const u = res as UserProfile;
        this.userCache = this.userCache.set(u.email, u);
      }, err => {
        const error = err.error;
      });
      return s.map(res => res as UserProfile);
    }
  }
  userLoaded(userId: String): Observable<boolean> {
    const v = this.userCache.get(userId);
    if (v) {
      return Observable.of(v.email ? true : false);
    } else {
      const s = this.http.userFromId(userId);
      s.subscribe(res => {
        const u = res as UserProfile;
        this.userCache = this.userCache.set(u.email, u);
      }, err => {
        const error = err.error;
      });
      return s.map(res => {
        return res['email'] || (res && res.email) ? true : false;
      });
    }
  }
  setProfile(u: UserProfile) {
    const profile = new UserProfile(u.id || u.userName, u.login  || u.userName, u.firstName  || u.userName, u.lastName  || u.userName, u.email  || u.userName, u.userName, u.avatarUrl, u.createdOn);
    this.userCache = this.userCache.set(profile.email, profile);
    this.profileChanged.next(profile);
  }
  userProfile(userId: String) {
      return this.userCache.get(userId);
  }

  clearUsers() {
    this.userCache = Immutable.OrderedMap<String, UserProfile>();
    this.profileChanged.next(null);
  }
  get User(): Observable<UserProfile> {
    const user = localStorage.getItem('user');
    return Observable.create((observer) => {
        if (user !== null && user.length > 0) {
          try {
            const us = JSON.parse(user);
            // this.eventservice.broadcast('profile', us);
            this.userInfo(us.id).single().subscribe( res => {
              this.profile = res;
              this.events.broadcast('profile', res);
              observer.next(res);
              this.profileChanged.next(res);
            }, (error) => {
                observer.error();
            });
          } catch (error) {

          }
        } else {
          observer.error();
        }
    });
  }
}
