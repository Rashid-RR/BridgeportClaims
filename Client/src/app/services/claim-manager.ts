
import {UUID} from "angular2-uuid";
import * as Immutable from "immutable";
import {Observable} from "rxjs/Observable";
import {Claim} from "../models/claim" 
import {Injectable} from "@angular/core";
import {HttpService} from "./http-service";
import {EventsService} from "./events-service";
  
@Injectable()
export class ClaimManager{
  private claims: Immutable.OrderedMap<Number, Claim> = Immutable.OrderedMap<Number, Claim>();
  selected:Number;
  constructor(private http:HttpService,private events:EventsService) {
       
  }
  
  search(data){    
    this.http.getClaimsData(data).map(res=>{return res.json()})
    .subscribe((result:Array<Claim>)=>{
        //this.claims = result;
        this.claims = Immutable.OrderedMap<Number, Claim>();
        result.forEach(claim=>{
         var c = new Claim(claim.claimId,claim.claimNumber,claim.dateEntered,claim.injuryDate,claim.gender,
         claim.carrier,claim.adjustor,claim.adjustorPhoneNumber,claim.dateEntered,claim.adjustorPhoneNumber
         ,claim.name,claim.firstName,claim.lastName);
         this.claims = this.claims.set(claim.claimId,c);
        })
        //console.log(result);
      },err=>{
        console.log(err);
      })
  }

  get dataSize(){
    return this.claims.size;
  }
  get claimsData():Claim []{
    return this.claims.asImmutable().toArray();
  }

  getClaimsDataById(id:Number){
      this.selected = id;
      var claim:Claim = this.claims.get(id) as Claim; 
      this.http.getClaimsData({claimId:id}).map(res=>{return res.json()})
        .subscribe(result=>{
            claim.setPrescription(result.prescriptions); 
            claim.setPayment(result.payments);
            claim.setEpisodes(result.episodes);
            claim.setPrescriptionNotes(result.prescriptionNotes);
        },err=>{
          console.log(err);
        })
  }
  get selectedClaim():Claim{
    return this.claims.get(this.selected)
  }
}
