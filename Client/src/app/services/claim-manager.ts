
import {UUID} from "angular2-uuid";
import * as Immutable from "immutable";
import {Observable} from "rxjs/Observable";
import {Claim} from "../models/claim" 
import {Injectable} from "@angular/core";
import {HttpService} from "./http-service";
import {EventsService} from "./events-service";
  
@Injectable()
export class ClaimManager{
  private claims: Array<Claim> =[];
  
  constructor(private http:HttpService,private events:EventsService) {
       
  }
  
  search(data){
    this.http.getClaimsData(data).map(res=>{return res.json()})
    .subscribe(result=>{
        this.claims = result;
        console.log(result);
      },err=>{
        console.log(err);
      })
  }
}
