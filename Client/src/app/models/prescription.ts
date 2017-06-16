// prescription.ts 
export class Prescription {
    rxNumber:Number;
    labelName:String;
    invoiceNumber:Number;
    invoiceAmount:Number;
    invoiceDate:Date ;
    billTo:String;
    amountPaid:Number;
    outstanding:Number;
constructor(rxNumber:Number,labelName:String,invoiceNumber:Number,invoiceDate:Date,
    billTo:String,amountPaid:Number,outstanding:Number,invoiceAmount:Number){
    this.rxNumber = rxNumber;
    this.labelName = labelName;
    this.invoiceNumber = invoiceNumber;
    this.invoiceDate = invoiceDate ;
    this.billTo = billTo;
    this.amountPaid = amountPaid;
    this.outstanding= outstanding;
    this.invoiceAmount = invoiceAmount;
  }
}
