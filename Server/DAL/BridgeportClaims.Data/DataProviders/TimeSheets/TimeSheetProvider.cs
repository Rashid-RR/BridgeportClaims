using System;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using Dapper;

namespace BridgeportClaims.Data.DataProviders.TimeSheets
{
    public class TimeSheetProvider : ITimeSheetProvider
    {
        public void ClockIn(string userId) =>
            DisposableService.Using(() => new SqlConnection(), conn =>
            {
                const string sp = "[dbo].[uspUserTimeSheetClockIn]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute(sp, new {UserID = userId}, commandType: CommandType.StoredProcedure);
            });

        public void ClockOut(string userId) =>
            DisposableService.Using(() => new SqlConnection(), conn =>
            {
                const string sp = "[dbo].[uspUserTimeSheetClockOut]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                conn.Execute(sp, new {UserID = userId}, commandType: CommandType.StoredProcedure);
            });

        public DateTime? GetStartTime(string userId) =>
            DisposableService.Using(() => new SqlConnection(), conn =>
            {
                const string sp = "[dbo].[uspGetUserTimeSheetClockInTime]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.ExecuteScalar<DateTime?>(sp, new {UserID = userId}, commandType: CommandType.StoredProcedure);
            });
    }
}
