import { Resolve } from '@angular/router';
import { Injectable, Inject } from "@angular/core";
import { Subject } from "rxjs/Subject";
import { Observable } from "rxjs/Observable";
import * as Rx from "rxjs/Rx";
declare var $: any;

@Injectable()
export class SignalRService {

    //signalR connection reference
    private connection: any;
    //signalR proxy reference
    private proxies: { id: string, value: any }[] = [];
    messages: { msgFrom: string, msg: string }[] = [];
    loading:boolean=false;
    constructor() {
        var fileref = document.createElement('script')
        fileref.setAttribute("type", "text/javascript")
        fileref.setAttribute("src", 'signalr/hubs');
        $('body').append(fileref);
        this.connection = $.connection;
        this.connection.hub.start().done(() => {

        });
    }

    connect(hub: string) {
        let proxy = this.proxies.find(p => p.id == hub);
        console.log($.connection.documentsHub);
        if (!proxy || !proxy.value) {
            proxy = { id: hub, value: this.connection[hub] };
            this.proxies.push(proxy);
            this.start(proxy.value);
        }
    }

    start(hub: any) {
        hub.client.receiveMessage = (msgFrom, msg) => this.onMessageReceived(msgFrom, msg);
        this.connection.hub.start().then(t => { 
        });
    }
    private onMessageReceived(msgFrom: string, msg: string) {
        this.messages.push({msgFrom:msgFrom,msg:msg});
        console.log('New message received from ' + msgFrom,msg);
    }

    getProxy(hub: string) {
        let proxy = this.proxies.find(p => p.id == hub);
        return proxy
    }
    //method for sending message
    broadcastMessage(hub: string, method: string, msg: any) {
        //invoke method by its name using proxy 
        let proxy = this.proxies.find(p => p.id == hub);
        proxy.value.invoke(method, msg);
    }
}