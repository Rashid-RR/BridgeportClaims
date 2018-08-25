// unpaid-script.ts
 
export class UnpaidScript {
    claimId:Number;
    prescriptionId:Number;
    owner:String;
    patientName:String;
    insuranceCarrier:String;
    labelName:String;
    claimNumber:Number;
    adjustorName:String;
    adjustorPhone:String;
    created:Date;
    invoiceDate:Date;
    invoiceNumber:String;
    rxDate:Date;
    rxNumber:String;
    invAmt:String;  
    pharmacyState:String;  
constructor(claimId:Number,prescriptionId:Number,claimNumber:Number,invAmt:Number,invoiceNumber:String,created:Date,owner:String,
    insuranceCarrier:String,labelName:String,adjustorName:String,adjustorPhone:String,invoiceDate:Date,rxDate:Date,rxNumber:String,patientName:String,pharmacyState:String){
    this.prescriptionId = prescriptionId;
    this.claimId = claimId;
    this.insuranceCarrier = insuranceCarrier;
    this.labelName = labelName;
    this.patientName = patientName;
    this.claimNumber = claimNumber;
    this.adjustorName= adjustorName;
    this.adjustorPhone = adjustorPhone;
    this.created = created;
    this.invoiceDate = invoiceDate;
    this.rxDate = rxDate;
    this.owner = owner;
    this.rxNumber = rxNumber;
    this.invoiceNumber = invoiceNumber;
    this.pharmacyState = pharmacyState;
  }

}
