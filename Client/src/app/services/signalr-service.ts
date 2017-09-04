// event-service.ts
/**
 * Class to facilitate event based communication within the app by availing subjects/topics and subscribers
 */

import {Injectable} from "@angular/core";
import * as Rx from "rxjs/Rx";
declare var $:any;

@Injectable()
export class SignalRService {
  connection = $.hubConnection();
  hub:any 
  constructor() {
    var chat = $.connection.clockHub;    
    /* chat.client.broadcastMessage = function (name, message) {
        // Html encode display name and message. 
        var encodedName = $('<div />').text(name).html();
        var encodedMsg = $('<div />').text(message).html();
        // Add the message to the page. 
        $('#discussion').append('<li><strong>' + encodedName
            + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
    }; */

    this.hub = this.connection.createHubProxy("clockHub");
     console.log(this.hub,this.connection);
     this.hub.on('setRealTime', ( message)=> {
      console.log( message);
    });
    /* this.connection.start()
        .done(function(){ 
          console.log('Now connected, connection ID='); 
          setTimeout(()=>{
            console.log(this.hub)},3000);
        })
        .fail(function(){ console.log('Could not connect'); });   */  
    /* $.connection.hub.start()
    .done(function () {
        // Wire up Send button to call NewContosoChatMessage on the server.
        console.log("Done...");
    }).fail(function(){ console.log('Could not connect'); });  */
  }
}
