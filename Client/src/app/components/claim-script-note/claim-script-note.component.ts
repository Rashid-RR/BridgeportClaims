import { Component, OnInit } from '@angular/core';
import { ClaimManager } from "../../services/claim-manager";
import swal from "sweetalert2";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { HttpService } from "../../services/http-service"
import { PrescriptionNote } from "../../models/prescription-note"
import { DatePipe,DecimalPipe } from '@angular/common';

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
    private toast: ToastsManager
  ) { }

  ngOnInit() {
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
    })
  }

}
