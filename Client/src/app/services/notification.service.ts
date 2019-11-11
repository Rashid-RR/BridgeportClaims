import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpService } from './http-service';
import { EventsService } from './events-service';
import { ProfileManager } from './profile-manager';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private notifications$ = new BehaviorSubject([]);
  private notificationCount$ = new BehaviorSubject(0);

  constructor(
    private http: HttpService,
    private profileManager: ProfileManager,
    private events: EventsService,
  ) {
    this.notifications$.subscribe((notifications: any[]) => {
      this.updateNotificationCount(notifications.length);
    });
    if (this.profileManager.profile) {
      /* if (this.profileManager.profile.roles[0] === 'Admin') {
      this.fetchNotifications();
      } */
    }
    this.events.on('login', () => {
      if (this.profileManager.profile) {
        if (this.profileManager.profile.roles[0] === 'Admin') {
        this.fetchNotifications();
        }
      }
    });
  }
  fetchNotifications() {
    this.http.getNotifications()
      .subscribe((result: any) => {
        this.notifications$.next(result);
      });
  }

  getNotification() {
    return this.notificationCount$;
  }

  updateNotificationCount(countParam: number) {
    this.notificationCount$.next(countParam);
  }
  updateNotifications(countParam: any[]) {
    this.notifications$.next(countParam);
  }

}
