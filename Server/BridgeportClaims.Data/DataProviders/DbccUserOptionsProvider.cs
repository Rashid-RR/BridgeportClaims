using System;
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
        private const string ReadCommittedSnapshot = "READ_COMMITTED_SNAPSHOT";

        public IList<DbccUserOptionsResults> GetDbccUserOptions()
        {
            IStoredProcedureExecutor spExecutor = new StoredProcedureExecutor(FluentSessionProvider.SessionFactory);
            var retVal = spExecutor.ExecuteMultiResultStoredProcedure<DbccUserOptionsResults>
                ("EXECUTE dbo.uspDbccUserOptions", new List<SqlParameter>()).ToList();
            return retVal;
        }

        public bool IsSessionUsingReadCommittedSnapshotIsolation()
        {
            var options = GetDbccUserOptions();
            var isolationLevel = options.FirstOrDefault(x => x.SetOption == "isolation level")?.Value;
            if (string.IsNullOrWhiteSpace(isolationLevel))
                throw new Exception("Error, could not find the \"isolation level\" user option.");
            isolationLevel = isolationLevel.Replace(" ", "_").ToUpper();
            return isolationLevel == ReadCommittedSnapshot;
        }
    }
}
