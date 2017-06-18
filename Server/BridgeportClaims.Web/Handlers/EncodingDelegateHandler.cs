using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BridgeportClaims.Web.Handlers
{
    public class EncodingDelegateHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken).ContinueWith(
                (responseToCompleteTask) =>
                {
                    var response = responseToCompleteTask.Result;

                    if (response.RequestMessage.Headers.AcceptEncoding == null ||
                        response.RequestMessage.Headers.AcceptEncoding.Count <= 0) return response;
                    var encodingType = response.RequestMessage.Headers.AcceptEncoding.First().Value;

                    response.Content = new CompressedContent(response.Content, encodingType);

                    return response;
                },
                TaskContinuationOptions.OnlyOnRanToCompletion);
        }
    }
}