import {Component, OnInit,ViewContainerRef } from '@angular/core';
import {DiaryService,HttpService} from "../../services/services.barrel";
import {Diary} from "../../models/diary";
import {Claim} from "../../models/claim";
import {PrescriptionNote} from "../../models/prescription-note";
import {DiaryScriptNoteWindowComponent} from "../../components/components-barrel";
import {WindowsInjetor,CustomPosition,Size,WindowConfig} from "../ng-window";
import {Router} from "@angular/router";
import { Toast,ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
  selector: 'app-diary-results',
  templateUrl: './diary-results.component.html',
  styleUrls: ['./diary-results.component.css']
})
export class DiaryResultsComponent implements OnInit {
  goToPage:any;
  activeToast: Toast;
  constructor(private _router:Router,public diaryService:DiaryService,private http:HttpService,
    private myInjector: WindowsInjetor,public viewContainerRef:ViewContainerRef, private toast: ToastsManager) {
      
  }

  ngOnInit() {
  /*
  //Use only for offline test
  this.showNoteWindow(new PrescriptionNote(12,new Date(),"Josephat","Very Long Type","Let's say we wanted to implement an AJAX search feature in which every keypress in a text field will automatically perform a search and update the page with the results. How would this look? Well we would have an Observable subscribed to events coming from an input field, and on every change of input we want to perform some HTTP request, which is also an Observable we subscribe to. What we end up with is an Observable of an Observable.",new Date(),12312423));
  this.showNoteWindow(new PrescriptionNote(12,new Date(),"Josephat","Some Type","Some HTTP request, which is also an Observable we subscribe to. What we end up with is an Observable of an Observable.",new Date(),12312423));
  //this.showNoteWindow(new PrescriptionNote(12,new Date(),"Josephat","Very Long Type","Let's say we wanted to implement an AJAX search feature in which every keypress in a text field will automatically perform a search and update the page with the results. How would this look? Well we would have an Observable subscribed to events coming from an input field, and on every change of input we want to perform some HTTP request, which is also an Observable we subscribe to. What we end up with is an Observable of an Observable.",new Date(),12312423)); */
  }
  showNote(diary:Diary){
    window.open("#/main/claims?claimId="+diary.claimId+"&prescriptionNoteId="+diary.prescriptionNoteId, "_blank");     
  }

  showNoteWindow(note:PrescriptionNote){
    let config = new WindowConfig("Prescription Note", new Size(250, 600))  //height, width
    config.position=new CustomPosition(50+Math.random()*200, 100)//left,top
    config.minusTop = 91;      
    config.centerInsideParent=false;
    var temp={}
    config.forAny=[temp];
    config.openAsMaximize=false;
    this.myInjector.openWindow( DiaryScriptNoteWindowComponent,config)
    .then((win:DiaryScriptNoteWindowComponent) => {       
      win.showNote(note);
    })
  }
  next(){ 
    this.diaryService.search(true);
  }
  prev(){ 
      this.diaryService.search(false,true);
  }

  goto(){ 
    let page = Number.parseInt(this.goToPage);
    if(!this.goToPage || isNaN(page)){ 
      if(this.activeToast && this.activeToast.timeoutId){
        this.activeToast.message =  'Invalid page number entered'
      }else{
        this.toast.warning('Invalid page number entered').then((toast: Toast) => {
            this.activeToast = toast;
        })
      }
    }else if(page >0 && ((this.diaryService.totalPages && page<=this.diaryService.totalPages) || this.diaryService.totalPages==null)){
      this.diaryService.search(false,false,page);
    }else{
      if(this.activeToast && this.activeToast.timeoutId){
        this.activeToast.message = 'Page number entered is out of range'
      }else{
        this.toast.warning('Page number entered is out of range').then((toast: Toast) => {
            this.activeToast = toast;
        })
      } 
    }
  }



}
