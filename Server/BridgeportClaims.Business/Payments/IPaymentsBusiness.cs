using BridgeportClaims.Business.Models;

namespace BridgeportClaims.Business.Payments
{
    public interface IPaymentsBusiness
    {
        bool CheckMultiLinePartialPayments(decimal amountSelected, decimal amountToPost,
            int countOfPrescriptions);

        decimal PostPartialPayment(string userId, PaymentInputsModel model);
    }
}