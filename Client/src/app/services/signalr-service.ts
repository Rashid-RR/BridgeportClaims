import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable, Inject, NgZone } from '@angular/core';
import { EventsService } from './events-service';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import * as Rx from 'rxjs/Rx';
import * as Immutable from 'immutable';
import { DocumentItem } from "../models/document"
import { setTimeout } from 'core-js/library/web/timers';
declare var $: any;

@Injectable()
export class SignalRService {

    // signalR connection reference
    connection: any;
    public connected: boolean = false;
    // signalR proxy reference 
    private proxies: { id: string, value: any }[] = [];
    loading = false;
    documentProxy: any
    constructor(private events: EventsService, private _ngZone: NgZone) {
        /* const fileref = document.createElement('script');
        fileref.setAttribute('type', 'text/javascript');
        fileref.setAttribute('src', 'signalr/hubs');
        $('body').append(fileref); */
        this.connection = $.connection;
        this.startConnection();

    }
    startConnection() {
        this.documentProxy = $.connection.documentsHub;
        this.connection.hub.start().done();
        this.connected = true;
        this.setUpDocumentProxy();

    }

    setUpDocumentProxy() {
        this.documentProxy.client.newDocument = (...args) => {
            this.onNewDocument(args);
        }
        this.documentProxy.client.receiveMessage = (msgFrom, msg) => {
            this.onMessageReceived(msgFrom, msg);
        }
    }
    private onMessageReceived(msgFrom: string, msg: string) {
        console.log('New message received from ' + msgFrom, msg);
        this._ngZone.run(() => {
            this.events.broadcast("new-message", { msgFrom: msgFrom, msg: msg });
        });
    }
    onNewDocument(args: Array<any>) {
        let doc: DocumentItem = {
            documentId: args[0], fileName: args[1], fileSize: args[3],
            creationTimeLocal: args[4], lastAccessTimeLocal: args[5],
            lastWriteTimeLocal: args[6], extension: args[2], fileUrl: args[8], fullFilePath: args[7]
        }
        this._ngZone.run(() => {
            this.events.broadcast("new-document", doc);
        });
    }

    getProxy(hub: string) {
        const proxy = this.proxies.find(p => p.id == hub);
        return proxy;
    }
}
