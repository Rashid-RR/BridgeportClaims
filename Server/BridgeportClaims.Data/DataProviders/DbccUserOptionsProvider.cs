using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Data.NHibernateProviders;
using BridgeportClaims.Data.StoredProcedureExecutors;
using BridgeportClaims.Data.StoredProcedureExecutors.Dtos;

namespace BridgeportClaims.Data.DataProviders
{
    public class DbccUserOptionsProvider : IDbccUserOptionsProvider
    {
        public IList<DbccUserOptionsResults> GetDbccUserOptions()
        {
            IStoredProcedureExecutor spExecutor = new StoredProcedureExecutor(FluentSessionProvider.SessionFactory);
            var retVal = spExecutor.ExecuteMultiResultStoredProcedure<DbccUserOptionsResults>
                ("EXECUTE dbo.uspDbccUserOptions", new List<SqlParameter>()).ToList();
            return retVal;
        }
    }
}
