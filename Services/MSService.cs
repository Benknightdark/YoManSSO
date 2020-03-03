using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using OAthLib.Models.MS;
using OAthLib.Services.Helpers;

namespace OAthLib.Services {
    public class MSService {
        private HttpClient Client { get; set; }
        private ConfigService _config { get; }
        public MSService (HttpClient client, ConfigService config) {
            _config = config;

            Client = client;
        }
        /// <summary>
        /// 取得MS 登入頁面網址
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessUrl () {
            await Task.Yield ();
            var msConfig = _config.GetMSConfig ();
            string AccessUrl = $"https://login.microsoftonline.com/{msConfig.TenantID}/oauth2/v2.0/authorize";
            AccessUrl += "?response_type=code";
            AccessUrl += $"&client_id={msConfig.ClientID}";
            AccessUrl += $"&redirect_uri={msConfig.RedirectUrl}";
            AccessUrl += $"&response_mode=query";
            AccessUrl += "&scope=openid offline_access https://graph.microsoft.com/user.read";
            AccessUrl += $"&state={Guid.NewGuid()}";
            AccessUrl += $"&nonce={Guid.NewGuid()}";
            return (AccessUrl);

        }
        /// <summary>
        /// 取得MS的AccessToken資料
        /// </summary>
        /// <param name="Code">授權碼</param>
        /// <returns></returns>
        public async Task<MSAccessToken> GetAccessToken (string Code) {
            var msConfigData = _config.GetMSConfig ();
            var nvc = new List<KeyValuePair<string, string>> ();
            nvc.Add (new KeyValuePair<string, string> ("grant_type", "authorization_code"));
            nvc.Add (new KeyValuePair<string, string> ("code", Code));
            nvc.Add (new KeyValuePair<string, string> ("redirect_uri", msConfigData.RedirectUrl));
            nvc.Add (new KeyValuePair<string, string> ("client_id", msConfigData.ClientID));
            nvc.Add (new KeyValuePair<string, string> ("scope", "openid offline_access https://graph.microsoft.com/user.read"));
            nvc.Add (new KeyValuePair<string, string> ("client_secret", msConfigData.ClientSecret));
            var url = $"https://login.microsoftonline.com/{msConfigData.TenantID}/oauth2/v2.0/token";;
            var req = await Client.PostAsync (url, new FormUrlEncodedContent (nvc));
            try {
                var bb = req.IsSuccessStatusCode;
                var data = await req.Content.ReadAsStringAsync ();
                var resData = JsonSerializer.Deserialize<MSAccessToken> (data);
                return resData;
            } catch (System.Exception e) {
                throw e;
            }
        }

        /// <summary>
        /// 取得MS使用者的Profile
        /// </summary>
        /// <param name="msAccessToken"></param>
        /// <returns></returns>
        public async Task<MSUserProfile> GetProfile (MSAccessToken msAccessToken) {
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue ("Bearer", msAccessToken.access_token);
            var req = await Client.GetAsync ($"https://graph.microsoft.com/v1.0/me ");
            try {
                var bb = req.IsSuccessStatusCode;
                var data = await req.Content.ReadAsStringAsync ();
                var resData = JsonSerializer.Deserialize<MSUserProfile> (data);
                return resData;
            } catch (System.Exception e) {
                throw e;
            }
        }

    }
}