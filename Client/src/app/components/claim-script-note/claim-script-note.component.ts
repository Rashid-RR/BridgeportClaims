import { Component, OnInit } from '@angular/core';
import { ClaimManager } from "../../services/claim-manager";
import swal from "sweetalert2";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { HttpService } from "../../services/http-service"
import { PrescriptionNote } from "../../models/prescription-note"
import { DatePipe,DecimalPipe } from '@angular/common';
import { Router } from '@angular/router';
import {WindowsInjetor,CustomPosition,Size,WindowConfig} from "../ng-window";
import {DiaryScriptNoteWindowComponent} from "../../components/components-barrel";

@Component({
  selector: 'app-claim-script-note',
  templateUrl: './claim-script-note.component.html',
  styleUrls: ['./claim-script-note.component.css']
})
export class ClaimScriptNoteComponent implements OnInit {

  constructor(
    public claimManager: ClaimManager,
    private http: HttpService,
    private dp: DatePipe,
    private router: Router,
    private toast: ToastsManager,
    private myInjector: WindowsInjetor
  ) { }

  ngOnInit() {
    this.router.routerState.root.queryParams.subscribe(params => {
        if(params['prescriptionNoteId']){
          let prescriptionNoteId = params['prescriptionNoteId'];
          let note:PrescriptionNote = this.claimManager.selectedClaim.prescriptionNotes.find((r:PrescriptionNote)=>r.prescriptionNoteId==prescriptionNoteId);
          if(note){
              this.showNoteWindow(note);
          }
        }
    });
  }
  showNoteWindow(note:PrescriptionNote){
    let config = new WindowConfig("Prescription Note", new Size(250, 600))  //height, width
    config.position=new CustomPosition(50+Math.random()*200, 100)//left,top
    config.minusTop = 91;      
    config.centerInsideParent=false;
    var temp={}
    config.forAny=[temp];
    config.openAsMaximize=false;
    this.myInjector.openWindow( DiaryScriptNoteWindowComponent,config)
    .then((win:DiaryScriptNoteWindowComponent) => {       
      win.showNote(note);
    })
  }

  openScriptNotesModal(note:PrescriptionNote) {
    let noteUpdatedOn = this.dp.transform(note.noteUpdatedOn, "shortDate");
    let rxDate = this.dp.transform(note.rxDate, "shortDate");
     
    swal({
      title: 'Script Note',
      html:
        `<div class="container-fluid">
          <div class="row heading">
            <div class="col-xs-4">Rx Date</div>
            <div class="col-xs-4">Rx Number</div>
            <div class="col-xs-4">Updated</div>
            <div class="col-xs-4">Note Type</div>
            <div class="col-xs-4">By</div>
          </div>
          <div class="row">
            <div class="col-xs-4">` + rxDate + `</div>
            <div class="col-xs-4">` + note.rxNumber + `</div>
            <div class="col-xs-4">` + noteUpdatedOn + `</div>
            <div class="col-xs-4 label label-info">` + note.type + `</div>
            <div class="col-xs-4">` + note.enteredBy + `</div>
          </div>
        </div>
        <div class="form-group" style="margin-top: 1.2rem;">
          <label id="noteTextLabel">Note Text</label>
          <div style="background: #d6d8cc; padding: 2rem;">` + note.note + `</div>
        </div>`,
      width: window.innerWidth*1.799/3,
      showCancelButton: true,
      showCloseButton: true,
      showLoaderOnConfirm: true,
      showConfirmButton: false,
      cancelButtonText: "Dismiss",
      onOpen: function () {
        window['jQuery']('#note').focus()
      }
    }).catch(swal.noop)
  }

}
