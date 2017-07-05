using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BridgeportClaims.Business.Models;
using BridgeportClaims.Common.Config;
using BridgeportClaims.Common.Disposable;

namespace BridgeportClaims.Data.DataProviders.PrescriptionNotes
{
    public class PrescriptionNotesDataProvider : IPrescriptionNotesDataProvider
    {
        public void AddOrUpdatePrescriptionNoteAsync(PrescriptionNoteSaveModel model, string userId)
        {
            if (null == model.Prescriptions || model.Prescriptions.Count < 1)
                throw new Exception(
                    "Error. There needs to be at least one or more Prescription ID's' " +
                    "associated to this Prescription Note.");
            DisposableService.Using(() => new SqlConnection(ConfigService.GetDbConnStr()),
                con =>
                {
                    DisposableService.Using(() => new SqlCommand("[dbo].[uspSavePrescriptionNote]", con),
                        cmd =>
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            var claimIdSqlParameter =
                                new SqlParameter("@ClaimID", SqlDbType.Int) {Value = model.ClaimId};
                            var prescriptionNoteTypeIdSqlParameter =
                                new SqlParameter("@PrescriptionNoteTypeID", SqlDbType.Int)
                                    {Value = model.PrescriptionNoteTypeId};
                            cmd.Parameters.Add(claimIdSqlParameter);
                            cmd.Parameters.Add(prescriptionNoteTypeIdSqlParameter);
                            var noteTextSqlParameter =
                                new SqlParameter("@NoteText", SqlDbType.VarChar) {Value = model.NoteText};
                            cmd.Parameters.Add(noteTextSqlParameter);
                            var enteredByUserIdSqlParameter =
                                new SqlParameter("@EnteredByUserID", SqlDbType.NVarChar)
                                    {Value = userId};
                            cmd.Parameters.Add(enteredByUserIdSqlParameter);
                            if (null != model.PrescriptionNoteId && model.PrescriptionNoteId.Value > 0)
                            {
                                var prescriptionNoteIdSqlParameter =
                                    new SqlParameter("@PrescriptionNoteID", SqlDbType.Int)
                                        {Value = model.PrescriptionNoteId.Value};
                                cmd.Parameters.Add(prescriptionNoteIdSqlParameter);
                            }
                            var dt = CreateDataTable(model.Prescriptions);
                            var prescriptionSqlParameter = new SqlParameter("@Prescription", SqlDbType.Structured)
                                {Value = dt};
                            cmd.Parameters.Add(prescriptionSqlParameter);
                            try
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                            finally
                            {
                                con.Close();
                                cmd.Dispose();
                            }
                        });
                });
        }


        private static DataTable CreateDataTable(IEnumerable<int> prescriptionIds)
        {
            var table = new DataTable();
            table.Columns.Add("PrescriptionID", typeof(int));
            foreach (var prescriptionId in prescriptionIds)
                table.Rows.Add(prescriptionId);
            return table;
        }
    }
}