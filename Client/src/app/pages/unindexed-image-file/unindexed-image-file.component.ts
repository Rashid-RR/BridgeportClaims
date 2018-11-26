import { Component, Input, OnInit } from '@angular/core';
import { DocumentItem } from '../../models/document';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { HttpService } from '../../services/http-service';
import { EventsService } from '../../services/events-service';
import { DocumentManagerService } from '../../services/document-manager.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { ToastsManager, Toast } from 'ng2-toastr';
import { Episode } from '../../interfaces/episode';
import { EpisodeNoteModalComponent } from '../../components/components-barrel';
import { WindowsInjetor, CustomPosition, Size, WindowConfig } from '../../components/ng-window';

declare var $: any;

@Component({
  selector: 'app-unindexed-image-file',
  templateUrl: './unindexed-image-file.component.html',
  styleUrls: ['./unindexed-image-file.component.css']
})
export class UnindexedImageFileComponent implements OnInit {

  loading = false;
  checkImage = false;
  sanitizedURL: any;
  @Input() file: any;
  constructor(
    public router: Router, private nativeHttp: HttpClient, private ds: DocumentManagerService,
    private route: ActivatedRoute, private toast: ToastsManager, private events: EventsService,
    private http: HttpService, private sanitizer: DomSanitizer, private myInjector: WindowsInjetor,
  ) { }
  get sanitize(): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl('assets/js/pdfjs/web/viewer.html?url=' + this.file.fileUrl);
  }
  showNoteWindow() {
    const config = new WindowConfig('Episode Note(s)', new Size(400, 700));  // height, width

    config.position = new CustomPosition((window.innerWidth - 700) / 2 + 50, 60); // left,top
    config.minusTop = 0;
    config.minusHeight = 0;
    config.minusLeft = 0;
    config.minusWidth = 0;
    config.centerInsideParent = false;
    const temp = {};
    config.forAny = [temp];
    config.openAsMaximize = false;
    this.myInjector.openWindow(EpisodeNoteModalComponent, config)
      .then((win: EpisodeNoteModalComponent) => {
        win.showNote(this.file);
      });
  }
  showNote(id: any) {
    const file = localStorage.getItem('file-' + id);
    this.checkImage = true;
    if (file) {
      try {
        this.file = JSON.parse(file) as any;
        this.loading = true;
        this.nativeHttp.get(decodeURIComponent(this.file.fileUrl), { observe: 'response', responseType: 'blob' }).single().subscribe(r => {
          this.showFile();
          this.loading = false;
        }, err => {
          this.showFile();
          this.toast.error('Error, the PDF that you are looking for cannot be found. Please contact your system administrator.', null, { showCloseButton: true, dismiss: 'click' });
          this.loading = false;
        });
      } catch (e) { }
    }
  }
  ngOnInit() {
    const scale = 1.5;
    if (this.file) {
      this.loading = true;
      const url = decodeURIComponent(this.file.fileUrl);
      this.nativeHttp.get(url, { observe: 'response', responseType: 'blob' }).single().subscribe(r => {
        this.showFile();
        this.loading = false;
      }, err => {
        this.showFile();
        this.toast.error('Error, the PDF that you are looking for cannot be found. Please contact your system administrator.', null, { showCloseButton: true, dismiss: 'click' });
        this.loading = false;
      });
    } else {
      this.route.params.subscribe(params => {
        const file = localStorage.getItem('file-' + params['id']);
        if (file) {
          try {
            this.file = JSON.parse(file) as any;
            this.loading = true;
            this.events.on('episode-note-updated', (episode) => {
              if (episode.episodeId === this.file.episodeId) {
                this.file.episodeNoteCount = episode.episodeNoteCount;
                localStorage.setItem('file-' + params['id'], JSON.stringify(this.file));
              }
            });
            this.nativeHttp.get(decodeURIComponent(this.file.fileUrl), { observe: 'response', responseType: 'blob' }).single().subscribe(r => {
              this.showFile();
              this.loading = false;
            }, err => {
              this.showFile();
              this.toast.error('Error, the PDF that you are looking for cannot be found. Please contact your system administrator.', null, { showCloseButton: true, dismiss: 'click' });
              this.loading = false;
            });
          } catch (e) { }
        }
      });
    }
  }
  cancel() {
    this.events.broadcast('reset-indexing-form', true);
    this.ds.cancel('image');
  }
  showFile() {
    const docInitParams: any = {};
    docInitParams.url = this.file.fileUrl;
    docInitParams.httpHeaders = { 'authorization': this.http.headers.get('authorization') };
    const minusHeight = this.router.url === '/main/unindexed-images' ? 300 : 110;
    $('#fileCanvas').html('<iframe id="docCanvas" src="assets/js/pdfjs/web/viewer.html?url=' + this.file.fileUrl + '" allowfullscreen style="width:100%;height:calc(100vh - ' + minusHeight + 'px);border: none;"></iframe>');
    if (!this.file.fileUrl) {
      this.toast.error('Error, the PDF that you are looking for cannot be found. Please contact your system administrator.', null, { showCloseButton: true, dismiss: 'click' });
    }
  }

  get isIndexedImage() {
    return this.router.url.indexOf('main/indexed-image') > -1;
  }


}
