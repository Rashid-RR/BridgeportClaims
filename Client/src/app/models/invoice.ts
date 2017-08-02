export class Invoice{
  claimNumber: Number;
  firstName: String;
  lastName: String;
  rxNumber: Number;
  invoiceNumber: any
  rxDate:Date;
  invoiceAmount:number;
  outstanding:Number;
  labelName:String;
  payor:String;
  selected:Boolean;
  constructor(claimNumber: Number, firstName: String, lastName: String,
    rxNumber: Number, invoiceNumber: any,rxDate:Date,labelName:String,
    outstanding:Number,invoiceAmount:number,payor:String,selected:Boolean=false){
    this.claimNumber = claimNumber,
    this.firstName = firstName;
    this.lastName = lastName;
    this.rxNumber = rxNumber;
    this.invoiceNumber = invoiceNumber;
    this.rxDate=rxDate;
    this.outstanding= outstanding;
    this.labelName = labelName;
    this.invoiceAmount = invoiceAmount;
    this.payor = payor;
    
  }
}