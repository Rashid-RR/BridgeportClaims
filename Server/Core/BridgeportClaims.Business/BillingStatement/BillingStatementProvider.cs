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
            if (null == billingStatementDto)
            {
                throw new ArgumentNullException(nameof(billingStatementDto));
            }
            var localNow = DateTime.UtcNow.ToMountainTime();
            var firstName = null == billingStatementDto.FirstName
                ? string.Empty
                : (billingStatementDto.FirstName.IsNullOrWhiteSpace()
                    ? string.Empty
                    : billingStatementDto.FirstName.Replace(".", ""));
            var lastName = null == billingStatementDto.LastName
                ? string.Empty
                : (billingStatementDto.LastName.IsNullOrWhiteSpace()
                    ? string.Empty
                    : billingStatementDto.LastName.Replace(".", ""));
            fileName = $"{lastName}_{firstName}_Billing_Statement_{localNow:MM-dd-yy}";
            var fullFilePath = ExcelFactory.GetBillingStatementExcelFilePathFromDataTable(dt,
                StringConstants.BillingStatementName, fileName, billingStatementDto);
            return fullFilePath;
        }
    }
}