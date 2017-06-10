// profile.ts
import {UUID} from "angular2-uuid";
 
export class Claim {
    claimId:Number;
    name:String;
    claimNumber:Number;
    dateOfBirth:Date ;
    injuryDate:Date ;
    gender:String;
    carrier:String;
    adjustor:String;
    adjustorPhoneNumber:String;
    dateEntered:Date;
    adjustorFaxNumber:String;
constructor(claimId:Number,name:String,claimNumber:Number,dateOfBirth:Date,injuryDate:Date,
    gender:String,carrier:String,adjustor:String,adjustorPhoneNumber:String,dateEntered:Date,adjustorFaxNumber:String){
    this.claimId = claimId;
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
}
