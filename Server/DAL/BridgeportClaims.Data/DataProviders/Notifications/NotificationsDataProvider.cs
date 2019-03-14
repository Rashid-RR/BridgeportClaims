using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Notifications
{
    public class NotificationsDataProvider : INotificationsDataProvider
    {
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
    }
}
