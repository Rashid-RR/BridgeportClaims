import { Component, OnInit, NgZone } from '@angular/core';
import { DocumentItem } from '../../models/document';
import { DocumentType } from '../../models/document-type';
import { EventsService } from '../../services/events-service';
import { SignalRService } from '../../services/signalr-service';
import * as Immutable from 'immutable';
import { Router } from '@angular/router';
import { timeInterval } from 'rxjs/operators/timeInterval';
import { setTimeout, setInterval } from 'core-js/library/web/timers';

@Component({
  selector: 'app-signalr-docment-demo',
  templateUrl: './signalr-docment-demo.component.html',
  styleUrls: ['./signalr-docment-demo.component.css']
})
export class SignalrDocmentDemoComponent implements OnInit {

  documents: Immutable.OrderedMap<any, any> = Immutable.OrderedMap<any, any>();
  documentTypes: Immutable.OrderedMap<any, any> = Immutable.OrderedMap<any, any>();
  data: any = {};
  loading:boolean = false;
  display:string = 'grid';
  lastUpdated: Date;
  hub:string = 'DocumentsHub';
  pollTime:number = 1000 * 60 * 1; // microseconds*seconds*minutes ~ currently after every one minute
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
      setInterval(()=>{
        this.fetchDocs();
      });
    }
    this.events.on('start-listening-to-signalr', () => {
      this.fetchDocs();
      setInterval(()=>{
        this.fetchDocs();
      }, this.pollTime);
    });
  }

  fetchDocs() {
    this._ngZone.run(() => {
      this.loading = true;
    });
    this.signalR.getProxy(this.hub).value.server.getDocuments(this.data).then(result => {
       this._ngZone.run(() => {
        this.documents = Immutable.OrderedMap<any, any>();
        result.DocumentResults.forEach((doc: any) => {
          try {
            this.documents = this.documents.set(doc.DocumentId, doc);
          } catch (e) { console.log(e); }
        });
        this.lastUpdated = new Date();
        this.loading = false;
      });
      this._ngZone.run(() => {
        result.DocumentTypes.forEach((type: any) => {
          try {
            this.documentTypes = this.documentTypes.set(type.DocumentTypeId, type);
          } catch (e) { }
        });
      });
    });
  }

  openFile(file: any) {
    this.loading = true;
    localStorage.setItem('file-' + file.DocumentId, JSON.stringify(file));
    window.open('#/main/unindexed-images/file?id=' + file.DocumentId, '_blank');
    this.router.navigate(['/main/unindexed-images/new-index'], { queryParams: { id: file.DocumentId } });
  }

  get documentList(): Array<any> {
    return this.documents.toArray();
  }

}
