using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using LakerFileImporter.Security;
using NLog;
using ServiceStack.Text;
using cs = LakerFileImporter.ConfigService.ConfigService;
using c = LakerFileImporter.StringConstants.Constants;

namespace LakerFileImporter.ApiClientCaller
{
    internal class ApiClient
    {
        private readonly string _apiHostName = cs.GetAppSetting(c.FileUploadApiHostNameKey);
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string AccessToken = "access_token";
        private const string Message = "message";

        internal async Task<string> GetAuthenticationBearerTokenAsync()
        {
            try
            {
                var authUrl = cs.GetAppSetting(c.AuthenticationApiUrlKey);
                var client = new HttpClient();
                var provider = new SensitiveStringsProvider();
                var password = provider.GetAuthenticatedPassword().ToUnsecureString();
                var dictionary = new Dictionary<string, string>
                {
                    {"username", cs.GetAppSetting(c.AuthenticationUserNameKey)},
                    {"password", password},
                    {"grant_type", "password"}
                };
                var request =
                    new HttpRequestMessage(HttpMethod.Post, _apiHostName + authUrl)
                    {
                        Content = new FormUrlEncodedContent(dictionary)
                    };
                var result = await client.SendAsync(request);
                if (!result.IsSuccessStatusCode)
                    return null;
                var jsonString = await result.Content.ReadAsStringAsync();
                var jObj = JsonObject.Parse(jsonString);
                var token = jObj.Get<string>(AccessToken);
                return token;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }


        internal async Task<bool> UploadFileToApiAsync(byte[] bytes, string fileName, string token)
        {
            var content = new MultipartFormDataContent();
            try
            {
                var apiUrlPath = cs.GetAppSetting(c.FileUploadApiUrlKey);
                var bearerToken = $"Bearer {token}";
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", bearerToken);
                var fileContent = new ByteArrayContent(bytes);
                fileContent.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment") {FileName = fileName};
                content.Add(fileContent);
                var result = await client.PostAsync($"{_apiHostName}{apiUrlPath}", content);
                return result.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
            finally
            {
                content.Dispose();
            }
        }

        internal async Task<bool> ProcessLakerFileToApiAsync(string newLakerFileName, string token)
        {
            try
            {
                var bearerToken = $"Bearer {token}";
                var apiUrlPath = cs.GetAppSetting(c.LakerFileProcessingApiUrlKey);
                var client = new HttpClient();

                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                client.DefaultRequestHeaders.Add("Authorization", bearerToken);
                var result = await client.PostAsync($"{_apiHostName}{apiUrlPath}", null);
                if (!result.IsSuccessStatusCode)
                    return false;
                var jsonString = await result.Content.ReadAsStringAsync();
                var jObj = JsonObject.Parse(jsonString);
                var message = jObj.Get<string>(Message);
                Logger.Info(message);
                return !string.IsNullOrWhiteSpace(message) && !message.ToLower().Contains("error");
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }
    }
}
