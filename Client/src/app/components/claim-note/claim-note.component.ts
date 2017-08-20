import { Component, OnInit,AfterViewChecked } from '@angular/core';
import {ClaimManager} from "../../services/claim-manager";
import {HttpService} from "../../services/http-service";
import {ClaimNote} from "../../models/claim-note"
import {FormBuilder,FormControl, FormGroup, Validators} from "@angular/forms";
import swal from "sweetalert2";
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
  selector: 'app-claim-note',
  templateUrl: './claim-note.component.html',
  styleUrls: ['./claim-note.component.css']
})
export class ClaimNoteComponent implements OnInit,AfterViewChecked {
  
  form: FormGroup;
  constructor(
    public claimManager:ClaimManager,
    private formBuilder: FormBuilder, 
    private http: HttpService,
    private toast: ToastsManager
  ) {
    this.form = this.formBuilder.group({
      //claimId: [this.claimManager.selectedClaim.claimId],
      noteText: [ null,Validators.compose([Validators.required])],
      noteTypeId: [null,Validators.compose([Validators.required])]
    });
  }

  ngOnInit() {
  
 }
  ngAfterViewChecked() {
    let text = this.claimManager.selectedClaim && this.claimManager.selectedClaim.claimNote  ? this.claimManager.selectedClaim.claimNote.noteText :null;
    let noteTypeId = this.claimManager.selectedClaim && this.claimManager.selectedClaim.claimNote  ? this.claimManager.selectedClaim.claimNote.noteType : null;

    if(this.claimManager.selectedClaim.claimNote!==undefined && this.form.get("noteText").value == null && this.form.get("noteText").value !==this.claimManager.selectedClaim.claimNote.noteText){
      this.form.patchValue({
          noteTypeId:noteTypeId,
          noteText:text
      })
    }    
  }

  parseText(txt:String){
    return txt ? txt.replace(/\\n/g,'<br>') : '';
  }
  saveNote(){
    this.claimManager.loading = true;
    if (this.form.valid) {
      try {
        let note=this.form.value;
        note.claimId = this.claimManager.selectedClaim.claimId;
        this.http.saveClaimNote(this.form.value).subscribe(res => {
            if(!this.claimManager.selectedClaim.claimNote){
              this.claimManager.selectedClaim.claimNote = new ClaimNote(this.form.value['noteText'],this.form.value['noteTypeId'])
            }else{
              this.claimManager.selectedClaim.claimNote.noteText=this.form.value['noteText'];
            } 
            this.claimManager.selectedClaim.editing = false;
            this.claimManager.loading = false;
        }, (error) => {
          console.log(error);
          this.claimManager.loading = false;
          let err = error.json();
          this.toast.warning(err.error_description);
        })
      } catch (e) {
        this.toast.warning('Invalid field value(s). Please correct to proceed.');
        this.claimManager.loading = false;
      }
    }else{
      console.log(this.form.value)
       this.toast.warning('Invalid field value(s). Please correct to proceed.');
       this.claimManager.loading = false;
    }
  }

}
