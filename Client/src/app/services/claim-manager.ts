
import {UUID} from "angular2-uuid";
import * as Immutable from "immutable";
import {Observable} from "rxjs/Observable";
import {Claim} from "../models/claim" 
import {ClaimNote} from "../models/claim-note" 
import {Injectable} from "@angular/core";
import {HttpService} from "./http-service";
import {EventsService} from "./events-service";
  
@Injectable()
export class ClaimManager{
  private claims: Immutable.OrderedMap<Number, Claim> = Immutable.OrderedMap<Number, Claim>();
  selected:Number;
  loading:Boolean=false;
  private notetypes:Array<any>=[];
  constructor(private http:HttpService,private events:EventsService) {}
  
  search(data){    
    this.loading = true;
    this.http.getClaimsData(data).map(res=>{return res.json()})
    .subscribe((result:any)=>{
        this.loading = false;
        this.selected=undefined;
        if(result.name){
            this.claims = Immutable.OrderedMap<Number, Claim>();
            var c = new Claim(-10,result.claimNumber,result.dateEntered,result.injuryDate,result.gender,
            result.carrier,result.adjustor,result.adjustorPhoneNumber,result.dateEntered,result.adjustorPhoneNumber
            ,result.name,result.firstName,result.lastName);
            this.claims = this.claims.set(-10,c);
            let claim = this.claims.get(-10);
            claim.setPrescription(result.prescriptions); 
            claim.setPayment(result.payments);
            claim.setEpisodes(result.episodes);
            claim.setClaimNotes(result.claimNotes? new ClaimNote(result.claimNotes[0].noteText,result.claimNotes[0].noteType.key) : null);
            claim.setPrescriptionNotes(result.prescriptionNotes);
        }else{
            let res:  Array<Claim>  = result;
            this.claims = Immutable.OrderedMap<Number, Claim>();
            result.forEach(claim=>{
            var c = new Claim(claim.claimId,claim.claimNumber,claim.dateEntered,claim.injuryDate,claim.gender,
            claim.carrier,claim.adjustor,claim.adjustorPhoneNumber,claim.dateEntered,claim.adjustorPhoneNumber
            ,claim.name,claim.firstName,claim.lastName);
            this.claims = this.claims.set(claim.claimId,c);
            })
        }
      },err=>{
        this.loading = false;
        console.log(err);
      })
      this.http.getNotetypes().map(res=>{return res.json()})
        .subscribe((result:Array<any>)=>{
            console.log(result)
            this.notetypes= result;
        },err=>{
            this.loading = false;
            console.log(err);
        })
  }

  get dataSize(){
    return this.claims.size;
  }
  get claimsData():Claim []{
    return this.claims.asImmutable().toArray();
  }
  get NoteTypes():Array<any>{
    return this.notetypes;
  }
  getClaimsDataById(id:Number){
      this.selected = id;
      var claim:Claim = this.claims.get(id) as Claim; 
      if(id !== -10){ 
        this.loading = true;         
        this.http.getClaimsData({claimId:id}).map(res=>{return res.json()})
          .subscribe(result=>{
              this.loading = false;
              claim.setPrescription(result.prescriptions); 
              claim.setPayment(result.payments);
              claim.setEpisodes(result.episodes);
              claim.setClaimNotes(result.claimNotes? new ClaimNote(result.claimNotes[0].noteText,result.claimNotes[0].noteType.key) : null);
              claim.setPrescriptionNotes(result.prescriptionNotes);
          },err=>{
            this.loading = false;
            console.log(err);
          })
      }
  }
  get selectedClaim():Claim {
    return this.claims.get(this.selected)
  }
}
