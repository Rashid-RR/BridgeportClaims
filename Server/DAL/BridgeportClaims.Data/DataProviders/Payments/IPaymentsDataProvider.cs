using System.Threading.Tasks;

namespace BridgeportClaims.Data.DataProviders.Payments
{
    public interface IPaymentsDataProvider
    {
        Task ImportPaymentFile(string fileName);
    }
}