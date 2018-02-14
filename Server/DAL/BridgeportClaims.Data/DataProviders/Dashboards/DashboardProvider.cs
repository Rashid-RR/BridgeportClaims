using System;
using System.Data;
using System.Data.SqlClient;
using BridgeportClaims.Common.Disposable;
using BridgeportClaims.Data.Dtos;
using cs = BridgeportClaims.Common.Config.ConfigService;

namespace BridgeportClaims.Data.DataProviders.Dashboards
{
    public class DashboardProvider : IDashboardProvider
    {
        public DashboardDto GetDashboardKpis() =>
            DisposableService.Using(() => new SqlConnection(cs.GetDbConnStr()), conn =>
            {
                return DisposableService.Using(() => new SqlCommand("[dbo].[uspDashboard]", conn), cmd =>
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var dashboardDto = new DashboardDto();
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    DisposableService.Using(cmd.ExecuteReader, reader =>
                    {
                        var lastWorkDateOrdinal = reader.GetOrdinal("LastWorkDate");
                        var totalImagesScannedOrdinal = reader.GetOrdinal("TotalImagesScanned");
                        var totalImagesIndexedOrdinal = reader.GetOrdinal("TotalImagesIndexed");
                        var totalImagesRemainingOrdinal = reader.GetOrdinal("TotalImagesRemaining");
                        var diariesAddedOrdinal = reader.GetOrdinal("DiariesAdded");
                        var totalDiariesResolvedOrdinal = reader.GetOrdinal("TotalDiariesResolved");
                        var totalDiariesUnresolvedOrdinal = reader.GetOrdinal("TotalDiariesUnResolved");
                        var newClaimsOrdinal = reader.GetOrdinal("NewClaims");
                        var newPrescriptionsOrdinal = reader.GetOrdinal("NewPrescriptions");
                        var newReversedPrescriptionsOrdinal = reader.GetOrdinal("NewReversedPrescriptions");
                        var newInvoicesPrintedOrdinal = reader.GetOrdinal("NewInvoicesPrinted");
                        var newPaymentsPostedOrdinal = reader.GetOrdinal("NewPaymentsPosted");
                        var newEpisodesOrdinal = reader.GetOrdinal("NewEpisodes");
                        var totalResolvedEpisodesOrdinal = reader.GetOrdinal("TotalResolvedEpisodes");
                        var totalUnresolvedEpisodesOrdinal = reader.GetOrdinal("TotalUnresolvedEpisodes");
                        var fileWatcherHealthyOrdinal = reader.GetOrdinal("FileWatcherHealthy");
                        while (reader.Read())
                        {
                            dashboardDto.LastWorkDate = !reader.IsDBNull(lastWorkDateOrdinal) ? reader.GetDateTime(lastWorkDateOrdinal) : (DateTime?) null;
                            dashboardDto.TotalImagesScanned = !reader.IsDBNull(totalImagesScannedOrdinal) ? reader.GetInt32(totalImagesScannedOrdinal) : (int?) null;
                            dashboardDto.TotalImagesIndexed = !reader.IsDBNull(totalImagesIndexedOrdinal) ? reader.GetInt32(totalImagesIndexedOrdinal) : (int?) null;
                            dashboardDto.TotalImagesRemaining = !reader.IsDBNull(totalImagesRemainingOrdinal) ? reader.GetInt32(totalImagesRemainingOrdinal) : (int?) null;
                            dashboardDto.DiariesAdded = !reader.IsDBNull(diariesAddedOrdinal) ? reader.GetInt32(diariesAddedOrdinal) : (int?) null;
                            dashboardDto.TotalDiariesResolved = !reader.IsDBNull(totalDiariesResolvedOrdinal) ? reader.GetInt32(totalDiariesResolvedOrdinal) : (int?) null;
                            dashboardDto.TotalDiariesUnResolved = !reader.IsDBNull(totalDiariesUnresolvedOrdinal) ? reader.GetInt32(totalDiariesUnresolvedOrdinal) : (int?) null;
                            dashboardDto.NewClaims = !reader.IsDBNull(newClaimsOrdinal) ? reader.GetInt32(newClaimsOrdinal) : (int?) null;
                            dashboardDto.NewPrescriptions = !reader.IsDBNull(newPrescriptionsOrdinal) ? reader.GetInt32(newPrescriptionsOrdinal) : (int?) null;
                            dashboardDto.NewReversedPrescriptions = !reader.IsDBNull(newReversedPrescriptionsOrdinal) ? reader.GetInt32(newReversedPrescriptionsOrdinal) : (int?) null;
                            dashboardDto.NewInvoicesPrinted = !reader.IsDBNull(newInvoicesPrintedOrdinal) ? reader.GetInt32(newInvoicesPrintedOrdinal) : (int?) null;
                            dashboardDto.NewPaymentsPosted = !reader.IsDBNull(newPaymentsPostedOrdinal) ? reader.GetDecimal(newPaymentsPostedOrdinal) : (decimal?) null;
                            dashboardDto.NewEpisodes = !reader.IsDBNull(newEpisodesOrdinal) ? reader.GetInt32(newEpisodesOrdinal) : (int?)null;
                            dashboardDto.TotalResolvedEpisodes = !reader.IsDBNull(totalResolvedEpisodesOrdinal) ? reader.GetInt32(totalResolvedEpisodesOrdinal) : (int?)null;
                            dashboardDto.TotalUnresolvedEpisodes = !reader.IsDBNull(totalUnresolvedEpisodesOrdinal) ? reader.GetInt32(totalUnresolvedEpisodesOrdinal) : (int?)null;
                            dashboardDto.FileWatcherHealthy = !reader.IsDBNull(fileWatcherHealthyOrdinal) ? reader.GetBoolean(fileWatcherHealthyOrdinal) : throw new ArgumentNullException(nameof(fileWatcherHealthyOrdinal));
                        }
                    });
                    if (conn.State != ConnectionState.Closed)
                        conn.Close();
                    return dashboardDto;
                });
            });
    }
}