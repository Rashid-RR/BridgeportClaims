using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Data.NHibernateProviders;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Data.StoredProcedureExecutors.Dtos;

namespace BridgeportClaims.Data.DataProviders
{
    public class SalesByUsStateProvider : ISalesByUsStateProvider
    {
        public IList<SalesByProductCategoryDbDto> GetSalesByCustomerDbDtos()
        {
            IStoredProcedureExecutor spExecutor = new StoredProcedureExecutor(FluentSessionProvider.SessionFactory);
            var retVal = spExecutor.ExecuteMultiResultStoredProcedure<SalesByProductCategoryDbDto>
                ("EXECUTE dbo.uspGetSalesByUsState", new List<SqlParameter>()).ToList();
            return retVal;
        }
    }
}
