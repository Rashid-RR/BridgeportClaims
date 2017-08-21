namespace BridgeportClaims.Business.Payments
{
    public class PaymentsBusiness : IPaymentsBusiness
    {
        public bool CheckMultiLinePartialPayments(decimal amountSelected, decimal amountToPost,
            int countOfPrescriptions) => amountSelected == amountToPost || countOfPrescriptions <= 1;
    }
}