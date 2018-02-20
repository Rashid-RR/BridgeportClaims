using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Pdf.ITextPdfFactory;

namespace BridgeportClaims.Business.PrescriptionReports
{
    public class PrescriptionReportFactory : IPrescriptionReportFactory
    {
        private readonly IClaimsDataProvider _claimsDataProvider;
        private readonly IPdfFactory _pdfFactory;

        public PrescriptionReportFactory(IClaimsDataProvider claimsDataProvider, IPdfFactory pdfFactory)
        {
            _claimsDataProvider = claimsDataProvider;
            _pdfFactory = pdfFactory;
        }

        public string GeneratePrescriptionReport(int claimId)
        {
            var data = _claimsDataProvider.GetPrescriptionDataByClaim(claimId, "RxDate", "DESC", 1, 5000);
            var dt = data?.ToDataTable();
            if (null == dt)
                return null;
            dt.Columns.Remove("PrescriptionId");
            dt.Columns.Remove("Status");
            dt.Columns.Remove("NoteCount");
            dt.Columns.Remove("IsReversed");
            dt.Columns.Remove("Prescriber");
            dt.Columns.Remove("PrescriberNpi");
            dt.Columns.Remove("PharmacyName");
            dt.Columns.Remove("PrescriptionNdc");
            dt.Columns.Remove("PrescriberPhone");
            dt.SetColumnsOrder("InvoiceDate", "InvoiceNumber", "LabelName", "BillTo", "RxNumber", "RxDate",
                "InvoiceAmount", "AmountPaid", "Outstanding");
            var pdfFullFilePath = _pdfFactory.GeneratePdf(dt);
            return pdfFullFilePath;
        }
    }
}