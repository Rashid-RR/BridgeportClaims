using System;
using System.Collections.Generic;
using System.Data;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Enums;

namespace BridgeportClaims.Data.DataProviders.Prescriptions
{
    public interface IPrescriptionsDataProvider
    {
        EntityOperation AddOrUpdatePrescriptionStatus(int prescriptionId,
            int prescriptionStatusId, string userId);
        void ArchiveUnpaidScript(int prescriptionId, string userId);
        UnpaidScriptsMasterDto GetUnpaidScripts(bool isDefaultSort, DateTime? startDate, DateTime? endDate,
            string sort, string sortDirection, int page, int pageSize, bool isArchived, DataTable carriers);
        IEnumerable<string> GetFileUrlsFromPrescriptionIds(IEnumerable<PrescriptionIdDto> dtos);
    }
}