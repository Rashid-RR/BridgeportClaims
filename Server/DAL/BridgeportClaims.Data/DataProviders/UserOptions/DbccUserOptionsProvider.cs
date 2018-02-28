using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.SessionFactory.StoredProcedureExecutors;

namespace BridgeportClaims.Data.DataProviders.UserOptions
{
    public class DbccUserOptionsProvider : IDbccUserOptionsProvider
    {
        private const string ReadCommittedSnapshot = "READ_COMMITTED_SNAPSHOT";
        private readonly IStoredProcedureExecutor _storedProcedureExecutor;

        public DbccUserOptionsProvider(IStoredProcedureExecutor storedProcedureExecutor)
        {
            _storedProcedureExecutor = storedProcedureExecutor;
        }


        public IList<DbccUserOptionsResults> GetDbccUserOptions()
        {
            var retVal = _storedProcedureExecutor.ExecuteMultiResultStoredProcedure<DbccUserOptionsResults>
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
