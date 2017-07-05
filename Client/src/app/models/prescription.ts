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
    selected:Boolean;
constructor(rxNumber:Number,labelName:String,invoiceNumber:Number,invoiceDate:Date,
    billTo:String,amountPaid:Number,outstanding:Number,invoiceAmount:Number,selected:Boolean=false){
    this.rxNumber = rxNumber;
    this.labelName = labelName;
    this.invoiceNumber = invoiceNumber;
    this.invoiceDate = invoiceDate ;
    this.billTo = billTo;
    this.amountPaid = amountPaid;
    this.outstanding= outstanding;
    this.invoiceAmount = invoiceAmount;
    this.selected=selected;
  }

  setSelected(s:Boolean){
      console.log("Works...");
      this.selected = s==undefined? true : s;
  }
}
