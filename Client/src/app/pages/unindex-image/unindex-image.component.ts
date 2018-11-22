import { Component, OnInit, AfterViewInit } from '@angular/core';
import { EventsService } from '../../services/events-service';
import { DocumentManagerService } from '../../services/document-manager.service';
import { DocumentItem } from '../../models/document';
declare var $: any;


@Component({
  selector: 'app-unindex-image',
  templateUrl: './unindex-image.component.html',
  styleUrls: ['./unindex-image.component.css'],
})
export class UnindexedImageComponent implements OnInit, AfterViewInit {

  file: DocumentItem;
  constructor(public ds: DocumentManagerService, private events: EventsService) { }
  ngOnInit() {
    this.events.broadcast('reset-indexing-form', true);
    this.ds.cancel('image');
    this.events.on('archived-image', (id: any) => {
      this.ds.cancel('image');
    });
    this;
  }
  ngAfterViewInit() {

  }
}

