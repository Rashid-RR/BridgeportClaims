import { Component, OnInit,Renderer2,AfterViewInit,NgZone,HostListener,ElementRef,ViewChild } from '@angular/core';
import {HttpService} from "../../services/http-service";
import {PaymentService} from "../../services/payment-service";
import {EventsService} from "../../services/events-service";
import {PaymentClaim} from "../../models/payment-claim";
import {ToastsManager } from 'ng2-toastr/ng2-toastr';
declare var jQuery:any;

@Component({
  selector: 'app-payment-detail-result',
  templateUrl: './payment-detail-result.component.html',
  styleUrls: ['./payment-detail-result.component.css']
})
export class PaymentDetailedResultComponent implements OnInit,AfterViewInit {

 constructor(private rd: Renderer2,private ngZone:NgZone,public paymentService:PaymentService, private http: HttpService, private events: EventsService,private toast: ToastsManager) { }
  checkAll:Boolean=false;
  selectMultiple:Boolean=false;
  lastSelectedIndex:number;
  @ViewChild('prescriptionTable') table:ElementRef;

  ngOnInit() {
  }
   activateClaimCheckBoxes(){
    jQuery('#claimsCheckBox').click();
  }
  select(p:any,$event,index){
      p.selected = $event.target.checked
      if(this.selectMultiple){
          for(var i=this.lastSelectedIndex;i<index;i++){
              try{
                let p = jQuery('#row'+i).attr('prescription');
                let prescription = JSON.parse(p);
                let data = this.paymentService.rawDetailedClaimsData.get(prescription.prescriptionId);
                data.selected = true;
              }catch(e){}
          }
      }
      this.lastSelectedIndex = index;
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
  claimsCheckBox($event){    
     this.checkAll =  $event.target.checked; 
     if(this.checkAll){
       this.paymentService.claimsDetail.forEach(c=>{
         c.selected = false;
       })
     }else{
       this.paymentService.claimsDetail.forEach(c=>{
         c.selected = false;
       });
     }   
  }

}
