import { Component, OnInit } from '@angular/core';
import {HttpService} from "../../services/http-service"
import {Payor} from "../../models/payor"

@Component({
  selector: 'app-payors',
  templateUrl: './payors.component.html',
  styleUrls: ['./payors.component.css']
})
export class PayorsComponent implements OnInit {
  payors:Array<Payor>=[];
  pageNumber:number;
  pageSize:number =5;
  loading:boolean;
  constructor(private http:HttpService) {
    this.loading=false;
     this.getPayors(1)
   }

   next(){
     this.getPayors(this.pageNumber+1);
   }
   prev(){
     if(this.pageNumber>1){
      this.getPayors(this.pageNumber-1);
     }
   }

  ngOnInit() {
    
  }

  getPayors(pageNumber:number){
    this.loading = true;
    this.http.getPayours(pageNumber,this.pageSize).map(res=>{this.loading = false;return res.json()}).subscribe(result=>{
          this.payors = result;
          this.pageNumber = pageNumber;
      },err=>{
        console.log(err);
      })
  }

}
