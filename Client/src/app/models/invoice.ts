export class Invoice{
  claimId: Number;
  claimNumber: Number;
  patientName: String;
  rxNumber: Number;
  invoiceNumber: any
  rxDate:Date;
  invoiceAmount:number;
  outstanding:Number;
  labelName:String;
  payor:String;
  selected:Boolean;
  constructor(claimId: Number,claimNumber: Number, patientName: String,
    rxNumber: Number, invoiceNumber: any,rxDate:Date,labelName:String,
    outstanding:Number,invoiceAmount:number,payor:String,selected:Boolean=false){
    this.claimId = claimId,
    this.claimNumber = claimNumber,
    this.patientName = patientName;
     this.rxNumber = rxNumber;
    this.invoiceNumber = invoiceNumber;
    this.rxDate=rxDate;
    this.outstanding= outstanding;
    this.labelName = labelName;
    this.invoiceAmount = invoiceAmount;
    this.payor = payor;
    
  }
}