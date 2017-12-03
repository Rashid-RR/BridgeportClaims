import { Component, OnInit } from '@angular/core';
import { SignalRService } from "../../services/services.barrel";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { setTimeout } from 'core-js/library/web/timers';
declare var $:any;
export class BroadCastMessage{
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
  nameset: boolean = false;
  documentProxy;
  messages: { msgFrom: string, msg: string }[] = [];
  hub:string='documentsHub';
  constructor(
    public signalR: SignalRService,
    private toast: ToastsManager
  ) { }

  ngOnInit() {
    console.log($.connection);
    this.signalR.connect(this.hub);
  }
  checkName() {
    console.log(this.name);
    if (!this.name) {
      this.toast.warning('Enter a name before continuing');
    } else {
      this.nameset = true;
      setTimeout(()=>{
        $('#message').focus();
      },100);
    }
  }
  send() {
    if (this.message) {
      this.signalR.loading=true;
      this.signalR.getProxy(this.hub).value.server.broadCastMessage(this.name,this.message).then(r=>{
        console.log("Sent ...");
        this.message = '';
        this.signalR.loading=false;
      });
    }
  }

}
