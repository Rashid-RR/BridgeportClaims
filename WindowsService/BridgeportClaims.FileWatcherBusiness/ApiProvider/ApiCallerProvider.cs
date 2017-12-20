using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BridgeportClaims.FileWatcherBusiness.DAL;
using BridgeportClaims.FileWatcherBusiness.Dto;
using BridgeportClaims.FileWatcherBusiness.Enums;
using BridgeportClaims.FileWatcherBusiness.Logging;
using BridgeportClaims.FileWatcherBusiness.Security;
using NLog;
using ServiceStack;
using ServiceStack.Text;
using cs = BridgeportClaims.FileWatcherBusiness.ConfigService.ConfigService;
using c = BridgeportClaims.FileWatcherBusiness.StringConstants.Constants;

namespace BridgeportClaims.FileWatcherBusiness.ApiProvider
{
    public class ApiCallerProvider
    {
        private readonly string _apiHostName = cs.GetAppSetting(c.ApiHostNameKey);
        private static readonly Logger Logger = LoggingService.Instance.Logger;
        private readonly ImageDataProvider _imageDataProvider;
        private const string AccessToken = "access_token";
        private const string Message = "message";

        public ApiCallerProvider()
        {
            _imageDataProvider = new ImageDataProvider();
        }

        internal async Task<string> GetAuthenticationBearerTokenAsync()
        {
            var client = new HttpClient();
            var authUrlPath = cs.GetAppSetting(c.AuthenticationApiUrlKey);
            var request = new HttpRequestMessage(HttpMethod.Post, _apiHostName + authUrlPath);
            var methodName = MethodBase.GetCurrentMethod().Name;
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
                var result = await client.SendAsync(request);
                if (!result.IsSuccessStatusCode)
                    return null;
                var jsonString = await result.Content.ReadAsStringAsync();
                var jObj = JsonObject.Parse(jsonString);
                var token = jObj.Get<string>(AccessToken);
                if (string.IsNullOrWhiteSpace(token) || !cs.AppIsInDebugMode) return token;

                Logger.Info($"Successfully retrieved an authentication bearer token from method {methodName} on {now}.");
                return token;
            }
            catch (Exception ex)
            {
                Logger.Info($"Did not successfully retreive an Authentication bearer token from method {methodName} on {now}.");
                Logger.Error(ex);
                throw;
            }
            finally
            {
                client.Dispose();
                request.Dispose();
            }
        }

        internal async Task<bool> CallSignalRApiMethod(SignalRMethodType type, string token, DocumentDto dto)
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var now = DateTime.Now.ToString("G");
            var req = new HttpRequestMessage();
            var client = new HttpClient();
            try
            {
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
                        var docId = _imageDataProvider.GetDocumentIdByDocumentName(dto.FileName);
                        if (default(int) == docId)
                            throw new Exception($"Error. The Document Id could not be found in the database from FileName {dto.FileName}");
                        apiUrlPath = cs.GetAppSetting(c.SignalRAddMethodApiUrlKey) + docId;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
                req.Method = HttpMethod.Post;
                req.RequestUri = new Uri($"{_apiHostName}{apiUrlPath}");
                var bearerToken = $"Bearer {token}";
                req.Headers.TryAddWithoutValidation("Accept", "application/json");
                req.Headers.TryAddWithoutValidation("Authorization", bearerToken);
                if (type != SignalRMethodType.Delete)
                {
                    var jsonObj = dto.ToJson();
                    var content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                    req.Content = content;
                }
                var result = await client.SendAsync(req);
                if (!result.IsSuccessStatusCode)
                    return false;
                var jsonString = await result.Content.ReadAsStringAsync();
                var jObj = JsonObject.Parse(jsonString);
                var message = jObj.Get<string>(Message);
                if (cs.AppIsInDebugMode)
                    Logger.Info(message);
                return !string.IsNullOrWhiteSpace(message) && !message.ToLower().Contains("error");
            }
            catch (Exception ex)
            {
                if (cs.AppIsInDebugMode)
                    Logger.Info($"Did not successfully retreive an Authentication bearer token from method {methodName} on {now}.");
                Logger.Error(ex);
                throw;
            }
            finally
            {
                req.Dispose();
                client.Dispose();
            }
        }
    }
}