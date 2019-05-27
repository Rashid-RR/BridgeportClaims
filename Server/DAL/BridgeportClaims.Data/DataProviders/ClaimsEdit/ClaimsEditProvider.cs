using System;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using Dapper;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.ClaimsEdit
{
    public class ClaimsEditProvider : IClaimsEditProvider
    {
        private readonly DateTime _defaultDateTime = new DateTime(1901, 1, 1);
        private const int DefaultInt = -1;
        private const string DefaultString = "NULL";

        public void UpdateClaimAttorneyManaged(int claimId, bool isAttorneyManaged, string modifiedByUserId) => DisposableService.Using(() =>
        new SqlConnection(cs.GetDbConnStr()), conn =>
        {
            const string sp = "[dbo].[uspClaimUpdateIsAttorneyManagedDate]";
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            var ps = new DynamicParameters();
            ps.Add("@ClaimID", claimId, DbType.Int32);
            ps.Add("@ModifiedByUserID", modifiedByUserId, DbType.String, size: 128);
            ps.Add("@IsAttorneyManaged", isAttorneyManaged, DbType.Boolean);
            conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
        });

        public void EditClaim(int claimId, string modifiedByUserId, DateTime? ofBirth, int genderId, int payorId,
            int? adjustorId, int? attorneyId,
            DateTime? ofInjury, string address1, string address2, string city, int? stateId, string postalCode,
            int? claimFlex2Id)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspEditClaim]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var ps = new DynamicParameters();
                ps.Add("@ClaimID", claimId, DbType.Int32);
                ps.Add("@ModifiedByUserID", modifiedByUserId, DbType.String, size: 128);
                if (ofBirth != _defaultDateTime)
                {
                    ps.Add("@DateOfBirth", ofBirth, DbType.Date);
                }
                if (genderId != DefaultInt)
                {
                    ps.Add("@GenderID", genderId, DbType.Int32);
                }
                if (payorId != DefaultInt)
                {
                    ps.Add("@PayorID", payorId, DbType.Int32);
                }
                if (adjustorId != DefaultInt)
                {
                    ps.Add("@AdjustorID", adjustorId, DbType.Int32);
                }
                if (attorneyId != DefaultInt)
                {
                    ps.Add("@AttorneyID", attorneyId, DbType.Int32);
                }
                if (ofInjury != _defaultDateTime)
                {
                    ps.Add("@DateOfInjury", ofInjury, DbType.Date);
                }
                if (address1 != DefaultString)
                {
                    ps.Add("@Address1", address1, DbType.AnsiString, size: 255);
                }
                if (address2 != DefaultString)
                {
                    ps.Add("@Address2", address2, DbType.AnsiString, size: 255);
                }
                if (city != DefaultString)
                {
                    ps.Add("@City", city, DbType.AnsiString, size: 155);
                }
                if (stateId != DefaultInt)
                {
                    ps.Add("@StateID", stateId, DbType.Int32);
                }
                if (postalCode != DefaultString)
                {
                    ps.Add("@PostalCode", postalCode, DbType.AnsiString, size: 100);
                }
                if (claimFlex2Id != DefaultInt)
                {
                    ps.Add("@ClaimFlex2ID", claimFlex2Id, DbType.Int32);
                }
                conn.Execute(sp, ps, commandType: CommandType.StoredProcedure);
            });
    }
}