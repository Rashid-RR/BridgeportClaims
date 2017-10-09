export class PrescriptionNotes{
   
    enteredBy:String;
    type:String;
    note:String;
    noteUpdatedOn:Date;
    rxDate:Date;
    rxNumber:Number;
      constructor(noteUpdatedOn:Date,enteredBy:String,type:String,note:String,rxDate:Date,rxNumber:Number){
          this.noteUpdatedOn = noteUpdatedOn;
          this.type = type;
          this.enteredBy = enteredBy;
          this.note = note;
          this.rxDate = rxDate;
          this.rxNumber = rxNumber;
      }
}