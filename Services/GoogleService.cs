using System.Net.Http;
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
            client.BaseAddress = new System.Uri("");
            Client = client;
        }
    }
}