using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Notifications.PayorLetterName
{
    public class PayorLetterNameProvider : IPayorLetterNameProvider
    {
        private const string Query = @"SELECT          [n].[NotificationID]
                                                      , [n].[MessageText]
                                                      , [n].[GeneratedDate]
                                                      , [nt].[TypeName] NotificationType
                                        FROM            [dbo].[Notification]     AS [n]
                                            INNER JOIN  [dbo].[NotificationType] AS [nt] ON [nt].[NotificationTypeID] = [n].[NotificationTypeID]
                                        WHERE           [n].[IsDismissed] = 0
                                        ORDER BY        [n].[UpdatedOnUTC] DESC";

        public IList<NotificationDto> GetNotifications() =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand(Query, conn), cmd =>
                {
                    IList<NotificationDto> retVal = new List<NotificationDto>();
                    cmd.CommandType = CommandType.Text;
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var notificationIdOrdinal = reader.GetOrdinal("NotificationID");
                        var messageTextOrdinal = reader.GetOrdinal("MessageText");
                        var generatedDateOrdinal = reader.GetOrdinal("GeneratedDate");
                        var notificationTypeOrdinal = reader.GetOrdinal("NotificationType");
                        while (reader.Read())
                        {
                            var result = new NotificationDto
                            {
                                NotificationId = reader.GetInt32(notificationIdOrdinal),
                                GeneratedDate = reader.GetDateTime(generatedDateOrdinal),
                                MessageText = !reader.IsDBNull(messageTextOrdinal) ? reader.GetString(messageTextOrdinal) : string.Empty,
                                NotificationType = !reader.IsDBNull(notificationTypeOrdinal) ? reader.GetString(notificationTypeOrdinal) : string.Empty
                            };
                            retVal.Add(result);
                        }
                    });
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    return retVal;
                });
            });

        public void SavePayorLetterNameNotification(int notificationId, string modifiedByUserId, string letterName) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
                {
                    DisposableService.Using(() => new SqlCommand("[dbo].[uspSavePayorLetterNameNotification]", conn), cmd =>
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            var notificationIdParam = cmd.CreateParameter();
                            notificationIdParam.DbType = DbType.Int32;
                            notificationIdParam.SqlDbType = SqlDbType.Int;
                            notificationIdParam.Direction = ParameterDirection.Input;
                            notificationIdParam.Value = notificationId;
                            notificationIdParam.ParameterName = "@NotificationID";
                            cmd.Parameters.Add(notificationIdParam);
                            var modifiedByUserIdParam = cmd.CreateParameter();
                            modifiedByUserIdParam.DbType = DbType.String;
                            modifiedByUserIdParam.SqlDbType = SqlDbType.NVarChar;
                            modifiedByUserIdParam.Size = 128;
                            modifiedByUserIdParam.Value = modifiedByUserId ?? (object) DBNull.Value;
                            modifiedByUserIdParam.ParameterName = "@ModifiedByUserID";
                            cmd.Parameters.Add(modifiedByUserIdParam);
                            var letterNameParam = cmd.CreateParameter();
                            letterNameParam.DbType = DbType.AnsiString;
                            letterNameParam.SqlDbType = SqlDbType.VarChar;
                            letterNameParam.Size = 255;
                            letterNameParam.Value = letterName ?? (object) DBNull.Value;
                            letterNameParam.ParameterName = "@LetterName";
                            cmd.Parameters.Add(letterNameParam);
                            if (conn.State != ConnectionState.Open)
                                conn.Open();
                            cmd.ExecuteNonQuery();
                            if (conn.State != ConnectionState.Closed)
                                conn.Close();
                        });
                });
    }
}