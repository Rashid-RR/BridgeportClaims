// unpaid-script.ts

 
export class UnpaidScript {
    claimId:Number;
    scriptId:Number;
    owner:String;
    patientName:String;
    insuranceCarrier:String;
    labelName:String;
    claimNumber:Number;
    adjustor:String;
    adjustorPhoneNumber:String;
    created:Date;
    invoiceDate:Date;
    invoiceNumber:String;
    rxDate:Date;
    rxNumber:String;
    invoiceAmount:String;  
constructor(claimId:Number,scriptId:Number,claimNumber:Number,invoiceAmount:Number,invoiceNumber:String,created:Date,owner:String,
    insuranceCarrier:String,labelName:String,adjustor:String,adjustorPhoneNumber:String,invoiceDate:Date,rxDate:Date,rxNumber:String,patientName:String){
    this.scriptId = scriptId;
    this.claimId = claimId;
    this.insuranceCarrier = insuranceCarrier;
    this.labelName = labelName;
    this.patientName = patientName;
    this.claimNumber = claimNumber;
    this.adjustor= adjustor;
    this.adjustorPhoneNumber = adjustorPhoneNumber;
    this.created = created;
    this.invoiceDate = invoiceDate;
    this.rxDate = rxDate;
    this.owner = owner;
    this.rxNumber = rxNumber;
    this.invoiceNumber = invoiceNumber;
  }

}
