using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OAthLib.Models.LinkedIn;
using OAthLib.Services.Helpers;

namespace OAthLib.Services
{
    public class LinkedInService
    {
         private HttpClient Client { get; set; }
        private ConfigService _config { get; }
        public LinkedInService (HttpClient client, ConfigService config) {
            _config = config;
            // client.BaseAddress = new System.Uri("https://oauth2.googleapis.com");
            Client = client;
        }
        /// <summary>
        /// 取得 LinkedIn 登入頁面網址
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessUrl () {
            await Task.Yield ();
            var linkedInConfig = _config.GetLinkedInConfig ();
            string AccessUrl = $"https://www.linkedin.com/oauth/v2/authorization";
            AccessUrl += "?response_type=code";
            AccessUrl += $"&client_id={linkedInConfig.ClientID}";
            AccessUrl += $"&redirect_uri={linkedInConfig.RedirectUrl}";
            AccessUrl += "&scope=r_liteprofile r_emailaddress w_member_social";
            // AccessUrl += $"&state={Guid.NewGuid()}";
            return (AccessUrl);

        }
        /// <summary>
        /// 取得LinkedIn的AccessToken資料
        /// </summary>
        /// <param name="Code">授權碼</param>
        /// <returns></returns>
        public async Task<LinkedInAccessToken> GetAccessToken (string Code) {
            var linkedInConfigData = _config.GetLinkedInConfig ();

            var nvc = new List<KeyValuePair<string, string>> ();
            nvc.Add (new KeyValuePair<string, string> ("grant_type", "authorization_code"));
            nvc.Add (new KeyValuePair<string, string> ("code", Code));
            nvc.Add (new KeyValuePair<string, string> ("redirect_uri", linkedInConfigData.RedirectUrl));
            nvc.Add (new KeyValuePair<string, string> ("client_id", linkedInConfigData.ClientID));
            nvc.Add (new KeyValuePair<string, string> ("client_secret", linkedInConfigData.ClientSecret));

            var req = await Client.PostAsync ("https://www.linkedin.com/oauth/v2/accessToken", new FormUrlEncodedContent (nvc));
            try {
                var data = await req.Content.ReadAsStringAsync ();
                var resData = JsonSerializer.Deserialize<LinkedInAccessToken> (data);
                return resData;
            } catch (System.Exception e) {
                throw e;
            }
        }

        // /// <summary>
        // /// 取得Google 使用者profile
        // /// </summary>
        // /// <param name="lineAccessToken">AccessToken</param>
        // /// <returns></returns>
        // public async Task<GoogleUserProfile> GetUserProfile (GoogleAccessToken googleAccessToken) {
        //     try {
        //         var req = await Client.GetAsync ($"https://www.googleapis.com/oauth2/v1/userinfo?access_token={googleAccessToken.access_token}");
        //         var data = await req.Content.ReadAsStringAsync ();
        //         var resData =  JsonSerializer.Deserialize<GoogleUserProfile> (data);
        //         return resData;

        //     } catch (Exception e) {
        //         throw e;
        //     }
        // }
    }
}