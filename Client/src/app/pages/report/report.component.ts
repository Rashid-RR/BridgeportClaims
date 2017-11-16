import { Component, OnInit,OnDestroy } from '@angular/core';
import { Router,NavigationEnd,UrlTree } from '@angular/router';
// Services
import { ReportLoaderService } from "../../services/services.barrel";

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit ,OnDestroy{

  location:String='';
  constructor(private router:Router,public reportloader:ReportLoaderService) { 

  }

  ngOnInit() {
       
  }
  ngOnDestroy(){
      
  }

}
