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
declare var  $:any; 

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
    let win = $("body:not(.sidebar-collapse)");
    var minusLeft = 50,x=0,y=0;
    if(win.length>0){
        minusLeft = 230;
    }
    x=(window.innerWidth - 580)/2+minusLeft;
    y = (window.innerHeight - 250)/2;
    let config = new WindowConfig("Prescription Note", new Size(250, 600))  //height, width
    config.position=new CustomPosition(x, y)//left,top
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
            <div class="col-xs-2">Rx Date</div>
            <div class="col-xs-2">Rx Number</div>
            <div class="col-xs-2">Updated</div>
            <div class="col-xs-2">Note Type</div>
            <div class="col-xs-3">By</div>
          </div>
          <div class="row">
            <div class="col-xs-2">` + rxDate + `</div>
            <div class="col-xs-2">` + note.rxNumber + `</div>
            <div class="col-xs-2">` + noteUpdatedOn + `</div>
            <div class="col-xs-2 label label-info">` + note.type + `</div>
            <div class="col-xs-3">` + note.enteredBy + `</div>
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
      showConfirmButton: note.hasDiaryEntry,
      confirmButtonText:'Remove from Diary',
      //confirmButtonColor: '#992727',
      focusCancel: true,
      cancelButtonText: "Done",
      onOpen: function () {
         
      }
    }).then((result)=>{
        swal({ title: "", html: "Saving changes... <br/> <img src='assets/1.gif'>", showConfirmButton: false }).catch(swal.noop)
        .catch(swal.noop);
        this.http.removeFromDiary(note.prescriptionNoteId)
        .map(res => { return res.json() }).subscribe(res => { 
            swal.close();
            this.toast.success(res.message);
            note.hasDiaryEntry = false;
        },err=>{
          this.toast.error(err.message);
        })
    }).catch(swal.noop)
  }

}
