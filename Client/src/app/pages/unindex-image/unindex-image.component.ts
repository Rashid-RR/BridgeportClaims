import { Component, OnInit } from '@angular/core';
// Services
import { DocumentManagerService } from "../../services/document-manager.service";

@Component({
  selector: 'app-unindex-image',
  templateUrl: './unindex-image.component.html',
  styleUrls: ['./unindex-image.component.css']
})
export class UnindexedImageComponent implements OnInit {

  constructor(public ds:DocumentManagerService) { }

  ngOnInit() {
  }

}
