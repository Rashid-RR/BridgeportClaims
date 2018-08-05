// prescription.ts 
export interface Prescription {
    rxnumber:number;
    labelName:String;
    invoicenumber:number;
    invoiceAmount:number;
    invoiceUrl:string;
    invoiceDate:Date ;
    billTo:String;
    amountPaid:number;
    outstanding:number;
    selected:Boolean;
    prescriptionId:number;
    noteCount:number;
    isReversed:Boolean;
    invoiceIsIndexed:boolean;
    status:String;
    prescriber:String;
    prescriberNpi:String;
    pharmacyName:String;
    fileName:String;
    prescriberPhone:any;
    prescriptionNdc:string;
}
