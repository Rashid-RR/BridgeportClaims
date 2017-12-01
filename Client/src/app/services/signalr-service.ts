import { Resolve } from '@angular/router';
import { SignalR,BroadcastEventListener, SignalRConnection } from 'ng2-signalr';
import { Injectable } from '@angular/core'; 
import * as Rx from "rxjs/Rx";
declare var $:any;
// 1. if you want your component code to be testable, it is best to use a route resolver and make the connection there

@Injectable()
export class SignalRService {
    connection:SignalRConnection;
    onMessageSent$:BroadcastEventListener<any>;
    constructor(private _signalR: SignalR)  { 
      
    }
    connect(){
        this._signalR.connect().then((c) => {
            this.connection=c as SignalRConnection;
            this.onMessageSent$ = new BroadcastEventListener<any>('ON_MESSAGE_SENT');            
            this.connection.listen(this.onMessageSent$);
        });
    }

    listen(){
        this.onMessageSent$.subscribe((chatMessage: any) => {
            console.log(chatMessage);
     });
    }
}
/* 
// 2. use the resolver to resolve 'connection' when navigation to the your page/component
import { Route } from '@angular/router';
import { DocumentationComponent } from './index';
import { ConnectionResolver } from './documentation.route.resolver';

export const DocumentationRoutes: Route[] = [
    {
        path: 'documentation',
    component: DocumentationComponent,
     resolve: { connection: ConnectionResolver }
    }
];
 */
/* // 3. then inside your component
 export class ChatComponent {
  private _connection: SignalRConnection;

  constructor(route: ActivatedRoute) {    
  }
  
  ngOnInit() {
      this.connection = this.route.snapshot.data['connection'];
  }
  
}     */