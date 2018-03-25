﻿using System;
using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Diaries
{
    public interface IDiaryProvider
    {
        IEnumerable<DiaryOwnerDto> GetDiaryOwners();
        DiariesDto GetDiaries(bool isDefaultSort, DateTime? startDate, DateTime? endDate,
            string sortColumn, string sortDirection, int pageNumber, int pageSize, bool closed, string userId);
        void RemoveDiary(int diaryId);
    }
}