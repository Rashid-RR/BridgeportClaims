﻿using System;
using System.Collections.Generic;
using System.Data;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Payments
{
    public interface IPaymentsDataProvider
    {
        void PrescriptionPostings(string checkNumber, bool hasSuspense, decimal? suspenseAmountRemaining,
            string toSuspenseNoteText, int documentId, string userId, IList<PaymentPostingDto> paymentPostings);
        IEnumerable<PrescriptionPaymentsDto> GetPrescriptionPaymentsDtos(int claimId, string sortColumn,
            string direction, int pageNumber, int pageSize, string secondarySortColumn, string secondaryDirection);
        IEnumerable<ClaimsWithPrescriptionDetailsDto> GetClaimsWithPrescriptionDetails(IEnumerable<int> claimIds);
        IEnumerable<ClaimsWithPrescriptionCountsDto> GetClaimsWithPrescriptionCounts(string claimNumber, string firstName,
            string lastName, DateTime? rxDate, string invoiceNumber);
        IEnumerable<byte> GetBytesFromDb(string fileName);
        void ImportDataTableIntoDb(DataTable dt);
    }
}