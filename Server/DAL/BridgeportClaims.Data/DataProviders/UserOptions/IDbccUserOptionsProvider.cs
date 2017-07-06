using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.UserOptions
{
    public interface IDbccUserOptionsProvider
    {
        IList<DbccUserOptionsResults> GetDbccUserOptions();
        bool IsSessionUsingReadCommittedSnapshotIsolation();
    }
}