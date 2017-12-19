import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable, Inject, NgZone } from '@angular/core';
import { EventsService } from './events-service';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import * as Rx from 'rxjs/Rx';
import * as Immutable from 'immutable';
import { DocumentItem } from "../models/document"
declare var $: any;

@Injectable()
export class SignalRService implements Resolve<boolean> {

    // signalR connection reference
    connection: any;
    public connected: boolean = false;
    // signalR proxy reference 
    private proxies: { id: string, value: any }[] = [];
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

    connect(hub: string,callback:(hub:any,name:string)=>void) {
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
            callback(proxy.value, hub);
        }else{
            callback(proxy.value, hub);
        }
    }

    getProxy(hub: string) {
        const proxy = this.proxies.find(p => p.id == hub);
        return proxy;
    }
}
