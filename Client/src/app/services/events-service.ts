// event-service.ts
/**
 * Class to facilitate event based communication within the app by availing subjects/topics and subscribers
 */

import {Injectable} from "@angular/core";
import * as Rx from "rxjs/Rx";


@Injectable()
export class EventsService {
  listeners: Object;
  events: Rx.Observable<any>;
  eventsSubject: Rx.Subject<any>;
  browserNotification:any;
  constructor() {
    this.listeners = {};
    this.eventsSubject = new Rx.Subject();
    
    this.events = Rx.Observable.from(this.eventsSubject);

    this.events.subscribe(
      ({name, args}) => {
        if (this.listeners[name]) {
          for (let listener of this.listeners[name]) {
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

  broadcast(name, ...args) {
    this.eventsSubject.next({
      name,
      args
    });
  }
}
