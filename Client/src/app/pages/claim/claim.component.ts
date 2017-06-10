import { Component, OnInit } from '@angular/core';
import {HttpService} from "../../services/http-service"
import {ClaimManager} from "../../services/claim-manager";

@Component({
  selector: 'app-claim',
  templateUrl: './claim.component.html',
  styleUrls: ['./claim.component.css']
})
export class ClaimsComponent implements OnInit {
  
  constructor(public claimManager:ClaimManager,private http:HttpService) {
     
   }

   
  ngOnInit() {
     window['jQuery']('body').addClass('sidebar-collapse');
  }

}
