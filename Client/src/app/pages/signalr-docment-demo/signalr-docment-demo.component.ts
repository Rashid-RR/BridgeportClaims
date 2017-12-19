import { Component, OnInit, NgZone } from '@angular/core';
import { DocumentItem } from '../../models/document';
import { DocumentType } from '../../models/document-type';
import { EventsService } from '../../services/events-service';
import { SignalRService } from '../../services/signalr-service';
import * as Immutable from 'immutable';
import { Router } from '@angular/router';
import { setTimeout } from 'core-js/library/web/timers';

let documents: DocumentItem[];
@Component({
  selector: 'app-signalr-docment-demo',
  templateUrl: './signalr-docment-demo.component.html',
  styleUrls: ['./signalr-docment-demo.component.css']
})
export class SignalrDocmentDemoComponent implements OnInit {

  documents: DocumentItem[]=[];
  documentTypes: Immutable.OrderedMap<any, any> = Immutable.OrderedMap<any, any>();
  data: any = {};
  loading: boolean = false;
  display: string = 'grid';
  lastUpdated: Date;
  hub: string = 'documentsHub';
  pollTime: number = 600000; // microseconds*seconds*minutes ~ currently after every two minute
  constructor(private signalR: SignalRService, private events: EventsService, private router: Router, private _ngZone: NgZone) {
    this.data = {
      date: null,
      sort: 'DocumentID',
      sortDirection: 'ASC',
      page: 1,
      pageSize: 500
    };
    documents = this.documentItems;
  }

  ngOnInit() {
    this.signalR.connect(this.hub, (hub: any, hubname: string) => { 
      this.start(hub, hubname);
    });
  }
  start(hub: any, hubname: string): void {
    hub.client.newDocument = (...args) => {
      this.onNewDocument(args);
    };
  }
  onNewDocument(args: Array<any>) {
    let doc: DocumentItem = {
      documentId: args[0], fileName: args[1], fileSize: args[3],
      creationTimeLocal: args[4], lastAccessTimeLocal: args[5],
      lastWriteTimeLocal: args[6], extension: args[2], fileUrl: args[8], fullFilePath: args[7]
    }
    this._ngZone.run(() => {
      this.documents.push(doc);
    });
  }
  get documentItems(): Array<DocumentItem> {
    return this.documents;
  }

  openFile(file: any) {
    this.loading = true;
    if (!file['fileUrl']) {
      file['fileUrl'] = file['FileUrl'];
    }
    if (!file['fileName']) {
      file['fileName'] = file['FileName'];
    }
    localStorage.setItem('file-' + (file.DocumentId || file.documentId), JSON.stringify(file));
    window.open('#/main/indexed-image/' + (file.DocumentId || file.documentId), '_blank');
  }

  get documentList(): Array<DocumentItem> {
    return this.documents;
  }

}
