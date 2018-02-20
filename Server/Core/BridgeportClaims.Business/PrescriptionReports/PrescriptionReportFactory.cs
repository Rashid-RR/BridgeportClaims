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
            var pdfFullFilePath = _pdfFactory.GeneratePdf(dt);
            return pdfFullFilePath;
        }
    }
}