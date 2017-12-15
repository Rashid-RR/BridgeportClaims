import { Component, OnInit, NgZone } from '@angular/core';
import { DocumentItem } from '../../models/document';
import { DocumentType } from '../../models/document-type';
import { EventsService } from '../../services/events-service';
import { SignalRService } from '../../services/signalr-service';
import * as Immutable from 'immutable';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signalr-docment-demo',
  templateUrl: './signalr-docment-demo.component.html',
  styleUrls: ['./signalr-docment-demo.component.css']
})
export class SignalrDocmentDemoComponent implements OnInit {

  documents: Immutable.OrderedMap<any, any> = Immutable.OrderedMap<any, any>();
  documentTypes: Immutable.OrderedMap<any, any> = Immutable.OrderedMap<any, any>();
  data: any = {};
  loading: boolean = false;
  display: string = 'grid';
  lastUpdated: Date;
  hub: string = 'DocumentsHub';
  pollTime: number = 120000; // microseconds*seconds*minutes ~ currently after every two minute
  constructor(private signalR: SignalRService, private events: EventsService, private router: Router, private _ngZone: NgZone) {
    this.data = {
      date: null,
      sort: 'DocumentID',
      sortDirection: 'ASC',
      page: 1,
      pageSize: 500
    };
  }

  ngOnInit() {
    this.signalR.connect(this.hub);
    if (this.signalR.connected) {
      this.fetchDocs();
      setInterval(() => {
        this.fetchDocs();
      }, this.pollTime);
    } else {
      this.events.on('start-listening-to-signalr', () => {
        this.fetchDocs();
        setInterval(() => {
          this.fetchDocs();
        }, this.pollTime);
      });
    }
  }

  fetchDocs() {
    this._ngZone.run(() => {
      this.signalR.loading = true;
      this.signalR.getProxy(this.hub).value.server.getDocuments(this.data).then(result => {
        console.log(result);
        if (result) {
          this.documents = Immutable.OrderedMap<any, any>();
          result.DocumentResults.forEach((doc: any) => {
            try {
              this.documents = this.documents.set(doc.DocumentId, doc);
            } catch (e) { console.log(e); }
          });
          result.DocumentTypes.forEach((type: any) => {
            try {
              this.documentTypes = this.documentTypes.set(type.DocumentTypeId, type);
            } catch (e) { }
          });
        } else {
          console.log("No data returned...");
        }
        this.lastUpdated = new Date();
        this.signalR.loading = false;
      }, err => {
        console.log(err);
        this.signalR.loading = false;
      });
    });
  }

  openFile(file: any) {
    this.loading = true;
    console.log(file);
    if (!file['fileUrl']) {
      file['fileUrl'] = file['FileUrl'];
    }
    if (!file['fileName']) {
      file['fileName'] = file['FileName'];
    }
    localStorage.setItem('file-' + (file.DocumentId || file.documentId), JSON.stringify(file));
    window.open('#/main/indexed-image/' + (file.DocumentId || file.documentId), '_blank');
  }

  get documentList(): Array<any> {
    return this.documents.toArray();
  }

}
