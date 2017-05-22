import {AfterViewInit,Renderer2, Component,OnDestroy, OnInit} from "@angular/core";
import {Http,Headers} from "@angular/http";
import { Router,NavigationEnd } from '@angular/router';
  @Component({
  selector: 'app-root',
  template: `<router-outlet></router-outlet>`
})
export class AppComponent implements OnInit,OnDestroy {
 
  constructor() {
     
  }
 ngOnDestroy(){
    
  }
  ngOnInit() {
  
}
}
