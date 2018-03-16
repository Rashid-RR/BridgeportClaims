using System;
using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;
using BridgeportClaims.Data.Enums;

namespace BridgeportClaims.Data.DataProviders.Prescriptions
{
    public interface IPrescriptionsDataProvider
    {
        EntityOperation AddOrUpdatePrescriptionStatus(int prescriptionId, int prescriptionStatusId);
        UnpaidScriptsDto GetUnpaidScripts(bool isDefaultSort, DateTime? startDate, DateTime? endDate,
            string sort, string sortDirection, int page, int pageSize);
        IList<string> GetFileUrlsFromPrescriptionIds(IList<int> modelPrescriptionIds);
    }
}