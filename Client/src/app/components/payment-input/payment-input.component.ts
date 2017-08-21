import { Component, OnInit } from '@angular/core';
import {FormBuilder,FormControl, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {HttpService} from "../../services/http-service";
import {PaymentService} from "../../services/payment-service";
import {EventsService} from "../../services/events-service";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DecimalPipe } from '@angular/common';

@Component({
  selector: 'app-payment-input',
  templateUrl: './payment-input.component.html',
  styleUrls: ['./payment-input.component.css']
})
export class PaymentInputComponent implements OnInit {

  form: FormGroup;
  submitted: boolean = false;
  checkAmount:number = 0;
  constructor(private decimalPipe:DecimalPipe,public paymentService:PaymentService,private formBuilder: FormBuilder, private http: HttpService, private router: Router, private events: EventsService,private toast: ToastsManager) {
    this.form = this.formBuilder.group({
      checkNumber: [null],
      checkAmount: [null],
      amountSelected: [null],
      amountToPost: [null],
      amountRemaining: [null]
    });
  }
  ngOnInit() {

  }

   search(){
 
   }

 textChange(controlName:string){
   if(this.form.get(controlName).value ==='undefined' || this.form.get(controlName).value ===''){
     this.form.get(controlName).setValue(null);
   }else{
     switch(controlName){
       case 'checkAmount':
       case 'amountToPost':
        var val = this.form.get(controlName).value.replace(",",'');
        this.form.get(controlName).setValue(this.decimalPipe.transform(val,"1.0-2"));
       break;
       default:
       break;

     }
   }
 }
 checkNumber($event){
  $event = ($event) ? $event : window.event;
  var charCode = ($event.which) ? $event.which : $event.keyCode;
  if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode!=44 && charCode!=46) {
      return false;
  }
  return true;
 }
  post(){
    var prescriptions=[];
    this.paymentService.detailedClaimsData.forEach(p=>{
        if(p.selected){
          prescriptions.push(p.prescriptionId)
        }
    })
    this.form.get('amountRemaining').setValue(this.decimalPipe.transform(this.amountRemaining,"1.0-2"));
    var form  = this.form.value;
    form.prescriptionIds = prescriptions;
    form.amountRemaining = undefined;
    this.paymentService.post(form);
  }
  get amountRemaining():Number{
    var amount =  Number((this.form.get('checkAmount') ? this.form.get('checkAmount').value : 0 ) -this.paymentService.amountSelected);
    return amount;
  }

}
