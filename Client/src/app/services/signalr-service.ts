import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable, Inject, NgZone } from '@angular/core';
import { EventsService } from './events-service';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import * as Rx from 'rxjs/Rx';

declare var $: any;

@Injectable()
export class SignalRService implements Resolve<boolean> {

    // signalR connection reference
    private connection: any;
    public connected: boolean = false;
    // signalR proxy reference
    private proxies: { id: string, value: any }[] = [];
    messages: { msgFrom: string, msg: string }[] = [];
    loading = false;
    constructor(private events: EventsService, private _ngZone: NgZone) {
        const fileref = document.createElement('script');
        fileref.setAttribute('type', 'text/javascript');
        fileref.setAttribute('src', 'signalr/hubs');
        $('body').append(fileref);
        this.connection = $.connection;
    }
    resolve(route: ActivatedRouteSnapshot): Promise<boolean> {
        return new Promise((resolve, reject) => {
            if (this.connected) {
                resolve(true);
            } else {
                this.connection.hub.start().done((r) => {
                    //console.log("Done connecting...");
                    this.connected = true;
                    this.events.broadcast('start-listening-to-signalr', true);
                    resolve(true);
                }).fail(err => {
                    //console.log("Error connecting...");
                    resolve(false);
                });
            }
        });
    }

    connect(hub: string) {
        if (!this.connected) {
            this.connection.hub.start().done((r) => {
                //console.log("Done connecting...");
                this.connected = true;
                this.events.broadcast('start-listening-to-signalr', true);
            }).fail(err => {
                //console.log("Error connecting...");
            });
        }
        let proxy = this.proxies.find(p => p.id == hub);
        //console.log(this.connection[hub]);
        if (!proxy || !proxy.value) {
            proxy = { id: hub, value: this.connection[hub] };
            this.proxies.push(proxy);
            this.start(proxy.value);
        }
    }

    start(hub: any) {
        //console.log(this.connected, hub.client);
        hub.client.receiveMessage = (msgFrom, msg) => this.onMessageReceived(msgFrom, msg);        
    }
    private onMessageReceived(msgFrom: string, msg: string) {
        //console.log('New message received from ' + msgFrom, msg);
        this._ngZone.run(() => {
            this.messages.push({ msgFrom: msgFrom, msg: msg });
        });
    }

    getProxy(hub: string) {
        const proxy = this.proxies.find(p => p.id == hub);
        return proxy;
    }
}
