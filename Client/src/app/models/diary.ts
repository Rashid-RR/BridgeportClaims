export class Diary {
    diaryId: Number;
	claimId: Number;
	prescriptionNoteId: Number;
	owner: String;
	created: Date;
	followUpDate: Date;
	patientName: String;
	claimNumber: Number;
	type: String;
	rxNumber: Number;
	rxDate: Date;
	insuranceCarrier: String;
	diaryNote:  String;
    constructor(diaryId: Number, claimId: Number, prescriptionNoteId: Number, owner: String, created: Date,
        followUpDate: Date, patientName: String, claimNumber: Number, type: String, rxNumber: Number, rxDate: Date, insuranceCarrier: String,
        diaryNote:  String) {
        this.diaryId = diaryId;
        this.claimId = claimId;
        this.prescriptionNoteId = prescriptionNoteId;
        this.owner = owner;
        this.created = created;
        this.followUpDate = followUpDate;
        this.patientName = patientName;
        this.claimNumber = claimNumber;
        this.type = type;
        this.rxNumber = rxNumber;
        this.rxDate = rxDate;
        this.insuranceCarrier = insuranceCarrier;
        this.diaryNote = diaryNote;
    }
}
