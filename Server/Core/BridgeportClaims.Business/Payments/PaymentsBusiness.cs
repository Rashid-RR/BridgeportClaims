using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BridgeportClaims.Excel.Adapters;
using BridgeportClaims.Common.DataTables;
using BridgeportClaims.Data.DataProviders.Payments;

namespace BridgeportClaims.Business.Payments
{
    public class PaymentsBusiness : IPaymentsBusiness
    {
        private readonly Lazy<IPaymentsDataProvider> _paymentsDataProvider;

        public PaymentsBusiness(Lazy<IPaymentsDataProvider> paymentsDataProvider)
        {
            _paymentsDataProvider = paymentsDataProvider;
        }

        public bool CheckMultiLinePartialPayments(decimal amountSelected, decimal amountToPost,
            int countOfPrescriptions) => amountSelected == amountToPost || countOfPrescriptions <= 1;

        private static DataTable GetDataTableFromExcelBytes(IEnumerable<byte> fileBytes) =>
            ExcelDataReaderAdapter.ReadExcelFileIntoDataTable(fileBytes.ToArray());

        public void ImportPaymentFile(string fileName)
        {
            var fileBytes = _paymentsDataProvider.Value.GetBytesFromDb(fileName);
            if (null == fileBytes)
                throw new ArgumentNullException($"Error. The File \"{fileName}\" does not Exist in the Database");
            var dt = GetDataTableFromExcelBytes(fileBytes.ToArray());
            if (null == dt)
                throw new Exception("Error. Could not Populate the Data Table from the Excel File Bytes " +
                                    "Returned from the Database");
            var newDt = DataTableProvider.FormatDataTableForPaymentImport(dt, true);
            if (null == newDt)
                throw new Exception("Error. Could not Copy over Contents of the Original Data Table into the " +
                                    "Cloned Data Table with String Column Types");
            _paymentsDataProvider.Value.ImportDataTableIntoDb(newDt);
        }
    }
}