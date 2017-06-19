export class PrescriptionNotes{
   
    enteredBy:String;
    type:String;
    note:String;
    date:Date;
      constructor(date:Date,enteredBy:String,type:String,note:String){
          this.date = date;
          this.type = type;
          this.enteredBy = enteredBy;
          this.note = note;
      }
}