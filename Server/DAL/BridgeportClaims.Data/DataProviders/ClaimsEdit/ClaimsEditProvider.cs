using System;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.ClaimsEdit
{
    public class ClaimsEditProvider : IClaimsEditProvider
    {
        public void EditClaim(int claimId, string modifiedByUserId, DateTime? dateOfBirth, int genderId, int payorId, int? adjustorId, string adjustorPhone,
            DateTime? dateOfInjury, string adjustorFax) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                DisposableService.Using(() => new SqlCommand("[dbo].[uspEditClaim]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var claimIdParam = cmd.CreateParameter();
                    claimIdParam.Value = claimId;
                    claimIdParam.Direction = ParameterDirection.Input;
                    claimIdParam.DbType = DbType.Int32;
                    claimIdParam.SqlDbType = SqlDbType.Int;
                    claimIdParam.ParameterName = "@ClaimID";
                    cmd.Parameters.Add(claimIdParam);
                    var modifiedByUserIdParam = cmd.CreateParameter();
                    modifiedByUserIdParam.Value = modifiedByUserId ?? (object) DBNull.Value;
                    modifiedByUserIdParam.Direction = ParameterDirection.Input;
                    modifiedByUserIdParam.DbType = DbType.Int32;
                    modifiedByUserIdParam.SqlDbType = SqlDbType.Int;
                    modifiedByUserIdParam.ParameterName = "@ModifiedByUserID";
                    cmd.Parameters.Add(modifiedByUserIdParam);
                    var dateOfBirthParam = cmd.CreateParameter();
                    dateOfBirthParam.Direction = ParameterDirection.Input;
                    dateOfBirthParam.Value = dateOfBirth ?? (object) DBNull.Value;
                    dateOfBirthParam.SqlDbType = SqlDbType.Date;
                    dateOfBirthParam.DbType = DbType.Date;
                    dateOfBirthParam.ParameterName = "@DateOfBirth";
                    cmd.Parameters.Add(dateOfBirthParam);
                    var genderIdParam = cmd.CreateParameter();
                    genderIdParam.Value = genderId;
                    genderIdParam.Direction = ParameterDirection.Input;
                    genderIdParam.DbType = DbType.Int32;
                    genderIdParam.SqlDbType = SqlDbType.Int;
                    genderIdParam.ParameterName = "@GenderID";
                    cmd.Parameters.Add(genderIdParam);
                    var payorIdParam = cmd.CreateParameter();
                    payorIdParam.Value = payorId;
                    payorIdParam.ParameterName = "@PayorID";
                    payorIdParam.DbType = DbType.Int32;
                    payorIdParam.SqlDbType = SqlDbType.Int;
                    payorIdParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(payorIdParam);
                    var adjustorIdParam = cmd.CreateParameter();
                    adjustorIdParam.Value = adjustorId ?? (object) DBNull.Value;
                    adjustorIdParam.ParameterName = "@AdjustorID";
                    adjustorIdParam.Direction = ParameterDirection.Input;
                    adjustorIdParam.DbType = DbType.Int32;
                    adjustorIdParam.SqlDbType = SqlDbType.Int;
                    cmd.Parameters.Add(adjustorIdParam);
                    var adjustorPhoneParam = cmd.CreateParameter();
                    adjustorPhoneParam.ParameterName = "@AdjustorPhone";
                    adjustorPhoneParam.Value = adjustorPhone ?? (object) DBNull.Value;
                    adjustorPhoneParam.Direction = ParameterDirection.Input;
                    adjustorPhoneParam.DbType = DbType.AnsiString;
                    adjustorPhoneParam.Size = 30;
                    adjustorPhoneParam.SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.Add(adjustorPhoneParam);
                    var dateOfInjuryParam = cmd.CreateParameter();
                    dateOfInjuryParam.DbType = DbType.Date;
                    dateOfInjuryParam.SqlDbType = SqlDbType.Date;
                    dateOfInjuryParam.Value = dateOfInjury ?? (object) DBNull.Value;
                    dateOfInjuryParam.SqlDbType = SqlDbType.Date;
                    dateOfInjuryParam.ParameterName = "@DateOfInjury";
                    cmd.Parameters.Add(dateOfInjuryParam);
                    var adjustorFaxParam = cmd.CreateParameter();
                    adjustorFaxParam.ParameterName = "@AdjustorFax";
                    adjustorFaxParam.Value = adjustorFax ?? (object) DBNull.Value;
                    adjustorFaxParam.Direction = ParameterDirection.Input;
                    adjustorFaxParam.DbType = DbType.AnsiString;
                    adjustorFaxParam.Size = 30;
                    adjustorFaxParam.SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.Add(adjustorFaxParam);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                });
            });
    }
}