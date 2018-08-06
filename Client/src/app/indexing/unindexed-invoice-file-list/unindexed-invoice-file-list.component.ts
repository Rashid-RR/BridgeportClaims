import { Component, OnInit, AfterViewInit } from '@angular/core';
import { DocumentManagerService } from "../../services/document-manager.service";
import { DocumentItem } from '../../models/document';
declare var $: any;

@Component({
  selector: 'indexing-unindexed-invoice-file-list',
  templateUrl: './unindexed-invoice-file-list.component.html',
  styleUrls: ['./unindexed-invoice-file-list.component.css']
})
export class UnindexedInvoiceFileListComponent implements OnInit, AfterViewInit {

  constructor(public ds: DocumentManagerService) { }

  ngOnInit() {
  }
  ngAfterViewInit() {

  }


}
