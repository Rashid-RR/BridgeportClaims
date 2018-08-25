import { Component, OnInit,AfterViewInit } from '@angular/core';
import { HttpService } from '../../services/http-service';
import { ProfileManager, ReportLoaderService, EventsService } from '../../services/services.barrel';
import { DatePipe,DecimalPipe } from '@angular/common';
declare var Highcharts:any; 

@Component({
  selector: 'app-report-sample',
  templateUrl: './report-sample.component.html',
  styleUrls: ['./report-sample.component.css']
})
export class ReportSampleComponent implements OnInit ,AfterViewInit{
    preload = 'auto';
    categories:Array<any>=[];
    data:Array<any>=[];
    constructor(
      private http: HttpService,
      private events: EventsService,
      private dp: DatePipe,
      private reportLoader:ReportLoaderService,
      private profileManager: ProfileManager
    ) { }
  
    ngAfterViewInit() { 
      if(this.allowed){
        this.reportLoader.loading = true;
        this.http.revenueByMonth({}).subscribe((res:Array<{datePosted:Date,totalPosted:Number}>)=>{
          let drilldown:any=[];
          res.forEach(r=>{
            let date = this.dp.transform(r.datePosted, "M/d");
            this.categories.push(date);
            /* 
            For testing
            if(r.totalPosted==363872.7){
              r.totalPosted=1917;
            } */
            this.data.push({
              name:date,
              drilldown:date,
              y:r.totalPosted
            });
            drilldown.push({
              name:date,
              id:date,
              data:[[r.totalPosted]]
            });
          })
          var chart = Highcharts.chart('container', {      
              title: {
                  text: 'Revenue From Last '+this.data.length+' Days'
              },
              chart: {
                type: 'column'
              },
              credits: {
                enabled: false
              },
              plotOptions: {
                  series: {
                      borderWidth: 0,
                      dataLabels: {
                          enabled: true,
                          format: '${point.y:.2f}'
                      }
                  }
              },
              yAxis: [{
                  title: {
                      text: 'Totals Posted'
                  }
              }],
              xAxis: {
                  categories: this.categories,
                  title: {
                    text: 'Date'
                }
              },
              legend: {
                enabled: false
              },
              tooltip: {
                headerFormat: '',
                pointFormat: '<span style="color:{point.color}">Amount</span>: <b>${point.y:.2f}</b><br/>'
            },
              series: [{
                  type: 'column',
                  name: 'Revenue by Month',
                  colorByPoint: true,
                  data: this.data,
              }],
              drilldown:{
                series:drilldown     
              }
          });
          this.reportLoader.loading = false;
        },error=>{
          this.reportLoader.loading = false;
        })
      }
    }
    ngOnInit() { 
      this.reportLoader.current ='Revenue From Last 21 Days';
      this.reportLoader.currentURL ='revenue';
       
    }
  
    get allowed(): Boolean {
      return (this.profileManager.profile && this.profileManager.profile.roles && (this.profileManager.profile.roles instanceof Array)
      && this.profileManager.profile.roles.indexOf('Admin') > -1);
    }
  
  }
  