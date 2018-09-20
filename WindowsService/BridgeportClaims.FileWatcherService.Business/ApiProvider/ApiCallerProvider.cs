using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BridgeportClaims.Business.Dto;
using BridgeportClaims.Business.Enums;
using BridgeportClaims.Business.Logging;
using BridgeportClaims.Business.Security;
using NLog;
using ServiceStack;
using ServiceStack.Text;
using cs = BridgeportClaims.Business.ConfigService.ConfigService;
using c = BridgeportClaims.Business.StringConstants.Constants;

namespace BridgeportClaims.Business.ApiProvider
{
    public class ApiCallerProvider : IApiCallerProvider
    {
        private readonly string _apiHostName = cs.GetAppSetting(c.ApiHostNameKey);
        private readonly Lazy<ILogger> _logger;

        public ApiCallerProvider(Lazy<ILogger> logger)
        {
            _logger = logger;
        }

        private const string AccessToken = "access_token";
        private const string Message = "message";

        public async Task<string> GetAuthenticationBearerTokenAsync()
        {
            var client = new HttpClient();
            var authUrlPath = cs.GetAppSetting(c.AuthenticationApiUrlKey);
            var request = new HttpRequestMessage(HttpMethod.Post, _apiHostName + authUrlPath);
            const string methodName = "GetAuthenticationBearerTokenAsync";
            var now = DateTime.Now.ToString("G");
            try
            {
                var dictionary = new Dictionary<string, string>
                {
                    {"username", SecurityProvider.SecureApiUsername},
                    {"password", SecurityProvider.SecureApiPassword},
                    {"grant_type", "password"}
                };
                request.Content = new FormUrlEncodedContent(dictionary);
                var result = await client.SendAsync(request).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode)
                {
                    return null;
                }
                var jsonString = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jObj = JsonObject.Parse(jsonString);
                var token = jObj.Get<string>(AccessToken);
                if (string.IsNullOrWhiteSpace(token) || !cs.AppIsInDebugMode)
                {
                    return token;
                }
                _logger.Value.Info($"Successfully retrieved an authentication bearer token from method {methodName} on {now}.");
                return token;
            }
            catch (Exception ex)
            {
                _logger.Value.Info($"Did not successfully retrieve an Authentication bearer token from method {methodName} on {now}.");
                _logger.Value.Error(ex);
                return string.Empty;
            }
            finally
            {
                client.Dispose();
                request.Dispose();
            }
        }

        public async Task<bool> CallSignalRApiMethod(SignalRMethodType type, string token, DocumentDto dto, int documentId)
        {
            const string methodName = "CallSignalRApiMethod";
            var now = DateTime.Now.ToString(LoggingService.TimeFormat);
            if (cs.AppIsInDebugMode)
                _logger.Value.Info($"Now entering the {methodName} method on {now}.");
            var req = new HttpRequestMessage();
            var client = new HttpClient();
            try
            {
                req.Method = HttpMethod.Post;
                req.RequestUri = new Uri($"{_apiHostName}{GetApiUrlPath(type, documentId)}");
                var bearerToken = $"Bearer {token}";
                req.Headers.TryAddWithoutValidation("Accept", "application/json");
                req.Headers.TryAddWithoutValidation("Authorization", bearerToken);
                if (type != SignalRMethodType.Delete)
                {
                    var jsonObj = dto.ToJson();
                    var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    req.Content = content;
                }
                var result = await client.SendAsync(req).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode)
                    return false;
                var jsonString = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jObj = JsonObject.Parse(jsonString);
                var message = jObj.Get<string>(Message);
                if (cs.AppIsInDebugMode)
                    _logger.Value.Info(message);
                return !string.IsNullOrWhiteSpace(message) && !message.ToLower().Contains("error");
            }
            catch (Exception ex)
            {
                if (cs.AppIsInDebugMode)
                    _logger.Value.Info($"Did not successfully retrieve an Authentication bearer token from method {methodName} on {now}.");
                _logger.Value.Error(ex);
                return false;
            }
            finally
            {
                req.Dispose();
                client.Dispose();
            }
        }

        private string GetApiUrlPath(SignalRMethodType type, int documentId)
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var now = DateTime.Now.ToString(LoggingService.TimeFormat);
            string apiUrlPath;
            switch (type)
            {
                case SignalRMethodType.Add:
                    apiUrlPath = cs.GetAppSetting(c.SignalRAddMethodApiUrlKey);
                    break;
                case SignalRMethodType.Modify:
                    apiUrlPath = cs.GetAppSetting(c.SignalRModifyMethodApiUrlKey);
                    break;
                case SignalRMethodType.Delete:
                    if (default(int) == documentId)
                        throw new Exception($"Error. The Document Id could not be found in the database from document Id {documentId} " +
                                            $"within {methodName} method on {now}.");
                    apiUrlPath = cs.GetAppSetting(c.SignalRDeleteMethodApiUrlKey) + documentId;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return apiUrlPath;
        }
    }
}