namespace BridgeportClaims.Business.Payments
{
    public interface IPaymentsBusiness
    {
        bool CheckMultiLinePartialPayments(decimal amountSelected, decimal amountToPost, int countOfPrescriptions);
        void ImportPaymentFile(string fileName);
    }
}