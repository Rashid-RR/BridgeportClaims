import { Component, OnInit, AfterViewInit } from '@angular/core';
import {  SkippedPaymentService } from "../../services/services.barrel";

declare var $:any;

@Component({
  selector: 'app-skipped-payment-search',
  templateUrl: './skipped-payment-search.component.html',
  styleUrls: ['./skipped-payment-search.component.css']
})
export class SkippedPaymentSearchComponent implements OnInit ,AfterViewInit{

  constructor(public skipped: SkippedPaymentService,) { }

  ngAfterViewInit() {
    if(this.skipped.payors && this.skipped.payors.length>0){
      $("#payorSelection").select2();
    }
  }
  ngOnInit() {
    this.skipped.payorListReady.subscribe(()=>{
      $("#payorSelection").select2();

    })
  }

  filter(){
    this.skipped.data.payorIds=($('#payorSelection').val()||undefined);
    this.skipped.fetchSkippedPayReport();
  }
  clear(){
    $('#payorSelection').val(null).trigger('change');
  }

}
