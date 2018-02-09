export class ClaimImage{
    constructor(public created: Date,public type: String,public rxNumber: Number,public rxDate: Date,public fileName: string,
        public diaryNote:  String,public fileUrl: string,public documentId?: number,public injuryDate?:Date,public attorneyName?:string,public invoiceNumber?:number){
        this.documentId = documentId; 
        this.created=created 
        this.type = type
        this.fileUrl = fileUrl;
        this.rxNumber = rxNumber
        this.rxDate = rxDate
        this.fileName=fileName 
    }
}