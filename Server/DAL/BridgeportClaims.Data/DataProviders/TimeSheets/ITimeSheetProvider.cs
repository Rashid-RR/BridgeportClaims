using System;

namespace BridgeportClaims.Data.DataProviders.TimeSheets
{
    public interface ITimeSheetProvider
    {
        void ClockIn(string userId);
        void ClockOut(string userId);
        DateTime? GetStartTime(string userId);
    }
}