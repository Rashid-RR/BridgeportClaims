using System;

namespace BridgeportClaims.Data.DataProviders.PrescriptionPayments
{
    public interface IPrescriptionPaymentProvider
    {
        void DeletePrescriptionPayment(int prescriptionPaymentId);

        void UpdatePrescriptionPayment(int prescriptionPaymentId, string checkNumber, decimal amountPaid,
            DateTime? datePosted, int prescriptionId, string userId);
    }
}