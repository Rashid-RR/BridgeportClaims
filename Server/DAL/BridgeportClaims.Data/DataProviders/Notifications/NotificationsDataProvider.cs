using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using cs = BridgeportClaims.Common.Config.ConfigService;
using Dapper;

namespace BridgeportClaims.Data.DataProviders.Notifications
{
    public class NotificationsDataProvider : INotificationsDataProvider
    {
        public IEnumerable<NotificationDto> GetNotifications() =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspGetNotifications]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<NotificationDto>(sp, commandType: CommandType.StoredProcedure);
            });

        public void DismissNotification(int notificationId, string dismissedByUserId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspDismissNotification]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@NotificationID", notificationId, DbType.Int32);
                ps.Add("@DismissedByUserID", dismissedByUserId, DbType.String, size: 128);
                conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
            });

        public void DismissEnvisionNotification(int prescriptionId, decimal billedAmount, string modifiedByUserId, int? payorId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspDismissEnvisionNotification]";
                var ps = new DynamicParameters();
                ps.Add("@PrescriptionID", prescriptionId, DbType.Int32);
                ps.Add("@BilledAmount", billedAmount, DbType.Decimal);
                ps.Add("@ModifiedByUserID", modifiedByUserId, DbType.String, size: 128);
                if (null != payorId)
                {
                    ps.Add("@PayorID", payorId, DbType.Int32);
                }
                conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
            });
    }
}
