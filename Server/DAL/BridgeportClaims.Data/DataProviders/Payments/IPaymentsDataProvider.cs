using System.Threading.Tasks;

namespace BridgeportClaims.Data.DataProviders.Payments
{
    public interface IPaymentsDataProvider
    {
        void ImportPaymentFile(string fileName);
    }
}