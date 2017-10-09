import { Component, OnInit,ViewContainerRef } from '@angular/core';
import {DiaryService,HttpService} from "../../services/services.barrel";
import {Diary} from "../../models/diary";
import {Claim} from "../../models/claim";
import {PrescriptionNote} from "../../models/prescription-note";
import {DiaryScriptNoteWindowComponent} from "../../components/components-barrel";
import {WindowsInjetor,CustomPosition,Size,WindowConfig} from "../ng-window";

@Component({
  selector: 'app-diary-results',
  templateUrl: './diary-results.component.html',
  styleUrls: ['./diary-results.component.css']
})
export class DiaryResultsComponent implements OnInit {
  constructor(public diaryService:DiaryService,private http:HttpService,
    private myInjector: WindowsInjetor,public viewContainerRef:ViewContainerRef) {
      
  }

  ngOnInit() {
  /*
  //Use only for offline test
  this.showNoteWindow(new PrescriptionNote(12,new Date(),"Josephat","Very Long Type","Let's say we wanted to implement an AJAX search feature in which every keypress in a text field will automatically perform a search and update the page with the results. How would this look? Well we would have an Observable subscribed to events coming from an input field, and on every change of input we want to perform some HTTP request, which is also an Observable we subscribe to. What we end up with is an Observable of an Observable.",new Date(),12312423));
  this.showNoteWindow(new PrescriptionNote(12,new Date(),"Josephat","Some Type","Some HTTP request, which is also an Observable we subscribe to. What we end up with is an Observable of an Observable.",new Date(),12312423));
  //this.showNoteWindow(new PrescriptionNote(12,new Date(),"Josephat","Very Long Type","Let's say we wanted to implement an AJAX search feature in which every keypress in a text field will automatically perform a search and update the page with the results. How would this look? Well we would have an Observable subscribed to events coming from an input field, and on every change of input we want to perform some HTTP request, which is also an Observable we subscribe to. What we end up with is an Observable of an Observable.",new Date(),12312423)); */
  }
  showNote(diary:Diary){
    this.diaryService.loading = true;
    this.http.getClaimsData({claimId:diary.claimId}).map(r=>{return r.json()})
    .subscribe((claim:Claim)=>{
      this.diaryService.loading = false;
      let note:PrescriptionNote = claim.prescriptionNotes.find((r:PrescriptionNote)=>r.prescriptionNoteId==diary.prescriptionNoteId);
      if(note){
          this.showNoteWindow(note);
      }
    },error=>{
      this.diaryService.loading = false;
    })
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

}
