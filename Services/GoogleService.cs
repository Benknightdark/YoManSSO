using System;
using System.Net.Http;
using System.Threading.Tasks;
using OAthLib.Services.Helpers;

namespace OAthLib.Services
{
    public class GoogleService
    {
        private HttpClient Client { get; set; }
        private ConfigService _config { get; }
        public GoogleService(HttpClient client, ConfigService config)
        {
            _config = config;
          //  client.BaseAddress = new System.Uri("");
            Client = client;
        }
         /// <summary>
        /// 取得 google 登入頁面網址
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessUrl()
        {
            await Task.Yield();
            var googleConfig =  _config.GetGoogleConfig();
            string AccessUrl = $"https://accounts.google.com/o/oauth2/v2/auth";
            AccessUrl +="?response_type=code";
            AccessUrl += $"&client_id={googleConfig.ClientID}";
            AccessUrl += $"&redirect_uri={googleConfig.RedirectUrl}";
            AccessUrl += "&scope=email openid";
            // AccessUrl += $"&state={Guid.NewGuid()}";
            return (AccessUrl);

        }
    }
}