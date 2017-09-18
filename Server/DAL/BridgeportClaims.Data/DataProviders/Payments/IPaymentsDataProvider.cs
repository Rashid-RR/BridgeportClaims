﻿using System;
using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Payments
{
    public interface IPaymentsDataProvider
    {
        decimal GetAmountRemaining(IList<int> claimsIds, string checkNumber);
        void ImportPaymentFile(string fileName);
        IList<PrescriptionPaymentsDto> GetPrescriptionPaymentsDtos(int claimId, string sortColumn,
            string direction, int pageNumber, int pageSize, string secondarySortColumn, string secondaryDirection);
        IList<ClaimsWithPrescriptionDetailsDto> GetClaimsWithPrescriptionDetails(int claimId);
        IList<ClaimsWithPrescriptionCountsDto> GetClaimsWithPrescriptionCounts(string claimNumber, string firstName,
            string lastName, DateTime? rxDate, string invoiceNumber);
        PostPaymentReturnDto PostPayment(IEnumerable<int> prescriptionIds, string checkNumber,
            decimal checkAmount, decimal amountSelected, decimal amountToPost);
    }
}