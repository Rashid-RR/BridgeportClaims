using System.Threading.Tasks;
using BridgeportClaims.Business.Dto;
using BridgeportClaims.Business.Enums;

namespace BridgeportClaims.Business.ApiProvider
{
    public interface IApiCallerProvider
    {
        Task<string> GetAuthenticationBearerTokenAsync();
        Task<bool> CallSignalRApiMethod(SignalRMethodType type, string token, DocumentDto dto, int documentId);
    }
}