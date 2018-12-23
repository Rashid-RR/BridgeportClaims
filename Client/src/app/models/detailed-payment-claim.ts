export class DetailedPaymentClaim {
  claimId: Number;
  claimNumber: Number;
  patientName: String;
  rxNumber: Number;
  invoiceNumber: any;
  rxDate: Date;
  invoicedAmount: number;
  outstanding: Number;
  labelName: String;
  payor: String;
  selected: Boolean;
  searchSelected: Boolean;
  filterSelected: Boolean;
  isReversed: Boolean;
  prescriptionId: number;
  amountRemaining: number;
  constructor(prescriptionId: number, claimId: Number, claimNumber: Number, patientName: String,
    rxNumber: Number, invoiceNumber: any, rxDate: Date, labelName: String,
    outstanding: Number, invoicedAmount: number, payor: String, isReversed: Boolean, selected: Boolean= false, amountRemaining?: number, searchSelected?: Boolean,
    filterSelected?: Boolean) {
    this.prescriptionId = prescriptionId,
    this.claimId = claimId,
    this.claimNumber = claimNumber,
    this.patientName = patientName;
     this.rxNumber = rxNumber;
    this.invoiceNumber = invoiceNumber;
    this.rxDate = rxDate;
    this.outstanding = outstanding;
    this.labelName = labelName;
    this.invoicedAmount = invoicedAmount;
    this.payor = payor;
    this.amountRemaining = amountRemaining;
    this.searchSelected = searchSelected;
    this.filterSelected = filterSelected;
    this.isReversed =  isReversed;

  }
}
