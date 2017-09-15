export class PaymentPostingPrescription {
    patientName: String;
    rxDate: Date;
    amountPosted: Number;
    prescriptionId: Number
    constructor(patientName:String,rxDate: Date,amountPosted: Number, prescriptionId: Number) {
        this.patientName= patientName;
        this.rxDate=rxDate;
        this.amountPosted=amountPosted;
        this.prescriptionId =prescriptionId;
    }
}
