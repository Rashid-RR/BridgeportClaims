import {Renderer2, Component, OnInit } from '@angular/core';
import { AccountReceivableService,ReportLoaderService,HttpService} from "../../services/services.barrel"
declare var $:any
@Component({
  selector: 'app-account-receivable-search',
  templateUrl: './account-receivable-search.component.html',
  styleUrls: ['./account-receivable-search.component.css']
})
export class AccountReceivableSearchComponent implements OnInit {
 
  private hashChange:Function;
  constructor(public http:HttpService,private renderer: Renderer2,public ar:AccountReceivableService,public reportloader:ReportLoaderService) {
    this.hashChange = this.renderer.listen('window', 'filteredList', (event) => {
      this.ar.filteredList = event['filteredList'];
    });
   }

  ngOnInit() {
    //The Calender
   // $("#calendar").datepicker();
  }
 
}
