using System;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Notifications.PayorLetterName
{
    public class PayorLetterNameProvider : IPayorLetterNameProvider
    {
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