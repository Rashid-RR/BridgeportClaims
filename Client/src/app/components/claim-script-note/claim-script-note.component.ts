import { Component, OnInit } from '@angular/core';
import {ClaimManager} from "../../services/claim-manager";

@Component({
  selector: 'app-claim-script-note',
  templateUrl: './claim-script-note.component.html',
  styleUrls: ['./claim-script-note.component.css']
})
export class ClaimScriptNoteComponent implements OnInit {

  constructor(public claimManager:ClaimManager) { }

  ngOnInit() {
  }

}
