using System;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Diaries
{
    public interface IDiaryProvider
    {
        DiariesDto GetDiaries(bool isDefaultSort, DateTime? startDate, DateTime? endDate,
            string sortColumn, string sortDirection, int pageNumber, int pageSize, bool closed);

        void RemoveDiary(int diaryId);
    }
}