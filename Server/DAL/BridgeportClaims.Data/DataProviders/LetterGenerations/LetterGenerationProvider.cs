using System;
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
                const string sp = "[dbo].[uspLetterGenerationData]";
                var ps = new DynamicParameters();
                ps.Add("@ClaimID", claimId, DbType.Int32);
                ps.Add("@UserID", userId, DbType.String, size: 128);
                ps.Add("@PrescriptionID", prescriptionId, DbType.Int32);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var result = conn.Query<LetterGenerationDto>(sp, ps, commandType: CommandType.StoredProcedure);
                return result.SingleOrDefault();
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