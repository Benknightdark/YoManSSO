using OAthLib.Models.Line;
using OAthLib.Services.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
namespace OAthLib.Services
{
    public class LineService
    {

        private HttpClient Client { get; set; }
        private ConfigService _config { get; }
        public LineService(HttpClient client, ConfigService config)
        {
            _config = config;
            client.BaseAddress = new Uri("https://api.line.me");
            Client = client;
        }
        /// <summary>
        /// 取得LINE 登入頁面網址
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessUrl()
        {
            await Task.Yield();
            var lineConfig =  _config.GetLineConfig();
            string AccessUrl = $"https://access.line.me/oauth2/v2.1/authorize";
            AccessUrl +="?response_type=code";
            AccessUrl += $"&client_id={lineConfig.ClientID}";
            AccessUrl += $"&redirect_uri={lineConfig.RedirectUrl}";
            AccessUrl += "&scope=email openid profile";
            AccessUrl += $"&state={Guid.NewGuid()}";
            return (AccessUrl);

        }

        /// <summary>
        /// 取得LINE API的AccessToken資料
        /// </summary>
        /// <param name="Code">授權碼</param>
        /// <returns></returns>
        public async Task<LineAccessToken> GetAccessToken(string Code)
        {
            var lineConfig =  _config.GetLineConfig();

            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            nvc.Add(new KeyValuePair<string, string>("code", Code));
            nvc.Add(new KeyValuePair<string, string>("redirect_uri", lineConfig.RedirectUrl));
            nvc.Add(new KeyValuePair<string, string>("client_id", lineConfig.ClientID));
            nvc.Add(new KeyValuePair<string, string>("client_secret", lineConfig.ClientSecret));

            var req = await Client.PostAsync("/oauth2/v2.1/token", new FormUrlEncodedContent(nvc));
            try
            {
                var bb = req.IsSuccessStatusCode;
                var data = await req.Content.ReadAsStringAsync();
                var nData = Newtonsoft.Json.JsonConvert.DeserializeObject<LineAccessToken>(data);
                return nData;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 取得Line使用者個人資料
        /// </summary>
        /// <param name="lineAccessToken">AccessToken</param>
        /// <returns></returns>
        public async Task<LineUserProfile> GetProfile(LineAccessToken lineAccessToken)
        {
            Client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", lineAccessToken.access_token);
            var req = await Client.GetAsync("/v2/profile");
            try
            {
                var data = await req.Content.ReadAsStringAsync();
                var nData =  JsonSerializer.Deserialize<LineUserProfile>(data);
                return nData;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// 取得Line使用者Email
        /// </summary>
        /// <param name="lineAccessToken">AccessToken</param>
        /// <returns></returns>
        public async Task<string> GetEmail(LineAccessToken lineAccessToken)
        {
            try
            {
                await Task.Yield();
                JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
                var tokenS = hand.ReadJwtToken(lineAccessToken.id_token);
                string email = string.Empty;
                if (tokenS.Claims.ToList().Find(c => c.Type == "email") != null)
                    email = tokenS.Claims.First(c => c.Type == "email").Value;
                return email;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}