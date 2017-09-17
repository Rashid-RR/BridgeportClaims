export class Payment{
  checkAmount:any;
  checkany:any;
  rxDate:Date;
  prescriptionPaymentId:any;
	prescriptionId: any;
	postedDate: Date;
	rxany:any ;
	invoiceany:any;
  constructor(checkAmount:any,checkany:any,rxDate:Date,
    prescriptionPaymentId:any,
    prescriptionId: any,
    postedDate: Date,
    rxany:any ,
    invoiceany:any){
      this.checkany=checkany;
      this.checkAmount=checkAmount;
      this.rxDate=rxDate;
      this.prescriptionPaymentId=prescriptionPaymentId;
      this.prescriptionId =prescriptionId 
      this.postedDate = postedDate;
      this.rxany=  rxany;
      this.invoiceany= invoiceany;
  }
}