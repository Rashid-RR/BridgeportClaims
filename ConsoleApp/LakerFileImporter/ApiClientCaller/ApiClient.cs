using NLog;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using LakerFileImporter.Logging;
using LakerFileImporter.Security;
using ServiceStack.Text;
using cs = LakerFileImporter.ConfigService.ConfigService;
using c = LakerFileImporter.StringConstants.Constants;

namespace LakerFileImporter.ApiClientCaller
{
    internal class ApiClient
    {
        private readonly string _apiHostName = cs.GetAppSetting(c.FileUploadApiHostNameKey);
        private static readonly Logger Logger = LoggingService.Instance.Logger;
        private const string AccessToken = "access_token";
        private const string Message = "message";

        internal async Task<string> GetAuthenticationBearerTokenAsync()
        {
            var client = new HttpClient();
            var authUrl = cs.GetAppSetting(c.AuthenticationApiUrlKey);
            var request = new HttpRequestMessage(HttpMethod.Post, _apiHostName + authUrl);
            var methodName = MethodBase.GetCurrentMethod().Name;
            var now = DateTime.Now.ToString("G");
            try
            {
                var password = new CompiledSecurityProvider().RawBridgeportClaimsSiteUserPassword;
                var dictionary = new Dictionary<string, string>
                {
                    {"username", cs.GetAppSetting(c.AuthenticationUserNameKey)},
                    {"password", password},
                    {"grant_type", "password"}
                };
                request.Content = new FormUrlEncodedContent(dictionary);
                var result = await client.SendAsync(request).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode)
                    return null;
                var jsonString = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
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


        internal async Task<bool> UploadFileToApiAsync(byte[] bytes, string fileName, string token)
        {
            var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(bytes);
            var client = new HttpClient();
            try
            {
                var apiUrlPath = cs.GetAppSetting(c.FileUploadApiUrlKey);
                var bearerToken = $"Bearer {token}";
                client.DefaultRequestHeaders.Add("Authorization", bearerToken);
                fileContent.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment") {FileName = fileName};
                content.Add(fileContent);
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                var result = await client.PostAsync($"{_apiHostName}{apiUrlPath}", content).ConfigureAwait(false);
                var retVal = result.IsSuccessStatusCode;
                if (!cs.AppIsInDebugMode)
                {
                    return retVal;
                }
                var methodName = MethodBase.GetCurrentMethod().Name;
                var now = DateTime.Now.ToString("G");
                Logger.Info(retVal
                    ? $"Successfully uploaded file {fileName} from method {methodName} on {now}."
                    : $"Failed to upload file {fileName} to server from method {methodName} on {now}.");
                return retVal;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
            finally
            {
                content.Dispose();
                fileContent.Dispose();
                client.Dispose();
            }
        }

        internal async Task<bool> ProcessLakerFileToApiAsync(string token)
        {
            var apiUrlPath = cs.GetAppSetting(c.LakerFileProcessingApiUrlKey);
            var req = new HttpRequestMessage(HttpMethod.Post, $"{_apiHostName}{apiUrlPath}");

            var client = new HttpClient();
            try
            {
                var bearerToken = $"Bearer {token}";
                req.Headers.TryAddWithoutValidation("Accept", "application/json");
                req.Headers.TryAddWithoutValidation("Authorization", bearerToken);
                var result = await client.SendAsync(req).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode)
                {
                    return false;
                }
                var jsonString = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                var jObj = JsonObject.Parse(jsonString);
                var message = jObj.Get<string>(Message);
                if (cs.AppIsInDebugMode)
                {
                    Logger.Info(message);
                }
                return !string.IsNullOrWhiteSpace(message) && !message.ToLower().Contains("error");
            }
            catch (Exception ex)
            {
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
