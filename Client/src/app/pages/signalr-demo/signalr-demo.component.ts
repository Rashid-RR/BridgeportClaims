import { Component, OnInit, NgZone } from '@angular/core';
import { SignalRService } from '../../services/services.barrel';
import { EventsService } from '../../services/events-service';
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
    public signalR: SignalRService, private events: EventsService,
    private toast: ToastsManager, private _ngZone: NgZone
  ) { }

  ngOnInit() {
    this.events.on("new-message", (m: any) => {
      setTimeout(() => {
        this.messages.push({ msgFrom: m.msgFrom, msg: m.msg });
      },50)
    })
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
      if(!this.signalR.connected){
          $.connection.hub.start().done(()=>{
            this.sendMessage();
          })
      }else{
        this.sendMessage();
      }
    }
  }
  sendMessage(){
    this.signalR.documentProxy.server.broadCastMessage(this.name, this.message).then(r => {
      console.log('Sent ...');
      this.message = '';
      this.signalR.loading = false;
    });
  }

}
