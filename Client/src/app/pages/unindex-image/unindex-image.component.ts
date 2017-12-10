import { Component, OnInit, AfterViewInit } from '@angular/core';
import { DocumentManagerService } from "../../services/document-manager.service";
import { DocumentItem } from 'app/models/document';
declare var $: any;


@Component({
  selector: 'app-unindex-image',
  templateUrl: './unindex-image.component.html',
  styleUrls:['./unindex-image.component.css'],
})
export class UnindexedImageComponent implements OnInit, AfterViewInit {
  
    file:DocumentItem;
    constructor(public ds: DocumentManagerService) { }
    ngOnInit() {
    }
    ngAfterViewInit() {
  
    }
  
  
  }
  
