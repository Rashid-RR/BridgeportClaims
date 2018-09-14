using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BridgeportClaims.Common.Config;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;

namespace BridgeportClaims.Data.DataProviders.Users
{
    public class UsersProvider : IUsersProvider
    {
        private const string Jordan = "Jordan";
        private const string Gurney = "Gurney";

        public UserDto GetUser(string userId) =>
            DisposableService.Using(() => new SqlConnection(ConfigService.GetDbConnStr()), conn =>
            {
                const string sp = "dbo.uspGetUserNamesFromID";
                conn.Open();
                var ps = new DynamicParameters();
                ps.Add("@UserName", userId, DbType.String, ParameterDirection.Input, 128);
                return conn.Query<UserDto>(sp, ps, commandType: CommandType.StoredProcedure)?.SingleOrDefault();
            });

        public IEnumerable<UserDto> GetUsers() =>
            DisposableService.Using(() => new SqlConnection(ConfigService.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspGetActiveUsers]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<UserDto>(sp, new {FirstName = Jordan, LastName = Gurney, DateTime.UtcNow},
                    commandType: CommandType.StoredProcedure);
            });
    }
}