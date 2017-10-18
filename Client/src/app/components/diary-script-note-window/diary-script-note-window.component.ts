import { Component, OnInit,Input } from '@angular/core';
import { PrescriptionNote } from "../../models/prescription-note"


declare var  $:any; 
@Component({
  selector: 'app-diary-script-note-window',
  templateUrl: './diary-script-note-window.component.html',
  styleUrls: ['./diary-script-note-window.component.css']
})
export class DiaryScriptNoteWindowComponent implements OnInit {

  note:PrescriptionNote;
  constructor(){
   }

  ngOnInit() {
    $(".ngPopup.in").css({"left":"30%"});
  }
  showNote(note){
    this.note = note;
  }

}
