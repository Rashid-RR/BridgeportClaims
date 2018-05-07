export interface PrescriptionNote{
   
    enteredBy: string;
    type: string;
    note: string;
    noteUpdatedOn:Date;
    rxDate:Date;
    rxNumber:number;
    diaryId:number;
    prescriptionNoteId:number;
    hasDiaryEntry:boolean;
    scripts:Array<{rxDate:Date,rxNumber: number}>;
}