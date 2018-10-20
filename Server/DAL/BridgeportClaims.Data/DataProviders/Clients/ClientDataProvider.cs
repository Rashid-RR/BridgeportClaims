using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using cs = BridgeportClaims.Common.Config.ConfigService;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using Dapper;

namespace BridgeportClaims.Data.DataProviders.Clients
{
    public class ClientDataProvider : IClientDataProvider
    {
        private const string TypesQuery =
            "SELECT rt.ReferralTypeID ReferralTypeId, rt.TypeName FROM client.ReferralType AS rt";

        private const string StatesQuery =
            "SELECT u.StateID StateId, u.StateName FROM dbo.UsState AS u WHERE u.IsTerritory = 0";

        public void InsertReferral(ReferralDto referral) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[client].[uspReferralInsert]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@ClaimNumber", referral.ClaimNumber, DbType.AnsiString, size: 255);
                ps.Add("@JurisdictionStateID", referral.JurisdictionStateId, DbType.Int32);
                ps.Add("@LastName", referral.LastName, DbType.AnsiString, size: 155);
                ps.Add("@FirstName", referral.FirstName, DbType.AnsiString, size: 155);
                ps.Add("@DateOfBirth", referral.DateOfBirth, DbType.Date);
                ps.Add("@InjuryDate", referral.InjuryDate, DbType.Date);
                ps.Add("@Notes", referral.Notes, DbType.AnsiString, size: 8000);
                ps.Add("@ReferredBy", referral.ReferredBy, DbType.String, size: 128);
                ps.Add("@ReferralDate", referral.ReferralDate, DbType.DateTime2);
                ps.Add("@ReferralTypeID", referral.ReferralTypeId, DbType.Byte);
                ps.Add("@EligibilityStart", referral.EligibilityStart, DbType.DateTime2);
                ps.Add("@EligibilityEnd", referral.EligibilityEnd, DbType.DateTime2);
                ps.Add("@Address1", referral.Address1, DbType.AnsiString, size: 255);
                ps.Add("@Address2", referral.Address2, DbType.AnsiString, size: 255);
                ps.Add("@City", referral.City, DbType.AnsiString, size: 155);
                ps.Add("@StateID", referral.StateId, DbType.Int32);
                ps.Add("@PostalCode", referral.PostalCode, DbType.AnsiString, size: 100);
                ps.Add("@PatientPhone", referral.PatientPhone, DbType.AnsiString, size: 30);
                ps.Add("@AdjustorName", referral.AdjustorName, DbType.AnsiString, size: 255);
                ps.Add("@AdjustorPhone", referral.AdjustorPhone, DbType.AnsiString, size: 30);
                conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
            });

        public IEnumerable<ReferralTypeDto> GetReferralTypes() =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<ReferralTypeDto>(TypesQuery, commandType: CommandType.Text);
            });

        public IEnumerable<UsStateDto> GetUsStates() =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<UsStateDto>(StatesQuery, commandType: CommandType.Text)?.OrderBy(x => x.StateName);
            });
    }
}
