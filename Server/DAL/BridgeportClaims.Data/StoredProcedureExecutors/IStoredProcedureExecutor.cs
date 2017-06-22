using System.Collections.Generic;
using System.Data.SqlClient;

namespace BridgeportClaims.Data.StoredProcedureExecutors
{
    public interface IStoredProcedureExecutor
    {
        IEnumerable<T> ExecuteMultiResultStoredProcedure<T>(string procedureNameExecStatement, IList<SqlParameter> parameters);
        T ExecuteSingleResultStoredProcedure<T>(string procedureNameExecStatement, IList<SqlParameter> parameters);
        T ExecuteScalarStoredProcedure<T>(string procedureName, IList<SqlParameter> parameters);
    }
}