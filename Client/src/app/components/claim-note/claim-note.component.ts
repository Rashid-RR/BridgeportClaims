import { Component, OnInit,AfterViewChecked } from '@angular/core';
import {ClaimManager} from "../../services/claim-manager";
import {HttpService} from "../../services/http-service";
import {warn,success} from "../../models/notification"
import {ClaimNote} from "../../models/claim-note"
import {FormBuilder,FormControl, FormGroup, Validators} from "@angular/forms";
 
@Component({
  selector: 'app-claim-note',
  templateUrl: './claim-note.component.html',
  styleUrls: ['./claim-note.component.css']
})
export class ClaimNoteComponent implements OnInit,AfterViewChecked {
  
  form: FormGroup;
  constructor(public claimManager:ClaimManager,private formBuilder: FormBuilder, private http: HttpService) {
      
  }

  ngOnInit() {

  }
  ngAfterViewChecked() {
    this.form = this.formBuilder.group({
        claimId: [this.claimManager.selectedClaim.claimId],
        noteText: [this.claimManager.selectedClaim.claimNote  ? this.claimManager.selectedClaim.claimNote.noteText : null,Validators.compose([Validators.required])],
        noteTypeId: [this.claimManager.selectedClaim.claimNote  ? this.claimManager.selectedClaim.claimNote.noteType : null,Validators.compose([Validators.required])]
      });
  }

  saveNote(){
    this.claimManager.loading = true;
    if (this.form.valid) {
      try {
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
          warn(err.error_description);
        })
      } catch (e) {
        warn( 'Error in fields. Please correct to proceed!');
        this.claimManager.loading = false;
      }
    }else{
      console.log(this.form.value)
       warn('Error in fields. Please correct to proceed!');
       this.claimManager.loading = false;
    }
  }

}
