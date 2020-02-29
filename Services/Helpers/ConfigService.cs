using Microsoft.Extensions.Configuration;
using OAthLib.Models.FB;
using OAthLib.Models.Google;
using OAthLib.Models.Line;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OAthLib.Services.Helpers
{
    public class ConfigService
    {
        private IConfiguration _config { get; }
        public ConfigService(IConfiguration config)
        {
            _config = config;


        }
        public LineConfig GetLineConfig()
        {
            var appConfig = new LineConfig();
            _config.GetSection("Line").Bind(appConfig);
            return appConfig;


        }
        public FBConfig GetFBConfig()
        {
            var appConfig = new FBConfig();
            _config.GetSection("FB").Bind(appConfig);
            return appConfig;
        }
          public GoogleConfig GetGoogleConfig()
        {
            var appConfig = new GoogleConfig();
            _config.GetSection("Google").Bind(appConfig);
            return appConfig;
        }
    }
}
