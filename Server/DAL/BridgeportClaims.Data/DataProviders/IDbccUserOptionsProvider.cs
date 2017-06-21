using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders
{
    public interface IDbccUserOptionsProvider
    {
        IList<DbccUserOptionsResults> GetDbccUserOptions();
        bool IsSessionUsingReadCommittedSnapshotIsolation();
    }
}