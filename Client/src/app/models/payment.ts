export class Payment{
  checkAmt:any;
  checkNumber:any;
  rxDate:Date;
  prescriptionPaymentId:any;
	prescriptionId: any;
	postedDate: Date;
	rxNumber:any ;
	invoiceNumber:any;
	isReversed:Boolean;
  constructor(checkAmt:any,checkNumber:any,rxDate:Date,
    prescriptionPaymentId:any,
    prescriptionId: any,
    postedDate: Date,
    rxNumber:any ,isReversed:Boolean,
    invoiceNumber:any){
      this.checkNumber=checkNumber;
      this.checkAmt=checkAmt;
      this.rxDate=rxDate;
      this.prescriptionPaymentId=prescriptionPaymentId;
      this.prescriptionId =prescriptionId 
      this.postedDate = postedDate;
      this.rxNumber=  rxNumber;
      this.invoiceNumber= invoiceNumber;
      this.isReversed = isReversed;
  }
}