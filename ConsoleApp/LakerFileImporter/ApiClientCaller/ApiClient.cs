using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using ServiceStack.Text;
using cs = LakerFileImporter.ConfigService.ConfigService;
using c = LakerFileImporter.StringConstants.Constants;

namespace LakerFileImporter.ApiClientCaller
{
    internal class ApiClient
    {
        private readonly string _apiHostName = cs.GetAppSetting(c.FileUploadApiHostNameKey);
        private const string AccessToken = "access_token";

        internal async Task<string> GetAuthenticationBearerTokenAsync()
        {
            var authUrl = cs.GetAppSetting(c.AuthenticationApiUrlKey);
            var client = new HttpClient();
            var dictionary = new Dictionary<string, string>
            {
                {"username", cs.GetAppSetting(c.AuthenticationUserNameKey)},
                {"password", cs.GetAppSetting(c.AuthenticationPasswordKey)},
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


        internal async Task<bool> UploadFileToApiAsync(byte[] bytes, string fileName, string token)
        {
            var apiUrlPath = cs.GetAppSetting(c.FileUploadApiUrlKey);
            var bearerToken = "Bearer " + token;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", bearerToken);
            using (var content = new MultipartFormDataContent())
            {
                var fileContent = new ByteArrayContent(bytes);
                fileContent.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment") {FileName = fileName};
                content.Add(fileContent);
                var result = await client.PostAsync(_apiHostName + apiUrlPath, content);
                return result.IsSuccessStatusCode;
            }
        }
    }
}
