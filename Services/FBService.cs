using OAthLib.Models.FB;
using OAthLib.Services.Helpers;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace OAthLib.Services
{
    public class FBService
    {
        private HttpClient Client { get; set; }
        private ConfigService _config { get; }

        public FBService(HttpClient client, ConfigService config)
        {
            client.BaseAddress = new Uri("https://graph.facebook.com/v6.0");
            Client = client;
            _config = config;
        }
        /// <summary>
        /// 取得FB 登入頁面網址
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessUrl()
        {
            await Task.Yield();
            var fbConfig = _config.GetFBConfig();
            string AccessUrl = $"https://www.facebook.com/v6.0/dialog/oauth?client_id={fbConfig.ClientID}&redirect_uri={fbConfig.RedirectUrl}&state={Guid.NewGuid()}";
            
            return (AccessUrl);

        }
        /// <summary>
        /// 取得FB的AccessToken資料
        /// </summary>
        /// <param name="Code">授權碼</param>
        /// <returns></returns>
        public async Task<FBAccessToken> GetAccessToken(string Code)
        {
            var fbConfigData = _config.GetFBConfig();
            var url = $"/oauth/access_token?client_id={fbConfigData.ClientID}&redirect_uri={fbConfigData.RedirectUrl}&client_secret={fbConfigData.ClientSecret}&code={Code}"; ;
            var req = await Client.GetAsync(url);
            try
            {
                var bb = req.IsSuccessStatusCode;
                var data = await req.Content.ReadAsStringAsync();
                var resData =  JsonSerializer.Deserialize<FBAccessToken>(data);
                return resData;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 取得FB使用者的Profile
        /// </summary>
        /// <param name="fBAccessToken"></param>
        /// <returns></returns>
        public async Task<FBUserProfile> GetProfile(FBAccessToken fBAccessToken)
        {
            var url = $"/me?access_token={fBAccessToken.access_token}";
            var req = await Client.GetAsync(url);
            try
            {
                var bb = req.IsSuccessStatusCode;
                var data = await req.Content.ReadAsStringAsync();
                var resData =  JsonSerializer.Deserialize<FBUserProfile>(data);
                return resData;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
        // /// <summary>
        // /// 取得FB應用資訊
        // /// </summary>
        // /// <param name="fBAccessToken"></param>
        // /// <returns></returns>
        // public async Task<FBApplicationInfo> GetAppicationInfo(FBAccessToken fBAccessToken){
        //     var fbConfig=_config.GetFBConfig();
        //     var url = $"/{fbConfig.ClientID}?access_token={fBAccessToken.access_token}";
        //     var req = await Client.GetAsync(url);
        //     try
        //     {
        //         var bb = req.IsSuccessStatusCode;
        //         var data = await req.Content.ReadAsStringAsync();
        //          var resData =  JsonSerializer.Deserialize<FBApplicationInfo>(data);
        //         return resData;
        //     }
        //     catch (System.Exception e)
        //     {
        //         throw e;
        //     }
        // }
    }
}