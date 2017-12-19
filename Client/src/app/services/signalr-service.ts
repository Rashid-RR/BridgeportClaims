import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Injectable, Inject, NgZone } from '@angular/core';
import { EventsService } from './events-service';
import { DocumentManagerService } from './document-manager.service';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import * as Rx from 'rxjs/Rx';
import * as Immutable from 'immutable';
import { DocumentItem } from "../models/document"
declare var $: any;

@Injectable()
export class SignalRService implements Resolve<boolean> {

    // signalR connection reference
    private connection: any;
    public connected: boolean = false;
    // signalR proxy reference
    private proxies: { id: string, value: any }[] = [];
    messages: { msgFrom: string, msg: string }[] = [];
    documents: DocumentItem[] = [];
    loading = false;
    constructor(private events: EventsService,private documentManager:DocumentManagerService, private _ngZone: NgZone) {
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
            this.start(proxy.value, hub);
        }
    }

    start(hub: any, hubname: string) {
        switch (hubname) {
            case 'documentsHub':
                hub.client.newDocument = (...args) => this.onNewDocument(args);
                break;
            default:
                hub.client.receiveMessage = (msgFrom, msg) => this.onMessageReceived(msgFrom, msg);
                break;
        }

    }
    get documentItems(): Array<DocumentItem> {
        return this.documents;
    }
    onNewDocument(args: Array<any>) {
        let doc: DocumentItem = {
            documentId: args[0], fileName: args[1], fileSize: args[2],
            creationTimeLocal: args[3], lastAccessTimeLocal: args[4],
            lastWriteTimeLocal: args[5], extension: '', fileUrl: '', fullFilePath: ''
        }
        console.log(this.documentManager.documents);
        this._ngZone.run(() => {
            this.documents.push(doc);
            this.documentManager.documents=this.documentManager.documents.set(args[0],doc);
            console.log(this.documentManager.documents);
        });
    }
    private onMessageReceived(msgFrom: string, msg: string) {
        console.log('New message received from ' + msgFrom, msg);
        this._ngZone.run(() => {
            this.messages.push({ msgFrom: msgFrom, msg: msg });
        });
    }

    getProxy(hub: string) {
        const proxy = this.proxies.find(p => p.id == hub);
        return proxy;
    }
}
