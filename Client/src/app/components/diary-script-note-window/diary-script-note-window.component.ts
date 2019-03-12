import { Component, OnInit, Input } from '@angular/core';
import { PrescriptionNote } from '../../models/prescription-note';
import {WindowInstance} from '../ng-window/WindowInstance';
import { ToastrService } from 'ngx-toastr';
import { DatePipe, DecimalPipe } from '@angular/common';
import swal from 'sweetalert2';
import { HttpService } from '../../services/http-service';

declare var  $: any;
@Component({
  selector: 'app-diary-script-note-window',
  templateUrl: './diary-script-note-window.component.html',
  styleUrls: ['./diary-script-note-window.component.css']
})
export class ScriptNoteWindowComponent implements OnInit {

  note: PrescriptionNote;
   editFollowUpDate = false;
  constructor(public dialog: WindowInstance,
    private dp: DatePipe,
    private http: HttpService,
    private toast: ToastrService) {
   }

  ngOnInit() {
    this.dialog.config.BlockParentUI = true;
  }
  showNote(note) {
    this.note = note;
  }
  saveFollowUpDate() {
    const followupDate = this.dp.transform($('#followupDate').val(), 'MM/dd/yyyy');
    if (followupDate) {
    swal({ title: '', html: 'Saving changes... <br/> <img src=\'assets/1.gif\'>', showConfirmButton: false }).catch(()=>{})
      .catch(()=>{});
    this.http.updateDiaryFollowUpDate(this.note.prescriptionNoteId, { diaryId: this.note.diaryId, followUpDate: followupDate })
      .subscribe(res => {
        swal.close();
        this.toast.success(res.message);
        this.editFollowUpDate = false;
      }, err => {
        this.toast.error(err.message);
      });
      } else {
        this.toast.error('Please select a follow-up date');
      }
  }
  update() {
    this.editFollowUpDate = true;
    setTimeout(() => {
      $('#followupDate').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' });
      $('#followupDate').datepicker({
        autoclose: true
      });
    }, 400);
  }

}
