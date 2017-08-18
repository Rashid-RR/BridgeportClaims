import { Component, OnInit,Renderer2,AfterViewChecked,AfterViewInit,NgZone,HostListener,ElementRef,ViewChild } from '@angular/core';
import {HttpService} from "../../services/http-service";
import {PaymentService} from "../../services/payment-service";
import {EventsService} from "../../services/events-service";
import {PaymentClaim} from "../../models/payment-claim";
import {ToastsManager } from 'ng2-toastr/ng2-toastr';
import {Router} from "@angular/router";

declare var jQuery:any;


@Component({
  selector: 'app-payment-claim-result',
  templateUrl: './payment-claim-result.component.html',
  styleUrls: ['./payment-claim-result.component.css']
})
export class PaymentClaimResultComponent implements OnInit,AfterViewInit,AfterViewChecked {
  checkAll:Boolean=false;
  selectMultiple:Boolean=false;
  lastSelectedIndex:number;
  @ViewChild('claimsTable') table:ElementRef;

  constructor(private rd: Renderer2,private ngZone:NgZone,public paymentService:PaymentService, private http: HttpService, private router: Router, private events: EventsService,private toast: ToastsManager) { 
   
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
      this.rd.listen(this.table.nativeElement,'keydown',($event)=>{
        if($event.keyCode==16){
          this.selectMultiple = true;
        }
      })
      this.rd.listen(this.table.nativeElement,'keyup',($event)=>{
        if($event.keyCode==16){
          this.selectMultiple = false;
        }
      })
       
  }
  select(claim:PaymentClaim,$event,index:number){
    claim.selected = $event.target.checked;
    if(this.selectMultiple){
      for(var i=this.lastSelectedIndex;i<index;i++){
          try{
            let c = jQuery('#row'+i).attr('claim');
            let claim = JSON.parse(c);
            let data = this.paymentService.rawClaimsData.get(claim.claimNumber);
            data.selected = true;
          }catch(e){}
      }
    }
    this.lastSelectedIndex = index; 
  }

   
  showNotes(){

  }
  activateClaimCheckBoxes(){
    jQuery('#claimsCheckBox').click();
  }
  claimsCheckBox($event){    
     this.checkAll =  $event.target.checked;    
     if(this.checkAll){
       this.paymentService.claimsData.forEach(claim=>{
         claim.selected = false;
       })
     }else{
       this.paymentService.claimsData.forEach(claim=>{
         claim.selected = false;
       });
     }
  }
  prescriptionsCheckBox(invoice:any,$event){
      if($event.target.checked){

      }
  }
  ngAfterViewChecked() {
    this.updateTableHeadingWidth();
    let fixedHeader = document.getElementById('fixed-header');
    if (fixedHeader.style.position !== 'fixed') {
      fixedHeader.style.position = 'fixed';
      // console.log('set fixed header to Fixed');
    }    
  }

  cloneTableHeading() {
    let cln = document.getElementById('fixed-thead').cloneNode(true);
    let fixedHeader = document.getElementById('fixed-header');
    fixedHeader.appendChild(cln);
    this.updateTableHeadingWidth();
  }

  cloneBoxHeader() {
    let cln = document.getElementById
  }


  updateTableHeadingWidth() {
    setTimeout(() => {
      let fixedHeader = document.getElementById('fixed-header');
      let fixedMaxHeader = document.getElementById('fixed-max-header');
      let mainTable = document.getElementById('maintable');
      if (fixedHeader) {
        if (mainTable) {
          let tableWidth = mainTable.clientWidth.toString();
          fixedHeader.style.width = tableWidth + 'px';
        }
      } else {
        if (mainTable) {
          let tableWidth = mainTable.clientWidth.toString();
          try{
            fixedMaxHeader.style.width = tableWidth + 'px';
          }catch(e){}
        }
      }
    }, 500)
  }
}
