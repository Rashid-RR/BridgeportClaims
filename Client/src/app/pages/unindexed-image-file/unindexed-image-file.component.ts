import { Component, Input, OnInit } from '@angular/core';
import { DocumentItem } from 'app/models/document';
import { Router } from "@angular/router";
import { Http } from "@angular/http";
import { HttpService } from "../../services/http-service"
import { EventsService } from "../../services/events-service"
import { DocumentManagerService } from "../../services/document-manager.service"
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { ToastsManager, Toast } from 'ng2-toastr/ng2-toastr';

declare var $: any;

@Component({
  selector: 'app-unindexed-image-file',
  templateUrl: './unindexed-image-file.component.html',
  styleUrls: ['./unindexed-image-file.component.css']
})
export class UnindexedImageFileComponent implements OnInit {

  loading: boolean = false;
  sanitizedURL: any;
  @Input() file: DocumentItem;
  constructor(
    private router: Router, private nativeHttp: Http,private ds:DocumentManagerService,
    private route: ActivatedRoute, private toast: ToastsManager,private events:EventsService,
    private http: HttpService, private sanitizer: DomSanitizer
  ) { }
  get sanitize(): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl('assets/js/pdfjs/web/viewer.html?url=' + this.file.fileUrl);
  }
  ngOnInit() {
    var scale = 1.5;
    if (this.file) {
      this.loading = true;
      this.nativeHttp.get(this.file.fileUrl).single().subscribe(r => {
        this.showFile();
        this.loading = false;
      }, err => {
        this.showFile();
        this.toast.error("Error, the PDF that you are looking for cannot be found. Please contact your system administrator.", null, { showCloseButton: true, dismiss: 'click' });
        this.loading = false;
      })
    } else {
      this.route.params.subscribe(params => {
        let file = localStorage.getItem('file-' + params['id']);
        if (file) {
          try {
            this.file = JSON.parse(file) as DocumentItem;
            this.loading = true;
            this.nativeHttp.get(this.file.fileUrl).single().subscribe(r => {
              this.showFile();
              this.loading = false;
            }, err => {
              this.showFile();
              this.toast.error("Error, the PDF that you are looking for cannot be found. Please contact your system administrator.", null, { showCloseButton: true, dismiss: 'click' });
              this.loading = false;
            })
          } catch (e) { }
        }
      });
    }
  }
  cancel() {
    this.events.broadcast("reset-indexing-form",true);
    this.ds.cancel();
  }
  showFile() {
    var docInitParams: any = {};
    docInitParams.url = this.file.fileUrl;
    docInitParams.httpHeaders = { 'authorization': this.http.headers.get('authorization') };
    $("#fileCanvas").html('<iframe id="docCanvas" src="assets/js/pdfjs/web/viewer.html?url=' + this.file.fileUrl + '" allowfullscreen style="width:100%;height:calc(100vh - 110px);border: none;"></iframe>');
    if (!this.file.fileUrl) {
      this.toast.error("Error, the PDF that you are looking for cannot be found. Please contact your system administrator.", null, { showCloseButton: true, dismiss: 'click' })
    }
  }


}
