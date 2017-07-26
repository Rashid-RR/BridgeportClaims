import { Component, OnInit } from '@angular/core';
import { ClaimManager } from "../../services/claim-manager";
import swal from "sweetalert2";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { HttpService } from "../../services/http-service"


@Component({
  selector: 'app-claim-script-note',
  templateUrl: './claim-script-note.component.html',
  styleUrls: ['./claim-script-note.component.css']
})
export class ClaimScriptNoteComponent implements OnInit {

  constructor(
    public claimManager: ClaimManager,
    private http: HttpService,
    private toast: ToastsManager
  ) { }

  ngOnInit() {
  }

  openScriptNotesModal(sType, sDate, SPerson, sNote) {
    let newDate = new Date(sDate);
    console.log(newDate);
    let dateString = (newDate.getMonth() + 1 + '/' + newDate.getDate() + '/' + newDate.getFullYear());

    console.log(dateString);

    swal({
      title: 'Script Note',
      html:
      `
      <div class="container-fluid">
        <div class="row heading">
          <div class="col-xs-4">Note Type</div>
          <div class="col-xs-4">Date</div>
          <div class="col-xs-4">By</div>
        </div>
        <div class="row">
          <div class="col-xs-4 label label-info">` + sType + `</div>
          <div class="col-xs-4">` + dateString + `</div>
          <div class="col-xs-4">` + SPerson + `</div>
        </div>
      </div>
      <div class="form-group">
        <label id="noteTextLabel">Note Text</label>
        <textarea class="form-control"  id="note" rows="5"  style="resize: vertical;">` + sNote + `</textarea>
      </div>`,
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
