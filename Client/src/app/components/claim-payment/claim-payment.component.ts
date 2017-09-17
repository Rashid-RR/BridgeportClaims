import { SortColumnInfo } from "../../directives/table-sort.directive";
import { Component, OnInit, Renderer2, AfterViewInit, NgZone, HostListener, AfterViewChecked, ElementRef, ViewChild } from '@angular/core';
import { ClaimManager } from "../../services/claim-manager";
import { HttpService } from "../../services/http-service";
import { Payment } from "../../models/payment";
import { EventsService } from "../../services/events-service";
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-claim-payment',
  templateUrl: './claim-payment.component.html',
  styleUrls: ['./claim-payment.component.css']
})
export class ClaimPaymentComponent implements OnInit {

  sortColumn:Array<SortColumnInfo>=[];
  payments:Array<Payment>=[];
  constructor(
    private rd: Renderer2, private ngZone: NgZone,
    private dp: DatePipe,
    public claimManager: ClaimManager,
    private events: EventsService,
    private http: HttpService
  ) { 
    this.fetchData();
    this.claimManager.onClaimIdChanged.subscribe(() => {
      this.fetchData();
    });
    
  }

  ngOnInit() {
     
  }

  onSortColumn(info: SortColumnInfo) {
    if(this.sortColumn.length==2 && this.sortColumn[1].column==info.column){
      this.sortColumn[1].dir=info.dir;
    }else if(this.sortColumn.length==1 && this.sortColumn[0].column==info.column){
      this.sortColumn[0].dir=info.dir;
    }else {
      this.sortColumn.push(info);
    }
    if(this.sortColumn.length>2){
      this.sortColumn=[this.sortColumn[this.sortColumn.length-2],this.sortColumn[this.sortColumn.length-1]];
    }
    console.log(this.sortColumn);
    this.fetchData();
  }

  fetchData() {
    let page = 1;
    let page_size = 1000;
    let sort: string = 'RxDate';
    let sort_dir: 'asc' | 'desc' = 'desc'; 
    let sort2: string = 'RxNumber';
    let sort_dir2: 'asc' | 'desc' = 'asc';
    if (this.sortColumn[0]) {
      sort = this.sortColumn[0].column;
      sort_dir = this.sortColumn[0].dir;
    }
    if (this.sortColumn[1]) {
      sort2 = this.sortColumn[1].column;
      sort_dir2 = this.sortColumn[1].dir;
    }
    this.http.getPayments(this.claimManager.selectedClaim.claimId, sort, sort_dir,sort2, sort_dir2,
      page, page_size).map(p => p.json())
      .subscribe(results => {
        console.log(results);
        this.payments = results;
      });
  }

}
