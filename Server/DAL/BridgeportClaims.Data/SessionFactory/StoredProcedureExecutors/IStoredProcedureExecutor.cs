using System.Collections.Generic;
using System.Data.SqlClient;

namespace BridgeportClaims.Data.SessionFactory.StoredProcedureExecutors
{
    public interface IStoredProcedureExecutor
    {
        IEnumerable<T> ExecuteMultiResultStoredProcedure<T>(string procedureNameExecStatement, IList<SqlParameter> parameters);
    }
}