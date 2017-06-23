import { Component, OnInit } from '@angular/core';
import {HttpService} from "../../services/http-service";
import {User} from "../../models/user"

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users:Array<User>=[];
  pageNumber:number;
  pageSize:number =5;
  loading:boolean;

   constructor(private http:HttpService) {
    this.loading=false;
     this.getUsers(1)
   }

   next(){
     this.getUsers(this.pageNumber+1);
   }
   prev(){
     if(this.pageNumber>1){
      this.getUsers(this.pageNumber-1);
     }
   }

  ngOnInit() {
  }

  getUsers(pageNumber:number){
    this.loading = true;
    this.http.getUsers(pageNumber,this.pageSize).map(res=>{this.loading = false;return res.json()}).subscribe(result=>{
          this.users = result;
          this.pageNumber = pageNumber;
      },err=>{
        console.log(err);
      })
  }

}
