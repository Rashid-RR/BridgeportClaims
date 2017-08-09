export class DetailedPaymentClaim{
  claimId: Number;
  claimNumber: Number;
  patientName: String;
  rxNumber: Number;
  invoiceNumber: any
  rxDate:Date;
  invoicedAmount:number;
  outstanding:Number;
  labelName:String;
  payor:String;
  selected:Boolean;
  constructor(claimId: Number,claimNumber: Number, patientName: String,
    rxNumber: Number, invoiceNumber: any,rxDate:Date,labelName:String,
    outstanding:Number,invoicedAmount:number,payor:String,selected:Boolean=false){
    this.claimId = claimId,
    this.claimNumber = claimNumber,
    this.patientName = patientName;
     this.rxNumber = rxNumber;
    this.invoiceNumber = invoiceNumber;
    this.rxDate=rxDate;
    this.outstanding= outstanding;
    this.labelName = labelName;
    this.invoicedAmount = invoicedAmount;
    this.payor = payor;
    
  }
}