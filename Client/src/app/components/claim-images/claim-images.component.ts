import { Component, OnInit } from '@angular/core';
import {ClaimManager} from "../../services/claim-manager";

@Component({
  selector: 'app-claim-images',
  templateUrl: './claim-images.component.html',
  styleUrls: ['./claim-images.component.css']
})
export class ClaimImagesComponent implements OnInit {

  constructor(public claimManager:ClaimManager) { }

  ngOnInit() {
  }

}
