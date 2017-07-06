import { Component, OnInit,AfterViewChecked } from '@angular/core';
import {ClaimManager} from "../../services/claim-manager";
import {EventsService} from "../../services/events-service";
import {Prescription} from "../../models/prescription";

@Component({
  selector: 'app-claim-prescriptions',
  templateUrl: './claim-prescriptions.component.html',
  styleUrls: ['./claim-prescriptions.component.css']
})
export class ClaimPrescriptionsComponent implements OnInit {

  constructor(public claimManager:ClaimManager,private events:EventsService) { }

  ngOnInit() {
    this.events.on("claim-updated",()=>{
        setTimeout(()=>{
          window['jQuery']('input[type="checkbox"].flat-red, input[type="radio"].flat-red')
          .iCheck({
            checkboxClass: 'icheckbox_flat-green',
            radioClass: 'iradio_flat-green'
          });
        },1000);
    })
  }

  setSelected(p:any,s:Boolean){
      console.log("Works...");
      p.selected = s==undefined? true : s;
  }

 
}
