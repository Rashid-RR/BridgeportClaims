// profile.ts
import {UUID} from "angular2-uuid";

 
export class Payor{
    payorId: number;
    billToName: String;
    billToAddress1: String;
    billToAddress2: String;
    billToCity: String;
    state: String;
    billToPostalCode: number;
    phonenumber: String;
    alternatePhonenumber: String;
    faxnumber: number;
    notes: String;
    contact: any;
    createdOn: Date;
    updatedOn: Date;
  constructor(payorId: number,billToName: String,billToAddress1: String,billToAddress2: String,billToCity: String,state: String,billToPostalCode: number,phonenumber: String,alternatePhonenumber: String,faxnumber: number,notes: String,contact: any,createdOn: Date,updatedOn: Date){
    this.payorId = payorId;
    this.billToName =billToName;
    this.billToAddress1 = billToAddress1;
    this.billToAddress2 = billToAddress2;
    this.billToCity =billToCity;
    this.state =state;
    this.billToPostalCode =billToPostalCode;
    this.phonenumber = phonenumber;
    this.alternatePhonenumber = alternatePhonenumber;
    this.faxnumber = faxnumber;
    this.notes = notes;
    this.contact=contact;
    this.createdOn =createdOn;
    this.updatedOn =updatedOn;
  }
}