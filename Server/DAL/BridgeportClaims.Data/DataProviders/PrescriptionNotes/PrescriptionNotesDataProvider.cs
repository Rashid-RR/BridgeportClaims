using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using cs = BridgeportClaims.Common.Config.ConfigService;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Common.Extensions;
using BridgeportClaims.Data.Dtos;
using Dapper;

namespace BridgeportClaims.Data.DataProviders.PrescriptionNotes
{
    public class PrescriptionNotesDataProvider : IPrescriptionNotesDataProvider
    {
        public IEnumerable<PrescriptionNotesDto> GetPrescriptionNotesByPrescriptionId(int prescriptionId)
            => DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                const string sp = "[dbo].[uspGetPrescriptionNotes]";
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return conn.Query<PrescriptionNotesDto>(sp, new {PrescriptionID = prescriptionId},
                    commandType: CommandType.StoredProcedure);
            });

        public void AddOrUpdatePrescriptionNote(PrescriptionNoteSaveDto dto, string userId)
        {
            if (null == dto.Prescriptions || dto.Prescriptions.Count < 1)
                throw new Exception(
                    "Error. There needs to be at least one or more Prescription ID's' " +
                    "associated to this Prescription Note.");
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()),
                con =>
                {
                    DisposableService.Using(() => new SqlCommand("[dbo].[uspSavePrescriptionNote]", con),
                        cmd =>
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            var claimIdSqlParameter =
                                new SqlParameter("@ClaimID", SqlDbType.Int) {Value = dto.ClaimId};
                            var prescriptionNoteTypeIdSqlParameter =
                                new SqlParameter("@PrescriptionNoteTypeID", SqlDbType.Int)
                                    {Value = dto.PrescriptionNoteTypeId};
                            cmd.Parameters.Add(claimIdSqlParameter);
                            cmd.Parameters.Add(prescriptionNoteTypeIdSqlParameter);
                            var noteTextSqlParameter =
                                new SqlParameter("@NoteText", SqlDbType.VarChar) {Value = dto.NoteText};
                            cmd.Parameters.Add(noteTextSqlParameter);
                            var enteredByUserIdSqlParameter =
                                new SqlParameter("@EnteredByUserID", SqlDbType.NVarChar)
                                    {Value = userId};
                            cmd.Parameters.Add(enteredByUserIdSqlParameter);
                            if (null != dto.PrescriptionNoteId && dto.PrescriptionNoteId.Value > 0)
                            {
                                var prescriptionNoteIdSqlParameter =
                                    new SqlParameter("@PrescriptionNoteID", SqlDbType.Int)
                                        {Value = dto.PrescriptionNoteId.Value};
                                cmd.Parameters.Add(prescriptionNoteIdSqlParameter);
                            }
                            var diaryFollowUpDateParameter = new SqlParameter("@FollowUpDate", SqlDbType.Date)
                                {Value = dto.IsDiaryEntry ? dto.FollowUpDate.ToNullableFormattedDateTime() : (object) DBNull.Value};
                            cmd.Parameters.Add(diaryFollowUpDateParameter);
                            var dt = CreateDataTable(dto.Prescriptions);
                            var prescriptionSqlParameter = new SqlParameter("@Prescription", SqlDbType.Structured)
                            {
                                Value = dt,
                                TypeName = "[dbo].[udtID]"
                            };
                            cmd.Parameters.Add(prescriptionSqlParameter);
                            if (con.State != ConnectionState.Open)
                                con.Open();
                            cmd.ExecuteNonQuery();
                        });
                });
        }


        private static DataTable CreateDataTable(IEnumerable<int> prescriptionIds)
        {
            var table = new DataTable();
            table.Columns.Add("PrescriptionPaymentId", typeof(int));
            foreach (var prescriptionId in prescriptionIds)
                table.Rows.Add(prescriptionId);
            return table;
        }
    }
}