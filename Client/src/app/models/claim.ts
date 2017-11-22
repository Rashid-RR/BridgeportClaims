// profile.ts
import {Prescription} from "./prescription";
import {Payment} from "./payment";
import {ClaimImage} from "./claim-image";
import {PrescriptionNote} from "./prescription-note";
import {ClaimNote} from "./claim-note";
import {Episode} from "./episode";
import {ClaimFlex2} from "./claim-flex2";
import {PrescriptionStatuses} from "./prescription-statuses";
import * as Immutable from 'immutable';

 
export class Claim {
    claimId:Number;
    name:String;
    firstName:String;
    lastName:String;
    claimNumber:Number;
    dateOfBirth:Date ;
    injuryDate:Date ;
    gender:String;
    carrier:String;
    adjustor:String;
    adjustorPhoneNumber:String;
    dateEntered:Date;
    eligibilityTermDate:Date;
    adjustorFaxNumber:String;
    flex2:String;
    private prescription:Array<Prescription> = [];
    //private prescriptionNote:Array<PrescriptionNote> = [];
    private prescriptionNote:Immutable.OrderedMap<Number, PrescriptionNote> = Immutable.OrderedMap<Number, PrescriptionNote>();
    private payment:Array<Payment> = [];
    private image:Array<ClaimImage> = [];
    private episode:Array<Episode> = [];
    private claimFlex2s:Array<ClaimFlex2> = [];
    private prescriptionStatus:Array<PrescriptionStatuses> = [];
    claimNote:ClaimNote;
    editing:Boolean=false;
constructor(claimId:Number,claimNumber:Number,dateOfBirth:Date,injuryDate:Date,
    gender:String,carrier:String,adjustor:String,adjustorPhoneNumber:String,dateEntered:Date,adjustorFaxNumber:String,name?:String,firstName?:String,lastName?:String,flex2?:String,eligibilityTermDate?:Date){
    this.claimId = claimId;
    this.firstName = firstName;
    this.lastName = lastName;
    this.name = name;
    this.claimNumber = claimNumber;
    this.dateOfBirth = dateOfBirth ;
    this.injuryDate = injuryDate ;
    this.gender = gender;
    this.flex2 = flex2;
    this.carrier = carrier;
    this.adjustor= adjustor;
    this.adjustorPhoneNumber = adjustorPhoneNumber;
    this.dateEntered = dateEntered;
    this.adjustorFaxNumber = adjustorFaxNumber;
    this.eligibilityTermDate = eligibilityTermDate;
  }

  setPrescription(prescription:Array<Prescription>){
      if(prescription){
        this.prescription = prescription
      }
  }
  get prescriptions():Array<Prescription>{
      return this.prescription
  }
  setPayment(payments:Array<Payment>){
      if(payments){
        this.payment = payments
      }
  }
  get payments():Array<Payment>{
      return this.payment
  }
  setImages(images:Array<ClaimImage>){
    if(images){
      this.image = images
    }
    }
    get images():Array<ClaimImage>{
        return this.image
    }
    setEpisodes(episodes:Array<Episode>){
        if(episodes){
            this.episode = episodes;
        }
    }
    get episodes():Array<Episode>{
        return this.episode;
    }
    setFlex2(claimFlex2s:Array<ClaimFlex2>){
        if(claimFlex2s){
            this.claimFlex2s = claimFlex2s;
        }
    }
    get getFlex2():Array<ClaimFlex2>{
        return this.claimFlex2s;
    }
    setPrescriptionNotes(prescriptionNotes:Array<PrescriptionNote>){
        if(prescriptionNotes){
            prescriptionNotes.forEach(note=>{ 
                let n = this.prescriptionNote.get(note.prescriptionNoteId);
                if(note.scripts && note.scripts.length>0){
                    note.rxDate = note.scripts[0].rxDate;
                    note.rxNumber = note.scripts[0].rxNumber;
                }
                if(!n || n.rxDate.getTime()<note.rxDate.getTime()){
                    this.prescriptionNote = this.prescriptionNote.set(note.prescriptionNoteId, note); 
                }             
            })
        }
    }
    get prescriptionStatuses():Array<PrescriptionStatuses>{
        return this.prescriptionStatus;
    }
    setPrescriptionStatuses(prescriptionStatuses:Array<PrescriptionStatuses>){
        if(prescriptionStatuses){
            this.prescriptionStatus = prescriptionStatuses
        }
    }
    get prescriptionNotes():Array<PrescriptionNote>{
        return this.prescriptionNote.toArray();
    }
    setClaimNotes(claimNote:ClaimNote){
        if(claimNote){
            this.claimNote = claimNote;
        }
    }
    get claimNotes():ClaimNote{
        return this.claimNote;
    }
}
