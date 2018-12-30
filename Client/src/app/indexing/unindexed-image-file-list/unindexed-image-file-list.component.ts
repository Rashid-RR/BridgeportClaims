import { Component, OnInit, AfterViewInit } from '@angular/core';
import { DocumentManagerService } from '../../services/document-manager.service';
import { DocumentItem } from '../../models/document';
declare var $: any;

@Component({
  selector: 'indexing-unindexed-image-file-list',
  templateUrl: './unindexed-image-file-list.component.html',
  styleUrls: ['./unindexed-image-file-list.component.css']
})
export class UnindexedImageFileListComponent implements OnInit, AfterViewInit {

  constructor(public ds: DocumentManagerService) { }

  ngOnInit() {
  }
  ngAfterViewInit() {

  }


}
