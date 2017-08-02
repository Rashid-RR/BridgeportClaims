import { Component, OnInit } from '@angular/core';
import {HttpService} from "../../services/http-service";
import {PaymentService} from "../../services/payment-service";
import {EventsService} from "../../services/events-service";
import {ToastsManager } from 'ng2-toastr/ng2-toastr';
import {Router} from "@angular/router";

@Component({
  selector: 'app-payment-result',
  templateUrl: './payment-result.component.html',
  styleUrls: ['./payment-result.component.css']
})
export class PaymentResultComponent implements OnInit {

  constructor(public paymentService:PaymentService, private http: HttpService, private router: Router, private events: EventsService,private toast: ToastsManager) {

   }

  ngOnInit() {
    
  }
  select(invoice:any,$event){
    invoice.selected = $event.target.checked
  }

}
