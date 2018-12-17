using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using SQLinq.Dapper;
using SQLinq;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Accounts
{
    public class AspNetUsersProvider : IAspNetUsersProvider
    {
        public void UpdatePersonalData(string userId, string firstName, string lastName, string extension) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
                {
                    DisposableService.Using(() => new SqlCommand("[dbo].[uspUpdatePersonalData]", conn), cmd =>
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var userIdParam = cmd.CreateParameter();
                        userIdParam.DbType = DbType.String;
                        userIdParam.SqlDbType = SqlDbType.NVarChar;
                        userIdParam.Size = 128;
                        userIdParam.Direction = ParameterDirection.Input;
                        userIdParam.Value = userId ?? (object) DBNull.Value;
                        userIdParam.ParameterName = "@UserID";
                        cmd.Parameters.Add(userIdParam);
                        var firstNameParam = cmd.CreateParameter();
                        firstNameParam.DbType = DbType.String;
                        firstNameParam.SqlDbType = SqlDbType.NVarChar;
                        firstNameParam.Direction = ParameterDirection.Input;
                        firstNameParam.Value = firstName ?? (object) DBNull.Value;
                        firstNameParam.ParameterName = "@FirstName";
                        cmd.Parameters.Add(firstNameParam);
                        var lastNameParam = cmd.CreateParameter();
                        lastNameParam.DbType = DbType.String;
                        lastNameParam.SqlDbType = SqlDbType.NVarChar;
                        lastNameParam.Direction = ParameterDirection.Input;
                        lastNameParam.Value = lastName ?? (object)DBNull.Value;
                        lastNameParam.ParameterName = "@LastName";
                        cmd.Parameters.Add(lastNameParam);
                        var extensionParam = cmd.CreateParameter();
                        extensionParam.DbType = DbType.String;
                        extensionParam.SqlDbType = SqlDbType.NVarChar;
                        extensionParam.Direction = ParameterDirection.Input;
                        extensionParam.Value = extension ?? (object)DBNull.Value;
                        extensionParam.ParameterName = "@Extension";
                        cmd.Parameters.Add(extensionParam);
                        if (conn.State != ConnectionState.Open)
                            conn.Open();
                        cmd.ExecuteNonQuery();
                        if (conn.State != ConnectionState.Closed)
                            conn.Close();
                    });
                });

        public IEnumerable<AspNetUsersDto> GetAllUsers() =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query(new SQLinq<AspNetUsersDto>()
                    .Select(s => new {s.Id, s.FirstName, s.LastName}));
            });
    }
}