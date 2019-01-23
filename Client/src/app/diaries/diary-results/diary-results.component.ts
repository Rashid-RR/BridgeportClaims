import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { DiaryService, HttpService } from '../../services/services.barrel';
import { Diary } from '../../models/diary';
import { Claim } from '../../models/claim';
import { PrescriptionNote } from '../../models/prescription-note';
import { WindowsInjetor, CustomPosition, Size, WindowConfig } from '../../components/ng-window';
import { Router } from '@angular/router';
import { Toast, ToastrService } from 'ngx-toastr';
import { ScriptNoteWindowComponent } from '../../components/diary-script-note-window/diary-script-note-window.component';

@Component({
  selector: 'app-diary-results',
  templateUrl: './diary-results.component.html',
  styleUrls: ['./diary-results.component.css']
})
export class DiaryResultsComponent implements OnInit {
  goToPage: any = '';
  activeToast: number;
  constructor(private _router: Router, public diaryService: DiaryService, private http: HttpService,
    private myInjector: WindowsInjetor, public viewContainerRef: ViewContainerRef, private toast: ToastrService) {

  }

  ngOnInit() {

  }
  showNote(diary: Diary) {
    window.open('#/main/claims?claimId=' + diary.claimId + '&prescriptionNoteId=' + diary.prescriptionNoteId, '_blank');
  }

  showNoteWindow(note: PrescriptionNote) {
    const config = new WindowConfig('Prescription Note', new Size(250, 600));  // height, width
    config.position = new CustomPosition(50 + Math.random() * 200, 100); // left,top
    config.minusTop = 91;
    config.centerInsideParent = false;
    const temp = {};
    config.forAny = [temp];
    config.openAsMaximize = false;
    this.myInjector.openWindow(ScriptNoteWindowComponent, config)
      .then((win: ScriptNoteWindowComponent) => {
        win.showNote(note);
      });
  }
  next() {
    this.diaryService.search(true);
  }
  prev() {
    this.diaryService.search(false, true);
  }

  goto() {
    const page = Number.parseInt(this.goToPage);
    if (!this.goToPage || isNaN(page)) {
      /* if(this.activeToast && this.activeToast.timeoutId){
        this.activeToast.message =  'Invalid page number entered'
        }else{
          this.toast.warning('Invalid page number entered').onHidden.subscribe((toast: Toast) => {
              this.activeToast = toast;
          })
      }*/
    } else if (page > 0 && ((this.diaryService.totalPages && page <= this.diaryService.totalPages) || this.diaryService.totalPages == null)) {
      this.diaryService.search(false, false, page);
    } else {
      let toast = this.toast.toasts.find(t=>t.toastId ==this.activeToast)
      if (toast) {
        toast.message = 'Page number entered is out of range. Enter a page number between 1 and ' + this.diaryService.totalPages;
      } else {
        this.activeToast = this.toast.warning('Page number entered is out of range. Enter a page number between 1 and ' + this.diaryService.totalPages).toastId
      }
    }
  }

  keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;
    const inputChar = String.fromCharCode(event.charCode);
    const input = Number(this.goToPage + '' + inputChar);
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
