using System;
using BridgeportClaims.Business.PrescriptionReports;
using BridgeportClaims.Common.Constants;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Excel.Factories;

namespace BridgeportClaims.Business.BillingStatement
{
    public class BillingStatementProvider : IBillingStatementProvider
    {
        private readonly Lazy<IPrescriptionReportFactory> _prescriptionReportFactory;

        public BillingStatementProvider(Lazy<IPrescriptionReportFactory> prescriptionReportFactory)
        {
            _prescriptionReportFactory = prescriptionReportFactory;
        }

        public string GenerateBillingStatementFullFilePath(int claimId, out string fileName)
        {
            var dt = _prescriptionReportFactory.Value.GenerateBillingStatementDataTable(claimId);
            var billingStatementDto = _prescriptionReportFactory.Value.GetBillingStatementDto(claimId);
            var localNow = DateTime.UtcNow.ToMountainTime();
            fileName = $"{billingStatementDto.LastName}_{billingStatementDto.FirstName}_Billing_Statement_{localNow:MM-dd-yy}";
            var fullFilePath = ExcelFactory.GetBillingStatementExcelFilePathFromDataTable(dt, StringConstants.BillingStatementName, fileName, billingStatementDto);
            return fullFilePath;
        }
    }
}