using System;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.ClaimsEdit
{
    public class ClaimsEditProvider : IClaimsEditProvider
    {
        private readonly DateTime _defaultDateTime = new DateTime(1901, 1, 1);
        private const int DefaultInt = -1;
        private const string DefaultString = "NULL";

        public void EditClaim(int claimId, string modifiedByUserId, DateTime? dateOfBirth, int genderId, int payorId, int? adjustorId, string adjustorPhone,
            DateTime? dateOfInjury, string adjustorFax, string address1, string address2, string city, int? stateId, string postalCode) =>
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
                    if (dateOfBirth != _defaultDateTime)
                    {
                        var dateOfBirthParam = cmd.CreateParameter();
                        dateOfBirthParam.Direction = ParameterDirection.Input;
                        dateOfBirthParam.Value = dateOfBirth ?? (object) DBNull.Value;
                        dateOfBirthParam.SqlDbType = SqlDbType.Date;
                        dateOfBirthParam.DbType = DbType.Date;
                        dateOfBirthParam.ParameterName = "@DateOfBirth";
                        cmd.Parameters.Add(dateOfBirthParam);
                    }
                    if (genderId != DefaultInt)
                    {
                        var genderIdParam = cmd.CreateParameter();
                        genderIdParam.Value = genderId;
                        genderIdParam.Direction = ParameterDirection.Input;
                        genderIdParam.DbType = DbType.Int32;
                        genderIdParam.SqlDbType = SqlDbType.Int;
                        genderIdParam.ParameterName = "@GenderID";
                        cmd.Parameters.Add(genderIdParam);
                    }
                    if (payorId != DefaultInt)
                    {
                        var payorIdParam = cmd.CreateParameter();
                        payorIdParam.Value = payorId;
                        payorIdParam.ParameterName = "@PayorID";
                        payorIdParam.DbType = DbType.Int32;
                        payorIdParam.SqlDbType = SqlDbType.Int;
                        payorIdParam.Direction = ParameterDirection.Input;
                        cmd.Parameters.Add(payorIdParam);
                    }
                    if (adjustorId != DefaultInt)
                    {
                        var adjustorIdParam = cmd.CreateParameter();
                        adjustorIdParam.Value = adjustorId ?? (object) DBNull.Value;
                        adjustorIdParam.ParameterName = "@AdjustorID";
                        adjustorIdParam.Direction = ParameterDirection.Input;
                        adjustorIdParam.DbType = DbType.Int32;
                        adjustorIdParam.SqlDbType = SqlDbType.Int;
                        cmd.Parameters.Add(adjustorIdParam);
                    }
                    if (adjustorPhone != DefaultString)
                    {
                        var adjustorPhoneParam = cmd.CreateParameter();
                        adjustorPhoneParam.ParameterName = "@AdjustorPhone";
                        adjustorPhoneParam.Value = adjustorPhone ?? (object) DBNull.Value;
                        adjustorPhoneParam.Direction = ParameterDirection.Input;
                        adjustorPhoneParam.DbType = DbType.AnsiString;
                        adjustorPhoneParam.Size = 30;
                        adjustorPhoneParam.SqlDbType = SqlDbType.VarChar;
                        cmd.Parameters.Add(adjustorPhoneParam);
                    }
                    if (dateOfInjury != _defaultDateTime)
                    {
                        var dateOfInjuryParam = cmd.CreateParameter();
                        dateOfInjuryParam.DbType = DbType.Date;
                        dateOfInjuryParam.SqlDbType = SqlDbType.Date;
                        dateOfInjuryParam.Value = dateOfInjury ?? (object) DBNull.Value;
                        dateOfInjuryParam.SqlDbType = SqlDbType.Date;
                        dateOfInjuryParam.ParameterName = "@DateOfInjury";
                        cmd.Parameters.Add(dateOfInjuryParam);
                    }
                    if (adjustorFax != DefaultString)
                    {
                        var adjustorFaxParam = cmd.CreateParameter();
                        adjustorFaxParam.ParameterName = "@AdjustorFax";
                        adjustorFaxParam.Value = adjustorFax ?? (object) DBNull.Value;
                        adjustorFaxParam.Direction = ParameterDirection.Input;
                        adjustorFaxParam.DbType = DbType.AnsiString;
                        adjustorFaxParam.Size = 30;
                        adjustorFaxParam.SqlDbType = SqlDbType.VarChar;
                        cmd.Parameters.Add(adjustorFaxParam);
                    }
                    if (address1 != DefaultString)
                    {
                        var address1Param = cmd.CreateParameter();
                        address1Param.Value = address1 ?? (object) DBNull.Value;
                        address1Param.ParameterName = "@Address1";
                        address1Param.Size = 255;
                        address1Param.DbType = DbType.AnsiString;
                        address1Param.SqlDbType = SqlDbType.VarChar;
                        address1Param.Direction = ParameterDirection.Input;
                        cmd.Parameters.Add(address1Param);
                    }
                    if (address2 != DefaultString)
                    {
                        var address2Param = cmd.CreateParameter();
                        address2Param.Value = address2 ?? (object) DBNull.Value;
                        address2Param.ParameterName = "@Address2";
                        address2Param.Size = 255;
                        address2Param.DbType = DbType.AnsiString;
                        address2Param.SqlDbType = SqlDbType.VarChar;
                        address2Param.Direction = ParameterDirection.Input;
                        cmd.Parameters.Add(address2Param);
                    }
                    if (city != DefaultString)
                    {
                        var cityParam = cmd.CreateParameter();
                        cityParam.Value = city ?? (object) DBNull.Value;
                        cityParam.ParameterName = "@City";
                        cityParam.DbType = DbType.AnsiString;
                        cityParam.Size = 155;
                        cityParam.SqlDbType = SqlDbType.VarChar;
                        cityParam.Direction = ParameterDirection.Input;
                        cmd.Parameters.Add(cityParam);
                    }
                    if (stateId != DefaultInt)
                    {
                        var stateIdParam = cmd.CreateParameter();
                        stateIdParam.Value = stateId ?? (object) DBNull.Value;
                        stateIdParam.DbType = DbType.Int32;
                        stateIdParam.SqlDbType = SqlDbType.Int;
                        stateIdParam.Direction = ParameterDirection.Input;
                        stateIdParam.ParameterName = "@StateID";
                        cmd.Parameters.Add(stateIdParam);
                    }
                    if (postalCode != DefaultString)
                    {
                        var postalCodeParam = cmd.CreateParameter();
                        postalCodeParam.Value = postalCode ?? (object) DBNull.Value;
                        postalCodeParam.DbType = DbType.AnsiString;
                        postalCodeParam.SqlDbType = SqlDbType.VarChar;
                        postalCodeParam.Size = 100;
                        postalCodeParam.ParameterName = "@PostalCode";
                        postalCodeParam.Direction = ParameterDirection.Input;
                        cmd.Parameters.Add(postalCodeParam);
                    }
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    cmd.ExecuteNonQuery();
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                });
            });
    }
}