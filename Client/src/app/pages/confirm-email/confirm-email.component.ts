import { Component, OnInit } from '@angular/core';
import {Router,ActivatedRoute,NavigationEnd,NavigationStart } from '@angular/router';
import {Http,Headers} from "@angular/http";

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.css']
})
export class ConfirmEmailComponent implements OnInit {

  confirmed: Number = 0;
  private hashChange:any;
  constructor(private route: ActivatedRoute,private req:Http) {

   }

  ngOnInit() {
      this.hashChange = this.route.params.subscribe(params => {
       if(params['link']){
          console.log(params['link'])
       }
    });
  }


}
