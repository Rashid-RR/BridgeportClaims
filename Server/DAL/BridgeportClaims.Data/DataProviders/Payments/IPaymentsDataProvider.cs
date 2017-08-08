using System;
using System.Collections.Generic;
using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Payments
{
    public interface IPaymentsDataProvider
    {
        IList<PaymentDto> SearchPayments(string claimNumber, string firstName,
            string lastName, DateTime? rxDate, string invoiceNumber);

        void ImportPaymentFile(string fileName);

        IList<PaymentSearchResultsDto> GetInitialPaymentsSearchResults(string claimNumber, string firstName,
            string lastName, DateTime? rxDate, string invoiceNumber);
    }
}