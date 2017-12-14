export class ClaimImage{
    documentId: number;
	created: Date;
	fileName: string;
	fileUrl: string;
	type: String;
	rxNumber: Number;
	rxDate: Date;
    constructor(created: Date,type: String,rxNumber: Number,rxDate: Date,fileName: string,
        diaryNote:  String,fileUrl:string,documentId?: number){
        this.documentId = documentId; 
        this.created=created 
        this.type = type
        this.fileUrl = fileUrl;
        this.rxNumber = rxNumber
        this.rxDate = rxDate
        this.fileName=fileName 
    }
}