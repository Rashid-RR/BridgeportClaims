export interface InvoiceScreen {
  invoiceDate: string; // Date -- MUST be formmatted MM/dd/yyyy
  carrier: string;
  patientName: string;
  claimNumber: string;
  invoiceCount: number;
  scriptCount: number;
}

export interface InvoiceProcess  {
  rxDate: string; // Date -- MUST be formmatted MM/dd/yyyy
  carrier: string;
  patientName: string;
  claimNumber: string;
  inQueue: number;
}
