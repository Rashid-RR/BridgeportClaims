import { Component, OnInit } from '@angular/core';
import {ClaimManager} from "../../services/claim-manager";

@Component({
  selector: 'app-claim-note',
  templateUrl: './claim-note.component.html',
  styleUrls: ['./claim-note.component.css']
})
export class ClaimNoteComponent implements OnInit {

  constructor(public claimManager:ClaimManager) { }

  ngOnInit() {
  }

}
