import { Component, OnInit, NgZone } from '@angular/core';
import { DocumentItem } from '../../models/document';
import { DocumentType } from '../../models/document-type';
import { EventsService } from '../../services/events-service';
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
  constructor(private events: EventsService, private router: Router, private _ngZone: NgZone) {
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
    this.events.on("new-document", (doc: DocumentItem) => {
      setTimeout(() => {
        this.documents.push(doc);
      },50)
    })
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
