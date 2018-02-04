import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { EpisodeService, HttpService } from '../../services/services.barrel';
import { Diary } from '../../models/diary';
import { Claim } from '../../models/claim';
import { PrescriptionNote } from '../../models/prescription-note';
import { DiaryScriptNoteWindowComponent } from '../../components/components-barrel';
import { WindowsInjetor, CustomPosition, Size, WindowConfig } from '../ng-window';
import { Router } from '@angular/router';
import { Toast, ToastsManager } from 'ng2-toastr/ng2-toastr';
import { DialogService } from 'ng2-bootstrap-modal';

import { ConfirmComponent } from '../../components/confirm.component';

@Component({
  selector: 'app-episode-results',
  templateUrl: './episode-results.component.html',
  styleUrls: ['./episode-results.component.css']
})
export class EpisodeResultsComponent implements OnInit {

  activeToast: Toast;
  constructor(
    private dialogService: DialogService,private _router: Router, public episodeService: EpisodeService, private http: HttpService,
    private myInjector: WindowsInjetor, public viewContainerRef: ViewContainerRef, private toast: ToastsManager) {

  }

  ngOnInit() {
    this.episodeService.search();
  }

  markAsResolved($event,episode){
    const disposable = this.dialogService.addDialog(ConfirmComponent, {
      title: 'Mark Episode as Resolved',
      message: 'Are you sure you with to resolve this episode?'
    })
      .subscribe((isConfirmed) => {
         if (isConfirmed) {
        this.episodeService.loading = true;
        this.http.markEpisodeAsSolved(episode.episodeId).map(r => { return r.json(); }).single().subscribe(res => {
          this.toast.success(res.message);
          this.episodeService.loading = false;
          this.episodeService.episodes = this.episodeService.episodes.delete(episode.episodeId);
        }, error => {
          this.toast.error(error.message);
          $event.target.checked = false;
          this.episodeService.loading = false;
        });
      } else {
        $event.target.checked = false;
       }
    });
  }
  next() {
    this.episodeService.search(true);
  }
  prev() {
    this.episodeService.search(false, true);
  }

  goto() {
    const page = Number.parseInt(this.episodeService.goToPage);
    if (!this.episodeService.goToPage || isNaN(page)) {
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
        this.activeToast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.episodeService.totalPages;
      } else {
        this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.episodeService.totalPages).then((toast: Toast) => {
          this.activeToast = toast;
        });
      }
    }
  }

  keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;
    const inputChar = String.fromCharCode(event.charCode);
    const input = Number(this.episodeService.goToPage + '' + inputChar);
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
