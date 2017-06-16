// profile.ts
import {Prescription} from "./prescription";
import {Payment} from "./payment";
import {PrescriptionNotes} from "./prescription-notes";
import {Episode} from "./episode";

 
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
    adjustorFaxNumber:String;
    private prescription:Array<Prescription> = [];
    private prescriptionNote:Array<PrescriptionNotes> = [];
    private payment:Array<Payment> = [];
    private episode:Array<Episode> = [];
constructor(claimId:Number,claimNumber:Number,dateOfBirth:Date,injuryDate:Date,
    gender:String,carrier:String,adjustor:String,adjustorPhoneNumber:String,dateEntered:Date,adjustorFaxNumber:String,name?:String,firstName?:String,lastName?:String){
    this.claimId = claimId;
    this.firstName = firstName;
    this.lastName = lastName;
    this.name = name;
    this.claimNumber = claimNumber;
    this.dateOfBirth = dateOfBirth ;
    this.injuryDate = injuryDate ;
    this.gender = gender;
    this.carrier = carrier;
    this.adjustor= adjustor;
    this.adjustorPhoneNumber = adjustorPhoneNumber;
    this.dateEntered = dateEntered;
    this.adjustorFaxNumber = adjustorFaxNumber;
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
  setEpisodes(episodes:Array<Payment>){
      if(episodes){
        this.episode = episodes;
      }
  }
  get episodes():Array<Episode>{
      return this.episode;
  }
  setPrescriptionNotes(prescriptionNotes:Array<Payment>){
      if(prescriptionNotes){
        this.prescriptionNote = prescriptionNotes
      }
  }
  get prescriptionNotes():Array<PrescriptionNotes>{
      return this.prescriptionNote;
  }
}
