import {Component, OnInit,ViewContainerRef } from '@angular/core';
import {UnpaidScriptService,HttpService} from "../../services/services.barrel";
import {Diary} from "../../models/diary";
import {Claim} from "../../models/claim";
import {PrescriptionNote} from "../../models/prescription-note";
import {DiaryScriptNoteWindowComponent} from "../../components/components-barrel";
import {WindowsInjetor,CustomPosition,Size,WindowConfig} from "../ng-window";
import {Router} from "@angular/router";

@Component({
  selector: 'app-unpaid-script-result',
  templateUrl: './unpaid-script-result.component.html',
  styleUrls: ['./unpaid-script-result.component.css']
})
export class UnpaidScriptResultsComponent implements OnInit {
  constructor(private _router:Router,public uss:UnpaidScriptService,private http:HttpService,
    private myInjector: WindowsInjetor,public viewContainerRef:ViewContainerRef) {
      
  }

  ngOnInit() {
    
  }
  next(){
      this.uss.data.page++;
      this.uss.search();
  }
  prev(){
    if(this.uss.data.page>1){
      this.uss.data.page--;
      this.uss.search();
    }
  }
 

}
