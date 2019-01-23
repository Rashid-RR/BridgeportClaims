import { Component, OnInit, AfterViewChecked } from '@angular/core';
import {ClaimManager} from '../../services/claim-manager';
import {HttpService} from '../../services/http-service';
import {ClaimNote} from '../../models/claim-note';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-claim-note',
  templateUrl: './claim-note.component.html',
  styleUrls: ['./claim-note.component.css']
})
export class ClaimNoteComponent implements OnInit, AfterViewChecked {

  form: FormGroup;
  constructor(
    public claimManager: ClaimManager,
    private formBuilder: FormBuilder,
    private http: HttpService,
    private toast: ToastrService
  ) {
    this.form = this.formBuilder.group({
      noteText: [ null, Validators.compose([Validators.required])],
      noteTypeId: [null]
    });
  }

  ngOnInit() {
 }
  ngAfterViewChecked() {
    const text = this.claimManager.selectedClaim && this.claimManager.selectedClaim.claimNote ?
     this.claimManager.selectedClaim.claimNote.noteText : null;
    const noteTypeId = this.claimManager.selectedClaim && this.claimManager.selectedClaim.claimNote ?
     this.claimManager.selectedClaim.claimNote.noteType : null;

    if (this.claimManager.selectedClaim.claimNote !== undefined && this.form.get('noteText').value == null &&
      this.form.get('noteText').value !== this.claimManager.selectedClaim.claimNote.noteText) {
      this.form.patchValue({
          noteTypeId: noteTypeId,
          noteText: text
      });
    }
  }

  parseText(txt: String) {
    return txt ? txt.replace(/\\n/g, '<br>') : '';
  }
  saveNote() {
    this.claimManager.loading = true;
    const warningMsg = 'Invalid field value(s). Please correct to proceed.';
    if (this.form.valid) {
      try {
        const note = this.form.value;
        note.claimId = this.claimManager.selectedClaim.claimId;
        this.http.saveClaimNote(this.form.value).subscribe(res => {
            if (!this.claimManager.selectedClaim.claimNote) {
              this.claimManager.selectedClaim.claimNote = new ClaimNote(this.form.value['noteText'], this.form.value['noteTypeId']);
            } else {
              this.claimManager.selectedClaim.claimNote.noteText = this.form.value['noteText'];
            }
            this.claimManager.selectedClaim.editing = false;
            this.claimManager.loading = false;
        }, (error) => {
          this.claimManager.loading = false;
          const err = error.error;
          this.toast.warning(err.error_description);
        });
      } catch (e) {
        this.toast.warning(warningMsg);
        this.claimManager.loading = false;
      }
    } else {
       this.toast.warning(warningMsg);
       this.claimManager.loading = false;
    }
  }
}
