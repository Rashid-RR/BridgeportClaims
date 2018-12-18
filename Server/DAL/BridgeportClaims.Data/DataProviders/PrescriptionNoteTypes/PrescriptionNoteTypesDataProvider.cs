using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using cs = BridgeportClaims.Common.Config.ConfigService;
using SQLinq.Dapper;
using SQLinq;

namespace BridgeportClaims.Data.DataProviders.PrescriptionNoteTypes
{
    public class PrescriptionNoteTypesDataProvider : IPrescriptionNoteTypesDataProvider
    {
        public IEnumerable<PrescriptionNoteTypesDto> GetPrescriptionNoteTypes()
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query(new SQLinq<PrescriptionNoteTypesDto>().OrderBy(o => o.TypeName)
                    ?.Select(s => new {s.PrescriptionNoteTypeId, s.TypeName}));
            });
    }
}
