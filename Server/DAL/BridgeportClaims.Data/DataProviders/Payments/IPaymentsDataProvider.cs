using System.Data;
using System.Threading.Tasks;

namespace BridgeportClaims.Data.DataProviders.Payments
{
    public interface IPaymentsDataProvider
    {
        byte[] GetBytesFromDbAsync(string fileName);
        Task ImportDataTableIntoDbAsync(DataTable dt);
    }
}