import { Component, OnInit, NgZone } from '@angular/core';
import { SignalRService } from '../../services/services.barrel';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { setTimeout } from 'core-js/library/web/timers';
declare var $: any;
export class BroadCastMessage {
  constructor(public msgFrom: string, public msg: string) {
  }
}
@Component({
  selector: 'app-signalr-demo',
  templateUrl: './signalr-demo.component.html',
  styleUrls: ['./signalr-demo.component.css']
})
export class SignalrDemoComponent implements OnInit {

  message: string;
  name: string;
  nameset = false;
  documentProxy: any;
  messages: { msgFrom: string, msg: string }[] = [];
  hub = 'documentsHub';
  constructor(
    public signalR: SignalRService,
    private toast: ToastsManager, private _ngZone: NgZone
  ) { }

  ngOnInit() {
    this.signalR.connect(this.hub, (hub: any, hubname: string) => {
      try {
        this.start(hub, hubname);
      } catch (e) {
        console.log("Hub does not exist")
      }
    });    
  }
  start(hub: any, hubname: string): void {
    this.documentProxy = hub;
    console.log(hub);
    hub.client.receiveMessage = (msgFrom, msg) => {
      this.onMessageReceived(msgFrom, msg);
    };
  }
  private onMessageReceived(msgFrom: string, msg: string) {
    console.log('New message received from ' + msgFrom, msg);
    this._ngZone.run(() => {
      this.messages.push({ msgFrom: msgFrom, msg: msg });
    });
  }
  checkName() {
    console.log(this.name);
    if (!this.name) {
      this.toast.warning('Enter a name before continuing');
    } else {
      this.nameset = true;
      setTimeout(() => {
        $('#message').focus();
      }, 100);
    }
  }
  send() {
    if (this.message) {
      this.signalR.loading = true;
      this.signalR.connection.hub.start().done((r) => {
        this.documentProxy.server.broadCastMessage(this.name, this.message).then(r => {
          console.log('Sent ...');
          this.message = '';
          this.signalR.loading = false;
        });
      });
    }
  }

}
