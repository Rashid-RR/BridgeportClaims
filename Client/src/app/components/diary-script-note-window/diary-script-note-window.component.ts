import { Component, OnInit,Input } from '@angular/core';
import { PrescriptionNote } from "../../models/prescription-note"
import {WindowInstance} from "../ng-window/WindowInstance"; 

declare var  $:any; 
@Component({
  selector: 'app-diary-script-note-window',
  templateUrl: './diary-script-note-window.component.html',
  styleUrls: ['./diary-script-note-window.component.css']
})
export class DiaryScriptNoteWindowComponent implements OnInit {

  note:PrescriptionNote;
  constructor(public dialog: WindowInstance){
   }

  ngOnInit() {
    this.dialog.config.BlockParentUI=true; 
    $(".ngPopup.in").css({"left":"30%"});
  }
  showNote(note){
    this.note = note;
  }

}
