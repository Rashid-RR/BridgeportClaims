import { Component, OnInit } from '@angular/core';
import { SignalRService } from "../../services/signalr-service";

@Component({
  selector: 'app-main-layout',
  template: `<router-outlet></router-outlet>`
})
export class MainLayoutComponent implements OnInit {

  constructor(private signalR:SignalRService) { }

  ngOnInit() {
  }

}
