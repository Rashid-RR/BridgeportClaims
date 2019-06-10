export interface InvoiceScreen {
  invoiceDate: string; // Date -- MUST be formmatted MM/dd/yyyy
  carrier: string;
  patientname: string;
  claimnumber: string;
  invoicecount: number;
  scriptcount: number;
  printed: number;
  totaltoprint: number;
}
