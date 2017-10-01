export class PaymentPostingPrescription {
    patientName: String;
    rxDate: Date;
    amountPosted: Number;
    prescriptionId: Number
    id: Number;
    constructor(id:Number,patientName:String,rxDate: Date,amountPosted: Number, prescriptionId: Number) {
        this.patientName= patientName;
        this.rxDate=rxDate;
        this.amountPosted=amountPosted;
        this.prescriptionId =prescriptionId;
        this.id =id;
    }
}
