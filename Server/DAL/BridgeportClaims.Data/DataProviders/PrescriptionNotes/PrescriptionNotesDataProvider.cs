﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BridgeportClaims.Common.Config;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using NHibernate;
using NHibernate.Transform;

namespace BridgeportClaims.Data.DataProviders.PrescriptionNotes
{
	public class PrescriptionNotesDataProvider : IPrescriptionNotesDataProvider
	{
		private readonly ISessionFactory _factory;

		public PrescriptionNotesDataProvider(ISessionFactory factory)
		{
			_factory = factory;
		}

		public IList<PrescriptionNotesDto> GetPrescriptionNotesByPrescriptionId(int prescriptionId) 
			=> DisposableService.Using(() => _factory.OpenSession(),
				session =>
				{
					return DisposableService.Using(() => session.BeginTransaction(IsolationLevel.ReadCommitted),
						transaction =>
						{
							try
							{
								var notes = session.CreateSQLQuery(
										@"SELECT  [p].[ClaimID] ClaimId
												, [p].[PrescriptionNoteId] PrescriptionNoteId
												, [p].[NoteUpdatedOn] [Date]
												, [p].[PrescriptionNoteType] [Type]
												, [p].[NoteAuthor] EnteredBy
												, [p].[NoteText] [Note]
												, [p].[NoteUpdatedOn] NoteUpdatedOn
										FROM     [dbo].[vwPrescriptionNote] AS [p] WITH ( NOEXPAND )
										WHERE    [p].[PrescriptionId] = :PrescriptionNoteId
										ORDER BY [p].[NoteUpdatedOn] ASC")
									.SetInt32("PrescriptionNoteId", prescriptionId)
									.SetMaxResults(1000)
									.SetResultTransformer(Transformers.AliasToBean(typeof(PrescriptionNotesDto)))
									.List<PrescriptionNotesDto>();
								if (transaction.IsActive)
									transaction.Commit();
								return notes;
							}
							catch
							{
								if (transaction.IsActive)
									transaction.Rollback();
								throw;
							}
						});
				});

		public void AddOrUpdatePrescriptionNote(PrescriptionNoteSaveDto dto, string userId)
		{
			if (null == dto.Prescriptions || dto.Prescriptions.Count < 1)
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
									new SqlParameter("@PrescriptionNoteId", SqlDbType.Int)
										{Value = dto.PrescriptionNoteId.Value};
								cmd.Parameters.Add(prescriptionNoteIdSqlParameter);
							}
							var dt = CreateDataTable(dto.Prescriptions);
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
			table.Columns.Add("PrescriptionId", typeof(int));
			foreach (var prescriptionId in prescriptionIds)
				table.Rows.Add(prescriptionId);
			return table;
		}
	}
}