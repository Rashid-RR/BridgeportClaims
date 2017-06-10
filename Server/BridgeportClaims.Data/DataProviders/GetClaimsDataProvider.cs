using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Data.StoredProcedureExecutors.Dtos;

namespace BridgeportClaims.Data.DataProviders
{
    public class GetClaimsDataProvider : IGetClaimsDataProvider
    {
        private readonly IStoredProcedureExecutor _storedProcedureExecutor;

        public GetClaimsDataProvider(IStoredProcedureExecutor storedProcedureExecutor)
        {
            _storedProcedureExecutor = storedProcedureExecutor;
        }

        public IList<GetClaimsData> GetClaimsData(string claimNumber, string firstName, string lastName,
            string rxNumber, string invoiceNumber)
        {
            var claimNumberParam = new SqlParameter
            {
                ParameterName = "ClaimNumber",
                Value = claimNumber,
                DbType = DbType.String
            };

            var firstNameParam = new SqlParameter
            {
                ParameterName = "FirstName",
                Value = firstName,
                DbType = DbType.String
            };

            var lastNameParam = new SqlParameter
            {
                ParameterName = "LastName",
                Value = lastName,
                DbType = DbType.String
            };

            var rxNumberParam = new SqlParameter
            {
                ParameterName = "RxNumber",
                Value = rxNumber,
                DbType = DbType.String
            };

            var invoiceNumberParam = new SqlParameter
            {
                ParameterName = "InvoiceNumber",
                Value = invoiceNumber,
                DbType = DbType.String
            };

            var retVal = _storedProcedureExecutor.ExecuteMultiResultStoredProcedure<GetClaimsData>
            ("EXECUTE dbo.uspGetClaimsData @ClaimNumber = :ClaimNumber, @FirstName = :FirstName, " +
             "@LastName = :LastName, @RxNumber = :RxNumber, @InvoiceNumber = :InvoiceNumber",
                new List<SqlParameter>
                {
                    claimNumberParam,
                    firstNameParam,
                    lastNameParam,
                    rxNumberParam,
                    invoiceNumberParam
                }).ToList();
            return retVal;
        }
    }
}
