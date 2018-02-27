import { Injectable } from '@angular/core';
import { Router,NavigationEnd } from '@angular/router';

@Injectable()
export class ReportLoaderService {
  loading: Boolean = false; 
  current:String='List';
  currentURL:String='List';
  routes:string[]=[];
  constructor(private router:Router) { 
      this.router.events.subscribe(e=>{
        if(e instanceof NavigationEnd){
          this.routes=this.router.url.split("/");
        }
      })
  }

}
