import {Component, OnInit,ViewContainerRef } from '@angular/core';
import {UnpaidScriptService,HttpService} from "../../services/services.barrel";
import {Diary} from "../../models/diary";
import {Claim} from "../../models/claim";
import {PrescriptionNote} from "../../models/prescription-note";
import {DiaryScriptNoteWindowComponent} from "../../components/components-barrel";
import {WindowsInjetor,CustomPosition,Size,WindowConfig} from "../ng-window";
import {Router} from "@angular/router";
import { Toast,ToastsManager } from 'ng2-toastr/ng2-toastr';

@Component({
  selector: 'app-unpaid-script-result',
  templateUrl: './unpaid-script-result.component.html',
  styleUrls: ['./unpaid-script-result.component.css']
})
export class UnpaidScriptResultsComponent implements OnInit {
  goToPage:any;
  activeToast: Toast;
  constructor(private _router:Router,public uss:UnpaidScriptService,private http:HttpService,
    private myInjector: WindowsInjetor,public viewContainerRef:ViewContainerRef, private toast: ToastsManager) {
      
  }

  ngOnInit() {
    
  }
  next(){ 
      this.uss.search(true);
  }
  goto(){ 
    let page = Number.parseInt(this.goToPage);
    if(!this.goToPage){ 
       
    }else if(page >0 && page<=this.uss.totalPages){
      this.uss.search(false,false,page);
    }else{
      if(this.activeToast && this.activeToast.timeoutId){
        this.activeToast.message = 'Page number entered is out of range. Enter a page number between 1 and '+this.uss.totalPages
      }else{
        this.toast.warning('Page number entered is out of range. Enter a page number between 1 and '+this.uss.totalPages).then((toast: Toast) => {
            this.activeToast = toast;
        })
      } 
    }
  }
  prev(){ 
      this.uss.search(false,true);
  }
  keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;
    let inputChar = String.fromCharCode(event.charCode);
    if ((event.keyCode != 8 && !pattern.test(inputChar)) || (Number(inputChar)>this.uss.totalPages || Number(inputChar)<1)) {
      event.preventDefault();
    }
  }
  
}
