import { Component, ViewChild, OnInit } from '@angular/core';
import { ClaimManager } from "../../services/claim-manager";
import swal from "sweetalert2";
import { ToastsManager } from 'ng2-toastr';
import { DatePipe, DecimalPipe } from '@angular/common';
import { Router } from '@angular/router';
import { SwalComponent, SwalPartialTargets } from '@toverux/ngx-sweetalert2';

import { HttpService } from "../../services/http-service"
import { PrescriptionNote } from "../../models/prescription-note"
import { WindowsInjetor, CustomPosition, Size, WindowConfig } from "../ng-window";
import { ScriptNoteWindowComponent } from "../components-barrel";
declare var $: any;

@Component({
  selector: 'app-claim-script-note',
  templateUrl: './claim-script-note.component.html',
  styleUrls: ['./claim-script-note.component.css']
})
export class ClaimScriptNoteComponent implements OnInit {

  note: PrescriptionNote;
  editFollowUpDate = false;
  @ViewChild('diaryNoteSwal') private diaryNoteSwal: SwalComponent;

  constructor(
    public claimManager: ClaimManager,
    private http: HttpService,
    private dp: DatePipe,
    private router: Router,
    public readonly swalTargets: SwalPartialTargets,
    private toast: ToastsManager,
    private myInjector: WindowsInjetor
  ) { }

  ngOnInit() {
    this.router.routerState.root.queryParams.subscribe(params => {
      if (params['prescriptionNoteId']) {
        let prescriptionNoteId = params['prescriptionNoteId'];
        let note: PrescriptionNote = this.claimManager.selectedClaim.prescriptionNotes.find((r: PrescriptionNote) => r.prescriptionNoteId == prescriptionNoteId);
        if (note) {
          this.showNoteWindow(note);
        }
      }
    });
  }
  saveFollowUpDate() {
    let followupDate = this.dp.transform($('#followupDate').val(), "MM/dd/yyyy");
    if (followupDate) {
      swal({ title: "", html: "Saving changes... <br/> <img src='assets/1.gif'>", showConfirmButton: false }).catch(swal.noop)
        .catch(swal.noop);
      this.http.updateDiaryFollowUpDate(this.note.prescriptionNoteId, { diaryId: this.note.diaryId, followUpDate: followupDate })
        .subscribe(res => {
          swal.close();
          this.toast.success(res.message);
          this.editFollowUpDate = false;
          this.scriptNotesModal(this.note);
        }, err => {
          this.toast.error(err.message);
        })
    } else {
      this.toast.error("Please select a follow-up date");
    }
  }
  update() {
    this.editFollowUpDate = true;
    setTimeout(() => {
      $("#followupDate").inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" });
      $('#followupDate').datepicker({
        autoclose: true
      });
    }, 400);

  }
  showNoteWindow(note: PrescriptionNote) {
    let win = $("body:not(.sidebar-collapse)");
    var minusLeft = 50, x = 0, y = 0;
    if (win.length > 0) {
      minusLeft = 230;
    }
    x = (window.innerWidth - 580) / 2 + minusLeft;
    y = (window.innerHeight - 250) / 2;
    let config = new WindowConfig("Prescription Note", new Size(250, 600))  //height, width
    config.position = new CustomPosition(x, y)//left,top
    config.minusTop = 91;
    config.centerInsideParent = false;
    var temp = {}
    config.forAny = [temp];
    config.openAsMaximize = false;
    this.myInjector.openWindow(ScriptNoteWindowComponent, config)
      .then((win: ScriptNoteWindowComponent) => {
        win.showNote(note);
      })
  }

  scriptNotesModal(note: PrescriptionNote) {
    this.note = note;
    this.diaryNoteSwal.showConfirmButton = false;
    if (note.hasDiaryEntry) {
      this.diaryNoteSwal.showConfirmButton = true;
    }
    this.diaryNoteSwal.show().then((result) => {
      if (!result.dismiss) {
        swal({ title: "", html: "Saving changes... <br/> <img src='assets/1.gif'>", showConfirmButton: false }).catch(swal.noop)
          .catch(swal.noop);
        this.http.removeFromDiary(note.prescriptionNoteId)
          .subscribe(res => {
            swal.close();
            this.toast.success(res.message);
            note.hasDiaryEntry = false;
            this.note.hasDiaryEntry = false;
          }, err => {
            this.toast.error(err.message);
          })
      }
    }).catch(swal.noop)
  }

}
