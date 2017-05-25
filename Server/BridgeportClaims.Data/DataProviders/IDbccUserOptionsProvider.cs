using System.Collections.Generic;
using BridgeportClaims.Data.StoredProcedureExecutors.Dtos;

namespace BridgeportClaims.Data.DataProviders
{
    public interface IDbccUserOptionsProvider
    {
        IList<DbccUserOptionsResults> GetDbccUserOptions();
        bool IsSessionUsingReadCommittedSnapshotIsolation();
    }
}