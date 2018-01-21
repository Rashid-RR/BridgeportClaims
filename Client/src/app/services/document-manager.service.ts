import { Injectable, NgZone } from '@angular/core';
import { HttpService } from './http-service';
import { EventsService } from './events-service';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { UUID } from 'angular2-uuid';
import * as Immutable from 'immutable';
import { SortColumnInfo } from "../directives/table-sort.directive";
import { DocumentItem } from '../models/document';
import { DocumentType } from '../models/document-type';
import { ProfileManager } from './profile-manager';

@Injectable()
export class DocumentManagerService {
  loading: Boolean = false;
  documents: Immutable.OrderedMap<any, DocumentItem> = Immutable.OrderedMap<any, DocumentItem>();
  documentTypes: Immutable.OrderedMap<any, DocumentType> = Immutable.OrderedMap<any, DocumentType>();
  data: any = {};
  display: string = 'list';
  totalRowCount: number;
  searchText: string = '';
  newIndex: boolean = false;
  file: DocumentItem;
  exactMatch: boolean = false;
  constructor(private profileManager: ProfileManager, private http: HttpService, private formBuilder: FormBuilder, private _ngZone: NgZone,
    private events: EventsService, private toast: ToastsManager) {
    this.data = {
      date: null,
      isIndexed: false,
      sort: "DocumentID",
      sortDirection: "ASC",
      page: 1,
      pageSize: 30
    };

    this.events.on("new-image", (doc: DocumentItem) => {
      setTimeout(() => {
        if (!this.documents.get(doc.documentId)) {
          this.totalRowCount++;
        }
        doc.added = true;
        this.documents = this.documents.set(doc.documentId, doc);
        if (this.adminOrAsociate) {
          this.toast.info(doc.fileName + ' was added...');
        }
      }, 50);
      setTimeout(() => {
        this.documents.get(doc.documentId).added = false;
      }, 3500)
    })
    this.events.on("modified-image", (doc: DocumentItem) => {
      let document = this.documents.get(doc.documentId)
      setTimeout(() => {
        if (!document) {
          this.totalRowCount++;
          doc.added = true;
        } else {
          doc.edited = true;
        }
        this.documents = this.documents.set(doc.documentId, doc);
        if (this.adminOrAsociate) {
          this.toast.info(doc.fileName + ' was modified...');
        }
      }, 50);
      setTimeout(() => {
        doc.edited = false;
        doc.added = false;
        this.documents = this.documents.set(doc.documentId, doc);
      }, 4000)
    })
    this.events.on("deleted-image", (id: any) => {
      let document = this.documents.get(id)
      if (document) {
        document.deleted = true;
        this.documents = this.documents.set(id, document);
        setTimeout(() => {
          if (this.adminOrAsociate) {
            this.toast.info(this.documents.get(id).fileName + ' was deleted...');
          }
          this.documents = this.documents.delete(id);
          this.totalRowCount--;
        }, 4000)
      }
    })
    this.events.on("archived-image", (id: any) => {
      let document = this.documents.get(id)
      if (document) {
        document.deleted = true;
        this.documents = this.documents.set(id, document);
        setTimeout(() => {
          if (this.adminOrAsociate) {
            this.toast.success(this.documents.get(id).fileName + ' has been indexed...');
          }
          this.documents = this.documents.delete(id);
          this.totalRowCount--;
        }, 4000)
      }
    })
    this.events.on("indexed-image", (id: any) => {
      let document = this.documents.get(id)
      if (document) {
        document.deleted = true;
        this.documents = this.documents.set(id, document);
        setTimeout(() => {
          if (this.adminOrAsociate) {
            this.toast.success(this.documents.get(id).fileName + ' image was just archived...');
          }
          this.documents = this.documents.delete(id);
          this.totalRowCount--;
        }, 4000)
      }
    })
    this.search();
  }

  get autoCompleteClaim(): string {
    return this.http.baseUrl + "/document/claim-search/?exactMatch=" + this.exactMatch + "&searchText=:keyword";
  }

  onSortColumn(info: SortColumnInfo) {
    this.data.isDefaultSort = false;
    this.data.sort = info.column;
    this.data.page = 1;
    this.data.sortDirection = info.dir.toUpperCase();
    this.search();
  }
  get pages(): Array<any> {
    return new Array(this.data.page);
  }
  get documentList(): Array<DocumentItem> {
    return this.documents.toArray();
  }
  get pageStart() {
    return this.documentList.length > 1 ? ((this.data.page - 1) * this.data.pageSize) + 1 : null;
  }
  get pageEnd() {
    return this.documentList.length > 1 ? (this.data.pageSize > this.documentList.length ? ((this.data.page - 1) * this.data.pageSize) + this.documentList.length : (this.data.page) * this.data.pageSize) : null;
  }
  get totalPages() {
    return this.totalRowCount ? Math.ceil(this.totalRowCount / this.data.pageSize) : 0;
  }
  get documentTypesList(): DocumentType[] {
    return this.documentTypes.toArray();
  }
  get end(): Boolean {
    return this.pageStart && this.data.pageSize > this.documentList.length;
  }
  archive(id: number) {
    this.loading = true;
    this.http.archiveDocument(id).map(r => r.json()).subscribe(r => {
      this.loading = false;
      this.toast.success(r.message);
    }, err => null);
  }
  cancel() {
    this.newIndex = false;
    this.file = undefined;
  }
  search(next: Boolean = false, prev: Boolean = false, page: number = undefined) {
    if (!this.data) {
      if (this.adminOrAsociate) {
        this.toast.warning('Please populate at least one search field.');
      }
    } else {
      this.loading = true;
      let data = JSON.parse(JSON.stringify(this.data)); //copy data instead of memory referencing

      if (next) {
        data.page++;
      }
      if (prev && data.page > 1) {
        data.page--;
      }
      if (page) {
        data.page = page;
      }
      this.http.getDocuments(data).map(res => { return res.json(); })
        .subscribe((result: any) => {
          //console.log(result);
          this.loading = false;
          this.totalRowCount = result.totalRowCount;
          this.documents = Immutable.OrderedMap<any, DocumentItem>();
          result.documentResults.forEach((doc: DocumentItem) => {
            try {
              this.documents = this.documents.set(doc.documentId, doc);
            } catch (e) { }
          });
          result.documentTypes.forEach((type: DocumentType) => {
            try {
              this.documentTypes = this.documentTypes.set(type.documentTypeId, type);
            } catch (e) { }
          });
          if (next) {
            this.data.page++;
          }
          if (prev && this.data.page != data.page) {
            this.data.page--;
          }
          if (page) {
            this.data.page = page;
          }
        }, err => {
          this.loading = false;
          try {
            const error = err.json();
          } catch (e) { }
        }, () => {
          this.events.broadcast('document-list-updated');
        });
    }
  }
  get allowed(): Boolean {
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array) && this.profileManager.profile.roles.indexOf('Admin') > -1)
  }
  get adminOrAsociate(): Boolean {
    return (this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array) && (this.profileManager.profile.roles.indexOf('Admin') > -1 || this.profileManager.profile.roles.indexOf('Indexer') > -1));
  }


}
