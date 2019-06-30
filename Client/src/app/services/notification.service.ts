import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpService } from './http-service';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private notifications$ = new BehaviorSubject([]);
  private notificationCount$ = new BehaviorSubject(0);

  constructor(
    private http: HttpService,
  ) {
      this.notifications$.subscribe((notifications: any[]) => {
        this.updateNotificationCount(notifications.length);
      });
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
