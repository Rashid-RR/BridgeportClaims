export class Payment{
  checkAmt:any;
  checkany:any;
  rxDate:Date;
  prescriptionPaymentId:any;
	prescriptionId: any;
	postedDate: Date;
	rxNumber:any ;
	invoiceAmount:any;
  constructor(checkAmt:any,checkany:any,rxDate:Date,
    prescriptionPaymentId:any,
    prescriptionId: any,
    postedDate: Date,
    rxNumber:any ,
    invoiceAmount:any){
      this.checkany=checkany;
      this.checkAmt=checkAmt;
      this.rxDate=rxDate;
      this.prescriptionPaymentId=prescriptionPaymentId;
      this.prescriptionId =prescriptionId 
      this.postedDate = postedDate;
      this.rxNumber=  rxNumber;
      this.invoiceAmount= invoiceAmount;
  }
}