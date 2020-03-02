using System.Net.Http;
using System.Threading.Tasks;
using OAthLib.Services.Helpers;

namespace OAthLib.Services
{
    public class MSService
    {
        private HttpClient Client { get; set; }
        private ConfigService _config { get; }
        public MSService(HttpClient client, ConfigService config)
        {
            _config = config;
            // client.BaseAddress = new Uri("https://api.line.me");
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
            string AccessUrl = $"https://login.microsoftonline.com/0b991457-5fbb-4426-b1e6-c625d63747c1/oauth2/v2.0/authorize";
            AccessUrl +="?response_type=code";
            AccessUrl += $"&client_id={lineConfig.ClientID}";
            AccessUrl += $"&redirect_uri={lineConfig.RedirectUrl}";
            AccessUrl += "&scope=email openid profile";
            AccessUrl += $"&state={Guid.NewGuid()}";
            return (AccessUrl);

        }
    }
}