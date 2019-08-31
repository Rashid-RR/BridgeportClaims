using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;
using s = BridgeportClaims.Common.Constants.StringConstants;

namespace BridgeportClaims.Data.DataProviders.LetterGenerations
{
    public class LetterGenerationProvider : ILetterGenerationProvider
    {
        public LetterGenerationDto GetLetterGenerationData(int claimId, string userId, int prescriptionId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspLetterGenerationData]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var claimIdParam = cmd.CreateParameter();
                    claimIdParam.Value = claimId;
                    claimIdParam.SqlDbType = SqlDbType.Int;
                    claimIdParam.DbType = DbType.Int32;
                    claimIdParam.Direction = ParameterDirection.Input;
                    claimIdParam.ParameterName = "@ClaimID";
                    cmd.Parameters.Add(claimIdParam);
                    var userIdParam = cmd.CreateParameter();
                    userIdParam.Value = userId ?? (object) DBNull.Value;
                    userIdParam.ParameterName = "@UserID";
                    userIdParam.DbType = DbType.String;
                    userIdParam.SqlDbType = SqlDbType.NVarChar;
                    userIdParam.Size = 128;
                    userIdParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(userIdParam);
                    var prescriptionIdParam = cmd.CreateParameter();
                    prescriptionIdParam.Value = prescriptionId;
                    prescriptionIdParam.Direction = ParameterDirection.Input;
                    prescriptionIdParam.DbType = DbType.Int32;
                    prescriptionIdParam.SqlDbType= SqlDbType.Int;
                    prescriptionIdParam.ParameterName = "@PrescriptionID";
                    cmd.Parameters.Add(prescriptionIdParam);
                    var retVal = new List<LetterGenerationDto>();
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    DisposableService.Using(cmd.ExecuteReader, reader => 
                    {
                        var todayDateParam = reader.GetOrdinal("TodaysDate");
                        var firstNameParam = reader.GetOrdinal("FirstName");
                        var lastNameParam = reader.GetOrdinal("LastName");
                        var address1Param = reader.GetOrdinal("Address1");
                        var address2Param = reader.GetOrdinal("Address2");
                        var cityParam = reader.GetOrdinal("City");
                        var stateCodeParam = reader.GetOrdinal("StateCode");
                        var postalCodeParam = reader.GetOrdinal("PostalCode");
                        var letterNameParam = reader.GetOrdinal("LetterName");
                        var userFirstNameParam = reader.GetOrdinal("UserFirstName");
                        var userLastNameParam = reader.GetOrdinal("UserLastName");
                        var pharmacyNameParam = reader.GetOrdinal("PharmacyName");
                        var extensionParam = reader.GetOrdinal("Extension");
                        while (reader.Read())
                        {
                            var letterGenerationDto = new LetterGenerationDto
                            {
                                TodaysDate = reader.GetDateTime(todayDateParam),
                                FirstName = !reader.IsDBNull(firstNameParam)
                                    ? reader.GetString(firstNameParam)
                                    : string.Empty,
                                LastName = !reader.IsDBNull(lastNameParam)
                                    ? reader.GetString(lastNameParam)
                                    : string.Empty,
                                Address1 = !reader.IsDBNull(address1Param)
                                    ? reader.GetString(address1Param)
                                    : string.Empty,
                                Address2 = !reader.IsDBNull(address2Param)
                                    ? reader.GetString(address2Param)
                                    : string.Empty,
                                City = !reader.IsDBNull(cityParam) ? reader.GetString(cityParam) : string.Empty,
                                StateCode = !reader.IsDBNull(stateCodeParam)
                                    ? (reader.GetString(stateCodeParam) == "NA"
                                        ? string.Empty
                                        : reader.GetString(stateCodeParam))
                                    : string.Empty,
                                PostalCode = !reader.IsDBNull(postalCodeParam)
                                    ? reader.GetString(postalCodeParam)
                                    : string.Empty,
                                LetterName = !reader.IsDBNull(letterNameParam)
                                    ? reader.GetString(letterNameParam)
                                    : string.Empty,
                                UserFirstName = !reader.IsDBNull(userFirstNameParam)
                                    ? reader.GetString(userFirstNameParam)
                                    : string.Empty,
                                UserLastName = !reader.IsDBNull(userLastNameParam)
                                    ? reader.GetString(userLastNameParam)
                                    : string.Empty,
                                PharmacyName = !reader.IsDBNull(pharmacyNameParam)
                                    ? reader.GetString(pharmacyNameParam)
                                    : string.Empty,
                                Extension = !reader.IsDBNull(extensionParam)
                                    ? reader.GetString(extensionParam)
                                    : string.Empty
                            };
                            retVal.Add(letterGenerationDto);
                        }
                    });
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }
                    return retVal.SingleOrDefault();
                });
            });

        public DrNoteLetterGenerationDto GetDrNoteLetterGenerationData(int claimId, string userId,
            int firstPrescriptionId, DataTable dt) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspDrNoteLetterGenerationData]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@ClaimID", claimId, DbType.Int32);
                ps.Add("@UserID", userId, DbType.String, size: 128);
                ps.Add("@FirstPrescriptionID", firstPrescriptionId, DbType.Int32);
                ps.Add("@PrescriptionIds", dt.AsTableValuedParameter(s.UdtId));
                var multi = conn.QueryMultiple(sp, ps, commandType: CommandType.StoredProcedure);
                var result = multi.Read<DrNoteLetterGenerationResultsDto>()?.SingleOrDefault();
                if (null == result)
                {
                    throw new Exception("Could not retrieve data from the database to populate the document.");
                }
                var scripts = multi.Read<DrNoteLetterGenerationScriptsDto>()?.ToList();
                if (null == scripts || scripts.Count < 1)
                {
                    throw new Exception("One or more scripts could not be found from the database.");
                }
                var retVal = new DrNoteLetterGenerationDto {Result = result, Scripts = scripts};
                return retVal;
            });
    }
}