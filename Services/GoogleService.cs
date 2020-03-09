using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using OAthLib.Models.Google;
using OAthLib.Services.Helpers;

namespace OAthLib.Services {
    public class GoogleService {
        private HttpClient Client { get; set; }
        private ConfigService _config { get; }
        public GoogleService (HttpClient client, ConfigService config) {
            _config = config;
            // client.BaseAddress = new System.Uri("https://oauth2.googleapis.com");
            Client = client;
        }
        /// <summary>
        /// 取得 google 登入頁面網址
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessUrl () {
            await Task.Yield ();
            var googleConfig = _config.GetGoogleConfig ();
            string AccessUrl = $"https://accounts.google.com/o/oauth2/v2/auth";
            AccessUrl += "?response_type=code";
            AccessUrl += $"&client_id={googleConfig.ClientID}";
            AccessUrl += $"&redirect_uri={googleConfig.RedirectUrl}";
            AccessUrl += "&scope=email openid profile";
            // AccessUrl += $"&state={Guid.NewGuid()}";
            return (AccessUrl);

        }
        /// <summary>
        /// 取得Google的AccessToken資料
        /// </summary>
        /// <param name="Code">授權碼</param>
        /// <returns></returns>
        public async Task<GoogleAccessToken> GetAccessToken (string Code) {
            var googleConfigData = _config.GetGoogleConfig ();

            var nvc = new List<KeyValuePair<string, string>> ();
            nvc.Add (new KeyValuePair<string, string> ("grant_type", "authorization_code"));
            nvc.Add (new KeyValuePair<string, string> ("code", Code));
            nvc.Add (new KeyValuePair<string, string> ("redirect_uri", googleConfigData.RedirectUrl));
            nvc.Add (new KeyValuePair<string, string> ("client_id", googleConfigData.ClientID));
            nvc.Add (new KeyValuePair<string, string> ("client_secret", googleConfigData.ClientSecret));

            var req = await Client.PostAsync ("https://oauth2.googleapis.com/token", new FormUrlEncodedContent (nvc));
            try {
                var data = await req.Content.ReadAsStringAsync ();
                var resData = JsonSerializer.Deserialize<GoogleAccessToken> (data);
                return resData;
            } catch (System.Exception e) {
                throw e;
            }
        }

        /// <summary>
        /// 取得Google 使用者profile
        /// </summary>
        /// <param name="googleAccessToken">AccessToken</param>
        /// <returns></returns>
        public async Task<GoogleUserProfile> GetProfile (GoogleAccessToken googleAccessToken) {
            try {
                var req = await Client.GetAsync ($"https://www.googleapis.com/oauth2/v1/userinfo?access_token={googleAccessToken.access_token}");
                var data = await req.Content.ReadAsStringAsync ();
                var resData =  JsonSerializer.Deserialize<GoogleUserProfile> (data);
                return resData;

            } catch (Exception e) {
                throw e;
            }
        }

        // public async Task<string> GetApplicationInfo (GoogleAccessToken googleAccessToken) {
        //     try {

        //         // var req = await Client.GetAsync ($"https://www.googleapis.com/oauth2/v1/userinfo?access_token={googleAccessToken.access_token}");
        //         // var data = await req.Content.ReadAsStringAsync ();
        //         // var resData =  JsonSerializer.Deserialize<GoogleUserProfile> (data);
        //         // return resData;

        //     } catch (Exception e) {
        //         throw e;
        //     }
        // }
    }
}