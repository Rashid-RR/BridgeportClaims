import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { EpisodeService, HttpService } from "../../services/services.barrel";
import { Diary } from "../../models/diary";
import { Claim } from "../../models/claim";
import { PrescriptionNote } from "../../models/prescription-note";
import { DiaryScriptNoteWindowComponent } from "../../components/components-barrel";
import { WindowsInjetor, CustomPosition, Size, WindowConfig } from "../ng-window";
import { Router } from "@angular/router";
import { Toast, ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
  selector: 'app-episode-results',
  templateUrl: './episode-results.component.html',
  styleUrls: ['./episode-results.component.css']
})
export class EpisodeResultsComponent implements OnInit {

  goToPage: any = '';
  activeToast: Toast;
  constructor(private _router: Router, public episodeService: EpisodeService, private http: HttpService,
    private myInjector: WindowsInjetor, public viewContainerRef: ViewContainerRef, private toast: ToastsManager) {

  }

  ngOnInit() {
    this.episodeService.search();
  }

  next() {
    this.episodeService.search(true);
  }
  prev() {
    this.episodeService.search(false, true);
  }

  goto() {
    let page = Number.parseInt(this.goToPage);
    if (!this.goToPage || isNaN(page)) {
      /* if(this.activeToast && this.activeToast.timeoutId){
        this.activeToast.message =  'Invalid page number entered'
        }else{
          this.toast.warning('Invalid page number entered').then((toast: Toast) => {
              this.activeToast = toast;
          })
      }*/
    } else if (page > 0 && ((this.episodeService.totalPages && page <= this.episodeService.totalPages) || this.episodeService.totalPages == null)) {
      this.episodeService.search(false, false, page);
    } else {
      if (this.activeToast && this.activeToast.timeoutId) {
        this.activeToast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.episodeService.totalPages
      } else {
        this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.episodeService.totalPages).then((toast: Toast) => {
          this.activeToast = toast;
        })
      }
    }
  }

  keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;
    let inputChar = String.fromCharCode(event.charCode);
    let input = Number(this.goToPage + "" + inputChar);
    if (!pattern.test(inputChar)) {
      event.preventDefault();
    } else if (!this.isNumeric(input)) {
      event.preventDefault();
    } else if (input < 1) {
      event.preventDefault();
    }
  }
  isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
  }

}
