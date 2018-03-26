using System.Data;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.DataProviders.Claims;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Business.PrescriptionReports
{
    public class PrescriptionReportFactory : IPrescriptionReportFactory
    {
        private readonly IClaimsDataProvider _claimsDataProvider;
        private const string InvoiceDate = "InvoiceDate";
        private const string InvoiceNumber = "InvoiceNumber";
        private const string LabelName = "LabelName";
        private const string BillTo = "BillTo";
        private const string RxNumber = "RxNumber";
        private const string RxDate = "RxDate";
        private const string InvoiceAmount = "InvoiceAmount";
        private const string AmountPaid = "AmountPaid";

        public PrescriptionReportFactory(IClaimsDataProvider claimsDataProvider)
        {
            _claimsDataProvider = claimsDataProvider;
        }

        public BillingStatementDto GetBillingStatementDto(int claimId) =>
            _claimsDataProvider.GetBillingStatementDto(claimId);

        public DataTable GenerateBillingStatementDataTable(int claimId)
        {
            var data = _claimsDataProvider.GetPrescriptionDataByClaim(claimId, "RxDate", "DESC", 1, 5000);
            var dt = data?.ToDataTable();
            if (null == dt)
                return null;
            FormatPrescriptionReportDataTable(dt);
            return dt;
        }

        private static void FormatPrescriptionReportDataTable(DataTable dt)
        {
            dt.Columns.Remove("PrescriptionId");
            dt.Columns.Remove("Status");
            dt.Columns.Remove("NoteCount");
            dt.Columns.Remove("IsReversed");
            dt.Columns.Remove("Prescriber");
            dt.Columns.Remove("PrescriberNpi");
            dt.Columns.Remove("PharmacyName");
            dt.Columns.Remove("PrescriptionNdc");
            dt.Columns.Remove("PrescriberPhone");
            dt.Columns.Remove("InvoiceIsIndexed");
            dt.Columns.Remove("InvoiceUrl");
            dt.SetColumnsOrder(InvoiceDate, InvoiceNumber, LabelName, BillTo, RxNumber, RxDate, InvoiceAmount, AmountPaid, "Outstanding");
            dt.Columns[InvoiceDate].ColumnName = "Inv Date";
            dt.Columns[InvoiceNumber].ColumnName = "Inv #";
            dt.Columns[LabelName].ColumnName = "Label Name";
            dt.Columns[BillTo].ColumnName = "Bill To";
            dt.Columns[RxNumber].ColumnName = "Rx #";
            dt.Columns[RxDate].ColumnName = "Rx Date";
            dt.Columns[InvoiceAmount].ColumnName = "Inv Amt";
            dt.Columns[AmountPaid].ColumnName = "Amt Paid";
        }
    }
}