import { Component, Input, OnInit } from '@angular/core';
import { DocumentItem } from 'app/models/document';
import { Router } from "@angular/router";
import { HttpService } from "../../services/http-service"
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';

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
  private sub: any;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private http: HttpService, private sanitizer: DomSanitizer
  ) { }
  get sanitize(): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl('assets/js/pdfjs/web/viewer.html?url=' + this.file.fileUrl);
  }
  ngOnInit() {
    var scale = 1.5;
    if (this.file) {
      this.showFile();
    } else {
      this.sub = this.route.params.subscribe(params => {
        let file = localStorage.getItem('file-' + params['id']);
        if (file) {
          try {
            this.file = JSON.parse(file) as DocumentItem;
            this.showFile();
          } catch (e) { }
        }
      });
    }
  }
  showFile() {
    var docInitParams: any = {};
    docInitParams.url = this.file.fileUrl;
    docInitParams.httpHeaders = { 'authorization': this.http.headers.get('authorization') };
    $("#fileCanvas").html('<iframe id="docCanvas" src="assets/js/pdfjs/web/viewer.html?url=' + this.file.fileUrl + '" allowfullscreen style="width:100%;height:calc(100vh - 110px);border: none;"></iframe>');
  }
  ngOnDestroy() {
    this.sub.unsubscribe();
  }

}
