using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using BridgeportClaims.Common.Config;
using BridgeportClaims.Data.DataProviders.PrescriptionNoteTypes;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Repositories;
using BridgeportClaims.Entities.DomainModels;
using Microsoft.AspNet.Identity;

namespace BridgeportClaims.Web.Controllers
{
    [Authorize(Roles = "User")]
    [RoutePrefix("api/prescriptionnotes")]
    public class PrescriptionNotesController : ApiController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IPrescriptionNoteTypesDataProvider _prescriptionNoteTypesDataProvider;
        private readonly IRepository<PrescriptionNote> _prescriptionNoteRepository;
        private readonly IRepository<AspNetUsers> _aspNetUsersRepository;

        public PrescriptionNotesController(IPrescriptionNoteTypesDataProvider prescriptionNoteTypesDataProvider,
                IRepository<PrescriptionNote> prescriptionNoteRepository, IRepository<AspNetUsers> aspNetUsersRepository)
        {
            _prescriptionNoteTypesDataProvider = prescriptionNoteTypesDataProvider;
            _prescriptionNoteRepository = prescriptionNoteRepository;
            _aspNetUsersRepository = aspNetUsersRepository;
        }

        [HttpPost]
        [Route("savenote")]
        public void AddOrUpdatePrescriptionNote(int claimId, string noteText, 
            int prescriptionNoteTypeId, IList<int> prescriptionIds, int? prescriptionNoteId = null)
        {
            try
            {
                if (null == prescriptionIds || prescriptionIds.Count < 1)
                    throw new Exception(
                        "Error. There needs to be at least one or more Prescription ID's' " +
                        "associated to this Prescription Note.");
                var now = DateTime.Now;
                using (var con = new SqlConnection(ConfigService.GetDbConnStr()))
                {
                    using (var cmd = new SqlCommand("[dbo].[uspSavePrescriptionNote]", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        var claimIdSqlParameter = new SqlParameter("@ClaimID", SqlDbType.Int) {Value = claimId};
                        var prescriptionNoteTypeIdSqlParameter =
                            new SqlParameter("@PrescriptionNoteTypeID", SqlDbType.Int)
                                {Value = prescriptionNoteTypeId};
                        cmd.Parameters.Add(claimIdSqlParameter);
                        cmd.Parameters.Add(prescriptionNoteTypeIdSqlParameter);
                        var noteTextSqlParameter =
                            new SqlParameter("@NoteText", SqlDbType.VarChar) {Value = noteText};
                        cmd.Parameters.Add(noteTextSqlParameter);
                        var enteredByUserIdSqlParameter = new SqlParameter("@EnteredByUserID", SqlDbType.NVarChar)
                            {Value = User.Identity.GetUserId()};
                        cmd.Parameters.Add(enteredByUserIdSqlParameter);
                        if (null != prescriptionNoteId && prescriptionNoteId.Value > 0)
                        {
                            var prescriptionNoteIdSqlParameter =
                                new SqlParameter("@PrescriptionNoteID", SqlDbType.Int)
                                    {Value = prescriptionNoteId.Value};
                            cmd.Parameters.Add(prescriptionNoteIdSqlParameter);
                        }
                        var dt = CreateDataTable(prescriptionIds);
                        var prescriptionSqlParameter = new SqlParameter("@Prescription", SqlDbType.Structured)
                            {Value = dt};
                        cmd.Parameters.Add(prescriptionSqlParameter);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        private static DataTable CreateDataTable(IEnumerable<int> prescriptionIds)
        {
            var table = new DataTable();
            table.Columns.Add("PrescriptionID", typeof(int));
            foreach (var prescriptionId in prescriptionIds)
                table.Rows.Add(prescriptionId);
            return table;
        }

        [HttpGet]
        [Route("notetypes")]
        public IList<PrescriptionNoteTypesDto> GetPrescriptionNoteTypes()
        {
            try
            {
                return _prescriptionNoteTypesDataProvider.GetPrescriptionNoteTypes();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}