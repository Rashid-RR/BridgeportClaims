export class ClaimImage{
    imageId: Number;
	created: Date;
	file: String;
	type: String;
	rxNumber: Number;
	rxDate: Date;
    constructor(created: Date,type: String,rxNumber: Number,rxDate: Date,file: String,
        diaryNote:  String,imageId?: Number){
        this.imageId = imageId; 
        this.created=created 
        this.type = type
        this.rxNumber = rxNumber
        this.rxDate = rxDate
        this.file=file 
    }
}