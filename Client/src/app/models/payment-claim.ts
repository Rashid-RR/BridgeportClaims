export class PaymentClaim {
  claimId: Number;
	claimNumber: any;
	patientName: String;
	payor: String;
	numberOfPrescriptions: Number;
  selected: Boolean;
  constructor(claimId: Number, claimNumber: any, patientName: String,
	payor: String, numberOfPrescriptions: Number, selected: Boolean= false) {
      this.claimId = claimId;
      this.claimNumber = claimNumber;
      this.patientName = patientName;
      this.payor =  payor;
      this.numberOfPrescriptions = numberOfPrescriptions;
  }
}
