using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.EpisodeNotes
{
    public class EpisodeNoteProvider : IEpisodeNoteProvider
    {
        public IList<EpisodeNotesDto> GetEpisodeNotes(int episodeId) =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspGetEpisodeNotes]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var episodeIdParam = cmd.CreateParameter();
                    episodeIdParam.Value = episodeId;
                    episodeIdParam.ParameterName = "@EpisodeID";
                    episodeIdParam.DbType = DbType.Int32;
                    episodeIdParam.SqlDbType = SqlDbType.Int;
                    episodeIdParam.Direction = ParameterDirection.Input;
                    cmd.Parameters.Add(episodeIdParam);
                    var episodeNotesDto = new List<EpisodeNotesDto>();
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var idOrdinal = reader.GetOrdinal("Id");
                        var ownerOrdinal = reader.GetOrdinal("Owner");
                        var episodeCreatedOrdinal = reader.GetOrdinal("EpisodeCreated");
                        var patientNameOrdinal = reader.GetOrdinal("PatientName");
                        var claimNumberOrdinal = reader.GetOrdinal("ClaimNumber");
                        var writtenByOrdinal = reader.GetOrdinal("WrittenBy");
                        var noteCreatedOrdinal = reader.GetOrdinal("NoteCreated");
                        var noteTextOrdinal = reader.GetOrdinal("NoteText");
                        while (reader.Read())
                        {
                            var result = new EpisodeNotesDto
                            {
                                Id = !reader.IsDBNull(idOrdinal) ? reader.GetInt32(idOrdinal) : throw new ArgumentNullException(nameof(idOrdinal)),
                                Owner = !reader.IsDBNull(ownerOrdinal) ? reader.GetString(ownerOrdinal) : throw new ArgumentNullException(nameof(ownerOrdinal)),
                                EpisodeCreated = !reader.IsDBNull(episodeCreatedOrdinal) ? reader.GetDateTime(episodeCreatedOrdinal) : (DateTime?) null,
                                PatientName = !reader.IsDBNull(patientNameOrdinal) ? reader.GetString(patientNameOrdinal) : null,
                                ClaimNumber = !reader.IsDBNull(claimNumberOrdinal) ? reader.GetString(claimNumberOrdinal) : null,
                                WrittenBy = !reader.IsDBNull(writtenByOrdinal) ? reader.GetString(writtenByOrdinal) : null,
                                NoteCreated = !reader.IsDBNull(noteCreatedOrdinal) ? reader.GetDateTime(noteCreatedOrdinal) : throw new ArgumentNullException(nameof(noteCreatedOrdinal)),
                                NoteText = !reader.IsDBNull(noteTextOrdinal) ? reader.GetString(noteTextOrdinal) : null
                            };
                            episodeNotesDto.Add(result);
                        }
                    });
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    return episodeNotesDto;
                });
            });
    }
}