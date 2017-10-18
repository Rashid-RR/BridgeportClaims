export class PrescriptionNote{
   
    enteredBy:String;
    type:String;
    note:String;
    noteUpdatedOn:Date;
    rxDate:Date;
    rxNumber:Number;
    prescriptionNoteId:Number;
    hasDiaryEntry:boolean;
      constructor(prescriptionNoteId:Number,noteUpdatedOn:Date,enteredBy:String,type:String,note:String,rxDate:Date,rxNumber:Number,hasDiaryEntry:boolean){
          this.prescriptionNoteId = prescriptionNoteId;
          this.noteUpdatedOn = noteUpdatedOn;
          this.type = type;
          this.enteredBy = enteredBy;
          this.note = note;
          this.rxDate = rxDate;
          this.rxNumber = rxNumber;
          this.hasDiaryEntry = hasDiaryEntry;
      }
}