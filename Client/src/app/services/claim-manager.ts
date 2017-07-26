
import {UUID} from "angular2-uuid";
import * as Immutable from "immutable";
import {Observable} from "rxjs/Observable";
import {Claim} from "../models/claim" 
import {Prescription} from "../models/prescription" 
import {ClaimNote} from "../models/claim-note" 
import {PrescriptionNoteType} from "../models/prescription-note-type" 
import {Injectable} from "@angular/core";
import {HttpService} from "./http-service";
import {EventsService} from "./events-service";
import {Router} from "@angular/router";

@Injectable()
export class ClaimManager{
  private claims: Immutable.OrderedMap<Number, Claim> = Immutable.OrderedMap<Number, Claim>();
  selected:Number;
  loading:Boolean=false;
  private notetypes:Array<any>=[];
  private prescriptionNotetypes:Array<PrescriptionNoteType>=[];
  constructor(private http:HttpService,private events:EventsService,private router: Router) {}
  
  search(data){    
    this.loading = true;
    this.http.getClaimsData(data).map(res=>{return res.json()})
    .subscribe((result:any)=>{
        this.loading = false;
        this.selected=undefined;
        if(result.name){
            this.claims = Immutable.OrderedMap<Number, Claim>();
            var c = new Claim(-10,result.claimNumber,result.date,result.injuryDate,result.gender,
            result.carrier,result.adjustor,result.adjustorPhoneNumber,result.dateEntered,result.adjustorPhoneNumber
            ,result.name,result.firstName,result.lastName);
            this.claims = this.claims.set(-10,c);
            let claim = this.claims.get(-10);
            claim.setPrescription(result.prescriptions  as Array<Prescription>); 
            claim.setPayment(result.payments);
            claim.setEpisodes(result.episodes);
            claim.setClaimNotes(result.claimNotes && result.claimNotes[0] ? new ClaimNote(result.claimNotes[0].noteText,result.claimNotes[0].noteType) : null);
            claim.setPrescriptionNotes(result.prescriptionNotes);
            this.selected = -10;
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
        try{
          let error = err.json();
          console.log(error);          
        }catch(e){}
      },()=>{
          this.events.broadcast("claim-updated")
      })
      this.http.getNotetypes().map(res=>{return res.json()})
        .subscribe((result:Array<any>)=>{
           // console.log("Claim Notes",result)
            this.notetypes= result;
        },err=>{
            this.loading = false;
            let error = err.json();              
        })
      this.http.getPrescriptionNotetypes().map(res=>{return res.json()})
        .subscribe((result:Array<any>)=>{
            //console.log("Prescription Notes",result)
            this.prescriptionNotetypes= result;
        },err=>{
            this.loading = false;
            console.log(err);
            let error = err.json();           
        })
  }

  get dataSize(){
    return this.claims.size;
  }
  get claimsData():Claim []{
    return this.claims.asImmutable().toArray();
  }
  clearClaimsData(){
    this.claims = Immutable.OrderedMap<Number, Claim>();
  }
  get NoteTypes():Array<any>{
    return this.notetypes;
  }
  get PrescriptionNoteTypes():Array<any>{
    return this.prescriptionNotetypes;
  }
  getClaimsDataById(id:Number){
      this.selected = id;
      var claim:Claim = this.claims.get(id) as Claim; 
      if(id !== -10){ 
        this.loading = true;         
        this.http.getClaimsData({claimId:id}).map(res=>{return res.json()})
          .subscribe(result=>{
              this.loading = false;
              claim.setPrescription(result.prescriptions as Array<Prescription>); 
              claim.setPayment(result.payments);
              claim.setEpisodes(result.episodes);
              claim.setClaimNotes(result.claimNotes && result.claimNotes[0] ? new ClaimNote(result.claimNotes[0].noteText,result.claimNotes[0].noteType) : null);
              claim.setPrescriptionNotes(result.prescriptionNotes);
          },err=>{
            this.loading = false;
            console.log(err);
            let error = err.json();
          },()=>{
              this.events.broadcast("claim-updated")
          })
      }
  }
  get selectedClaim():Claim {
    return this.claims.get(this.selected)
  }

  
}
