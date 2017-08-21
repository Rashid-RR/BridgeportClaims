using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Payments
{
    public interface IPaymentsDataProvider
    {
        void ImportPaymentFile(string fileName);
        IList<ClaimsWithPrescriptionDetailsDto> GetClaimsWithPrescriptionDetails(IList<int> claimIds);
        IList<ClaimsWithPrescriptionCountsDto> GetClaimsWithPrescriptionCounts(string claimNumber, string firstName,
            string lastName, DateTime? rxDate, string invoiceNumber);
        void PostPayment(IEnumerable<int> prescriptionIds, string checkNumber,
            decimal checkAmount, decimal amountSelected, decimal amountToPost);
    }
}