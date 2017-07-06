import { Component, OnInit,AfterViewChecked } from '@angular/core';
import {ClaimManager} from "../../services/claim-manager";
import {HttpService} from "../../services/http-service";
import {EventsService} from "../../services/events-service";
import {Prescription} from "../../models/prescription";
import {PrescriptionNotes} from "../../models/prescription-notes";
import swal from "sweetalert2";
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-claim-prescriptions',
  templateUrl: './claim-prescriptions.component.html',
  styleUrls: ['./claim-prescriptions.component.css']
})
export class ClaimPrescriptionsComponent implements OnInit {

  constructor(private dp:DatePipe,public claimManager:ClaimManager,private events:EventsService,private http:HttpService) { }

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

  showNotes(prescriptionId:Number){
    this.claimManager.loading = true;
      this.http.getPrescriptionNotes(prescriptionId).single().subscribe(res=>{
        let notes:Array<PrescriptionNotes> = res.json();
        this.displayNotes(notes);
      },error=>{
        this.claimManager.loading = false;
      });
  }

  displayNotes(notes:Array<PrescriptionNotes>){

    let notesHTML='';
    notes.forEach(note=>{
      
      let noteDate = this.dp.transform(note.date,"shortDate");
      notesHTML = notesHTML+`
            <tr>
              <td>`+noteDate+`</td>
              <td>`+note.type+`</td>
              <td>`+note.enteredBy+`</td>
              <td>`+note.note+`</td>               
            </tr>`;
    })
    let html=`<div class="row invoice-info">
              <div class="col-sm-12 invoice-col" style="text-align:left;font-size:10pt">
                <div class="table-responsive">
                  <table class="table no-margin table-striped">
                    <thead>
                    <tr>
                      <th>Date</th>
                      <th>Type</th>
                      <th>By</th>
                      <th width="60%">Notes</th>
                    </tr>
                    </thead>
                    <tbody>
                    `+notesHTML+`
                    </tbody>
                  </table>
                </div>
              </div>
        </div>`;
        this.claimManager.loading = false;
        swal({
            title: 'Claim Note',
            width:"650px",
            html:html
        }).then(success=>{          
          
        }).catch(swal.noop)
  }

 
}
