import { Component, OnInit } from '@angular/core';
import { DocumentItem } from 'app/models/document';
import { Router } from "@angular/router";
import { HttpService } from "../../services/http-service"
import { DomSanitizer,SafeResourceUrl } from '@angular/platform-browser';
declare var $:any;

@Component({
  selector: 'app-unindexed-image-file',
  templateUrl: './unindexed-image-file.component.html',
  styleUrls: ['./unindexed-image-file.component.css']
})
export class UnindexedImageFileComponent implements OnInit {

  file: DocumentItem;
  loading: boolean = false;
  sanitizedURL:any;
  constructor(
    private router: Router,
    private http: HttpService,private sanitizer:DomSanitizer
  ) { }
  get sanitize():SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl('assets/js/pdfjs/web/viewer.html?url='+this.file.fileUrl);
  }
  ngOnInit() {
    var scale = 1.5;

    this.router.routerState.root.queryParams.subscribe(params => {
      if (params['id']) {
        let file = localStorage.getItem("file-" + params['id']);
        if (file) {
          this.file = JSON.parse(file) as DocumentItem;
        }
        console.log(window['PDFJS'], this.file);
        console.log(this.file);        
        var docInitParams: any = {};
        docInitParams.url = this.file.fileUrl;
        //console.log("Header", this.http.headers.get('authorization'));
        docInitParams.httpHeaders = { 'authorization': this.http.headers.get('authorization') };
        $("#fileCanvas").html('<iframe id="docCanvas" src="assets/js/pdfjs/web/viewer.html?url='+this.file.fileUrl+'" allowfullscreen style="width:100%;height:calc(100vh - 110px);border: none;"></iframe>');       
      }
    });
  }
}