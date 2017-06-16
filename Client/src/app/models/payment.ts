export class Payment{
  checkAmount:Number;
  checkNumber:Number;
  date:Date 
  constructor(checkAmount:Number,checkNumber:Number,date:Date){
      this.checkNumber=checkNumber;
      this.checkAmount=checkAmount;
      this.date=date;
  }
}