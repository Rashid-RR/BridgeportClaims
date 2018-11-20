import { Injectable, NgZone } from '@angular/core';
import { HttpService } from './http-service';
import { EventsService } from './events-service';
import { ToastsManager } from 'ng2-toastr';
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import * as Immutable from 'immutable';
import swal from "sweetalert2";
import { SortColumnInfo } from "../directives/table-sort.directive";
import { DocumentItem } from '../models/document';
import { DocumentType } from '../models/document-type';
import { ProfileManager } from './profile-manager';
declare var $: any;

@Injectable()
export class DocumentManagerService {
  loading: Boolean = false;
  documents: Immutable.OrderedMap<any, DocumentItem> = Immutable.OrderedMap<any, DocumentItem>();
  checks: Immutable.OrderedMap<any, DocumentItem> = Immutable.OrderedMap<any, DocumentItem>();
  invalidChecks: Immutable.OrderedMap<any, DocumentItem> = Immutable.OrderedMap<any, DocumentItem>();
  invoices: Immutable.OrderedMap<any, DocumentItem> = Immutable.OrderedMap<any, DocumentItem>();
  documentTypes: Immutable.OrderedMap<any, DocumentType> = Immutable.OrderedMap<any, DocumentType>();
  data: any = {};
  invoiceData: any = {};
  postedChecksData: any = {};
  archivedChecksData: any = {};
  postedChecks: boolean = false;
  viewPostedDetail: any;
  invalidChecksData: any = {};
  display: string = 'list';
  invDisplay: string = 'list';
  checkDisplay: string = 'list';
  invalidCheckDisplay: string = 'list';
  totalRowCount: number;
  totalInvoiceRowCount: number;
  totalCheckRowCount: number;
  totalInvalidCheckRowCount: number;
  searchText: string = '';
  newIndex: boolean = false;
  invoiceArchived: boolean = false;
  checksArchived: boolean = false;
  invalidChecksArchived: boolean = false;
  imagesArchived: boolean = false;
  newInvoice: boolean = false;
  newCheck: boolean = false;
  indexNewCheck: boolean = true;
  file: DocumentItem;
  invoiceFile: DocumentItem;
  checksFile: DocumentItem;
  exactMatch: boolean = false;
  constructor(private profileManager: ProfileManager, private http: HttpService, private formBuilder: FormBuilder, private _ngZone: NgZone,
    private events: EventsService, private toast: ToastsManager) {
    this.data = {
      date: null,
      isIndexed: false,
      sort: "DocumentID",
      sortDirection: "ASC",
      page: 1,
      fileTypeId: 1,
      pageSize: 30
    };
    this.invoiceData = {
      date: null,
      fileTypeId: 2,
      isIndexed: false,
      sort: "DocumentID",
      sortDirection: "ASC",
      page: 1,
      pageSize: 30
    };
    this.archivedChecksData = {
      date: null,
      fileTypeId: 3,
      isIndexed: false,
      sort: "DocumentID",
      sortDirection: "ASC",
      page: 1,
      pageSize: 30
    };
    this.postedChecksData = {
      date: null,
      fileName: null,
      isIndexed: false,
      sort: "IndexedOn",
      sortDirection: "ASC",
      page: 1,
      pageSize: 30
    };
    this.invalidChecksData = {
      fileName: null,
      date: null,
      sort: "FileName",
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
            this.toast.success(this.documents.get(id).fileName + ' image was just archived...');
          }
          this.documents = this.documents.delete(id);
          this.totalRowCount--;
        }, 4000)
      }
    })
    this.events.on("indexed-image", (id: any) => {
      let document = this.documents.get(id);
      if (document) {
        let doc = JSON.parse(JSON.stringify(document));//copy document
        document.edited = true;
        this.documents = this.documents.set(id, document);
        setTimeout(() => {
          if (this.adminOrAsociate) {
            this.toast.success(doc.fileName + ' has been indexed...');
          }
          this.documents = this.documents.delete(id);
          this.totalRowCount--;
        }, 4000)
      }
    })
    this.search();
    this.searchInvoices();
    this.searchCheckes();
    this.searchInvalidCheckes();
  }

  get autoCompleteClaim(): string {
    return this.http.baseUrl + "/document/claim-search/?exactMatch=" + this.exactMatch + "&searchText=:keyword";
  }

  get checksData() {
    return this.postedChecks ? this.postedChecksData : this.archivedChecksData;;
  }

  onSortColumn(info: SortColumnInfo) {
    this.data.isDefaultSort = false;
    this.data.sort = info.column;
    this.data.page = 1;
    this.data.sortDirection = info.dir.toUpperCase();
    this.search();
  }
  onInvoiceSortColumn(info: SortColumnInfo) {
    this.invoiceData.isDefaultSort = false;
    this.invoiceData.sort = info.column;
    this.invoiceData.page = 1;
    this.invoiceData.sortDirection = info.dir.toUpperCase();
    this.searchInvoices();
  }
  onCheckSortColumn(info: SortColumnInfo) {
    let data = this.postedChecks ? this.postedChecksData : this.archivedChecksData;;
    data.isDefaultSort = false;
    data.sort = info.column;
    data.page = 1;
    data.sortDirection = info.dir.toUpperCase();
    this.searchCheckes();
  }
  onInvalidCheckSortColumn(info: SortColumnInfo) {
    this.invalidChecksData.isDefaultSort = false;
    this.invalidChecksData.sort = info.column;
    this.invalidChecksData.page = 1;
    this.invalidChecksData.sortDirection = info.dir.toUpperCase();
    this.searchInvalidCheckes();
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
  archive(id: number, invoice: boolean = false, method: string = '') {
    this.loading = true;
    this.http.archiveDocument(id).subscribe(r => {
      this.loading = false;
      this.toast.success(r.message);
      this.cancel(invoice ? 'invoice' : 'image')
      if (invoice) {
        this.invoices = this.invoices.delete(id);
      } else {
        this.documents = this.documents.delete(id);
      }
      if (method) {
        this[method].apply(this)
      }
    }, () => {
      this.loading = false;
    });
  }
  deleteAndKeep(id: number, skipPayments: boolean = false,prescriptionPaymentId?:any) {
    console.log('delete and keep')
    this.loading = true;
     this.http.reIndexedCheck({documentId:id,skipPayments:skipPayments,prescriptionPaymentId:prescriptionPaymentId}).subscribe(r => {
      this.loading = false;
      this.toast.success(r.message);
      this.checks = this.checks.delete(id);
    }, () => {
      this.loading = false;
    });
  }
  viewPosted(id: any) {
    this.loading = true;
     this.http.getIndexedCheckDetail(id).subscribe(r => {
      this.loading = false;
      this.viewPostedDetail = r;
    }, () => {
      this.loading = false;
    });
  }
  cancel(type) {
    switch (type) {
      case 'image':
        this.newIndex = false;
        this.file = undefined;
        break;
      case 'invoice':
        this.newInvoice = false;
        this.invoiceFile = undefined;
        break;
      case 'checks':
      case 'check':
        this.newCheck = false;
        this.checksFile = undefined;
        setTimeout(() => {
          $(`.nav-stacked a[href="#${(this.indexNewCheck ? 'checksA' : 'checksB')}"]`).tab('show');
        }, 200);
        break;
    }
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
      this.http.getDocuments(data)
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
          (result.documentTypes || []).forEach((type: DocumentType) => {
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
          this.imagesArchived = this.data.archived;
        }, err => {
          this.loading = false;
          try {
            const error = err.error;
          } catch (e) { }
        }, () => {
          this.events.broadcast('document-list-updated');
        });
    }
  }
  searchInvoices(next: Boolean = false, prev: Boolean = false, page: number = undefined) {
    if (!this.invoiceData) {
      if (this.adminOrAsociate) {
        this.toast.warning('Please populate at least one search field.');
      }
    } else {
      this.loading = true;
      let invoiceData = JSON.parse(JSON.stringify(this.invoiceData)); //copy invoiceData instead of memory referencing
      if (next) {
        invoiceData.page++;
      }
      if (prev && invoiceData.page > 1) {
        invoiceData.page--;
      }
      if (page) {
        invoiceData.page = page;
      }
      this.http.getDocuments(invoiceData)
        .subscribe((result: any) => {
          //console.log(result);
          this.loading = false;
          this.totalInvoiceRowCount = result.totalRowCount;
          this.invoices = Immutable.OrderedMap<any, DocumentItem>();
          result.documentResults.forEach((doc: DocumentItem) => {
            try {
              this.invoices = this.invoices.set(doc.documentId, doc);
            } catch (e) { }
          });
          (result.documentTypes || []).forEach((type: DocumentType) => {
            try {
              this.documentTypes = this.documentTypes.set(type.documentTypeId, type);
            } catch (e) { }
          });
          if (next) {
            this.invoiceData.page++;
          }
          if (prev && this.invoiceData.page != invoiceData.page) {
            this.invoiceData.page--;
          }
          if (page) {
            this.invoiceData.page = page;
          }
          this.invoiceArchived = this.invoiceData.archived;
        }, () => {
          this.loading = false;
        }, () => {
          this.events.broadcast('document-list-updated');
        });
    }
  }
  searchCheckes(next: Boolean = false, prev: Boolean = false, page: number = undefined) {
    if (!this.checksData) {
      if (this.adminOrAsociate) {
        this.toast.warning('Please populate at least one search field.');
      }
    } else {
      this.loading = true;
      let checksData = JSON.parse(JSON.stringify(this.checksData)); //copy checksData instead of memory referencing
      if (next) {
        checksData.page++;
      }
      if (prev && checksData.page > 1) {
        checksData.page--;
      }
      if (page) {
        checksData.page = page;
      }

      let apiCall = this.postedChecks ? this.http.getIndexedChecks(checksData) : this.http.getDocuments(checksData)
      apiCall.subscribe((result: any) => {
        //console.log(result);
        this.loading = false;
        this.totalCheckRowCount = result.totalRowCount;
        this.checks = Immutable.OrderedMap<any, DocumentItem>();
        (result.documentResults || result.results || []).forEach((doc: DocumentItem) => {
          try {
            this.checks = this.checks.set(doc.documentId, doc);
          } catch (e) { }
        });
        (result.documentTypes || []).forEach((type: DocumentType) => {
          try {
            this.documentTypes = this.documentTypes.set(type.documentTypeId, type);
          } catch (e) { }
        });
        if (next) {
          this.checksData.page++;
        }
        if (prev && this.checksData.page != checksData.page) {
          this.checksData.page--;
        }
        if (page) {
          this.checksData.page = page;
        }
        this.checksArchived = this.checksData.archived;
      }, err => {
        this.loading = false;
        try {
          const error = err.error;
        } catch (e) { }
      }, () => {
        this.events.broadcast('document-list-updated');
      });
    }
  }
  searchInvalidCheckes(next: Boolean = false, prev: Boolean = false, page: number = undefined) {
    if (!this.invalidChecksData) {
      if (this.adminOrAsociate) {
        this.toast.warning('Please populate at least one search field.');
      }
    } else {
      this.loading = true;
      let invalidChecksData = JSON.parse(JSON.stringify(this.invalidChecksData)); //copy invalidChecksData instead of memory referencing
      if (next) {
        invalidChecksData.page++;
      }
      if (prev && invalidChecksData.page > 1) {
        invalidChecksData.page--;
      }
      if (page) {
        invalidChecksData.page = page;
      }
      this.http.getInvalidChecks(invalidChecksData)
        .subscribe((result: any) => {
          //console.log(result);
          this.loading = false;
          this.totalInvalidCheckRowCount = result.totalRowCount || (result.documentResults && result.documentResults.length) || 0;
          this.invalidChecks = Immutable.OrderedMap<any, DocumentItem>();
          result.documentResults.forEach((doc: DocumentItem) => {
            try {
              this.invalidChecks = this.invalidChecks.set(doc.documentId, doc);
            } catch (e) { }
          });
          (result.documentTypes || []).forEach((type: DocumentType) => {
            try {
              this.documentTypes = this.documentTypes.set(type.documentTypeId, type);
            } catch (e) { }
          });
          if (next) {
            this.invalidChecksData.page++;
          }
          if (prev && this.invalidChecksData.page != invalidChecksData.page) {
            this.invalidChecksData.page--;
          }
          if (page) {
            this.invalidChecksData.page = page;
          }
          this.invalidChecksArchived = this.invalidChecksData.archived;
        }, err => {
          this.loading = false;
          try {
            const error = err.error;
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
  get invPages(): Array<any> {
    return new Array(this.invoiceData.page);
  }
  get invoiceList(): Array<DocumentItem> {
    return this.invoices.toArray();
  }
  get invPageStart() {
    return this.invoiceList.length > 1 ? ((this.invoiceData.page - 1) * this.invoiceData.pageSize) + 1 : null;
  }
  get invPageEnd() {
    return this.invoiceList.length > 1 ? (this.invoiceData.pageSize > this.invoiceList.length ? ((this.invoiceData.page - 1) * this.invoiceData.pageSize) + this.invoiceList.length : (this.invoiceData.page) * this.invoiceData.pageSize) : null;
  }
  get invTotalPages() {
    return this.totalInvoiceRowCount ? Math.ceil(this.totalInvoiceRowCount / this.invoiceData.pageSize) : 0;
  }
  get invEnd(): Boolean {
    return this.invPageStart && this.invoiceData.pageSize > this.invoiceList.length;
  }
  get checkPages(): Array<any> {
    return new Array(this.checksData.page);
  }
  get checksList(): Array<DocumentItem> {
    return this.checks.toArray();
  }
  get checkPageStart() {
    return this.checksList.length > 1 ? ((this.checksData.page - 1) * this.checksData.pageSize) + 1 : null;
  }
  get checkPageEnd() {
    return this.checksList.length > 1 ? (this.checksData.pageSize > this.checksList.length ? ((this.checksData.page - 1) * this.checksData.pageSize) + this.checksList.length : (this.checksData.page) * this.checksData.pageSize) : null;
  }
  get checkTotalPages() {
    return this.totalCheckRowCount ? Math.ceil(this.totalCheckRowCount / this.checksData.pageSize) : 0;
  }
  get checkEnd(): Boolean {
    return this.checkPageStart && this.checksData.pageSize > this.checksList.length;
  }
  get invalidCheckPages(): Array<any> {
    return new Array(this.invalidChecksData.page);
  }
  get invalidChecksList(): Array<DocumentItem> {
    return this.invalidChecks.toArray();
  }
  get invalidCheckPageStart() {
    return this.invalidChecksList.length > 1 ? ((this.invalidChecksData.page - 1) * this.invalidChecksData.pageSize) + 1 : null;
  }
  get invalidCheckPageEnd() {
    return this.invalidChecksList.length > 1 ? (this.invalidChecksData.pageSize > this.invalidChecksList.length ? ((this.invalidChecksData.page - 1) * this.invalidChecksData.pageSize) + this.invalidChecksList.length : (this.invalidChecksData.page) * this.invalidChecksData.pageSize) : null;
  }
  get invalidCheckTotalPages() {
    return this.totalInvalidCheckRowCount ? Math.ceil(this.totalInvalidCheckRowCount / this.invalidChecksData.pageSize) : 0;
  }
  get invalidCheckEnd(): Boolean {
    return this.invalidCheckPageStart && this.invalidChecksData.pageSize > this.invalidChecksList.length;
  }
  closeModal() {
    try { swal.clickCancel(); } catch (e) { }
  }
}
