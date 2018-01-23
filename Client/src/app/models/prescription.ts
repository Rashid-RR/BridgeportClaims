// prescription.ts 
export class Prescription {
    rxnumber:number;
    labelName:String;
    invoicenumber:number;
    invoiceAmount:number;
    invoiceDate:Date ;
    billTo:String;
    amountPaid:number;
    outstanding:number;
    selected:Boolean;
    prescriptionId:number;
    noteCount:number;
    isReversed:Boolean;
    status:String;
    prescriber:String;
    prescriberNpi:String;
    pharmacyName:String;
    prescriberPhone:any;
    prescriptionNdc:string;
constructor(rxnumber:number,labelName:String,invoicenumber:number,invoiceDate:Date,
    billTo:String,amountPaid:number,outstanding:number,invoiceAmount:number,prescriptionId:number,noteCount:number,isReversed?:Boolean,status?:String,selected:Boolean=false,
    prescriber?:String,prescriberNpi?:String,pharmacyName?:String,prescriberPhone?:any,prescriptionNdc?:string){
    this.rxnumber = rxnumber;
    this.labelName = labelName;
    this.invoicenumber = invoicenumber;
    this.invoiceDate = invoiceDate ;
    this.billTo = billTo;
    this.amountPaid = amountPaid;
    this.outstanding= outstanding;
    this.invoiceAmount = invoiceAmount;
    this.selected=selected;
    this.prescriptionId = prescriptionId;
    this.noteCount = noteCount;
    this.isReversed=isReversed;
    this.status=status;
    this.prescriber = prescriber;
    this.prescriberNpi = prescriberNpi;
    this.pharmacyName = pharmacyName;
    this.prescriptionNdc = prescriptionNdc;
    this.prescriberPhone = prescriberPhone;
  }
  setSelected(s:Boolean){
      this.selected = s==undefined? true : s;
  }
}
