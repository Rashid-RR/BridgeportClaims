using System.Threading.Tasks;

namespace BridgeportClaims.Web.Caching
{
    public interface ICachingClearingService
    {
        Task ClearClaimNoteTypeCache();
    }
}