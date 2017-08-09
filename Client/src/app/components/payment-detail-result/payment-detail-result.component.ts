import { Component, OnInit,NgZone } from '@angular/core';
import {HttpService} from "../../services/http-service";
import {PaymentService} from "../../services/payment-service";
import {EventsService} from "../../services/events-service";
import {PaymentClaim} from "../../models/payment-claim";
import {ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
  selector: 'app-payment-detail-result',
  templateUrl: './payment-detail-result.component.html',
  styleUrls: ['./payment-detail-result.component.css']
})
export class PaymentDetailedResultComponent implements OnInit {

 constructor(private ngZone:NgZone,public paymentService:PaymentService, private http: HttpService, private events: EventsService,private toast: ToastsManager) { }

  ngOnInit() {
  }
   select(invoice:any,$event){
    invoice.selected = $event.target.checked
  }

}
