// event-service.ts
/**
 * Class to facilitate event based communication within the app by availing subjects/topics and subscribers
 */

import {Injectable} from '@angular/core';
import {from,Subject,Observable} from 'rxjs';


@Injectable()
export class EventsService {
  listeners: Object;
  events: Observable<any>;
  eventsSubject: Subject<any>;
  browserNotification: any;
  constructor() {
    this.listeners = {};
    this.eventsSubject = new Subject();

    this.events = from(this.eventsSubject);

    this.events.subscribe(
      ({name, args}) => {
        if (this.listeners[name]) {
          for (const listener of this.listeners[name]) {
            listener(...args);
          }
        }
      });
  }
  on(name, listener) {
    if (!this.listeners[name]) {
      this.listeners[name] = [];
    }

    this.listeners[name].push(listener);
  }
  flash(name) {
    this.listeners[name] = [];
  }

  broadcast(name, ...args) {
    this.eventsSubject.next({
      name,
      args
    });
  }
}
