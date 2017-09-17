export class Payment{
  checkAmt:any;
  checkany:any;
  rxDate:Date;
  prescriptionPaymentId:any;
	prescriptionId: any;
	postedDate: Date;
	rxany:any ;
	invoiceany:any;
  constructor(checkAmt:any,checkany:any,rxDate:Date,
    prescriptionPaymentId:any,
    prescriptionId: any,
    postedDate: Date,
    rxany:any ,
    invoiceany:any){
      this.checkany=checkany;
      this.checkAmt=checkAmt;
      this.rxDate=rxDate;
      this.prescriptionPaymentId=prescriptionPaymentId;
      this.prescriptionId =prescriptionId 
      this.postedDate = postedDate;
      this.rxany=  rxany;
      this.invoiceany= invoiceany;
  }
}