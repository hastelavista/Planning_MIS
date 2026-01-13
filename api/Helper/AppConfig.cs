using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class AppConfig
    {
        private readonly IConfiguration _config;
        public AppConfig(IConfiguration config)
        {
            _config = config;
        }

        public string GetValue(string key)
        {
            return _config[$"AppSettings:{key}"];
        }

        //documentAPI
        public string DocumentApiUrl => _config["AppSettings:DocumentAPI"];

        //JWTKey
        public string JwtKey => _config["Jwt:Key"];
        


        // IdentityServer Configuration
        public bool IsIdentityServerEnabled => Convert.ToBoolean(_config["IdentityServer:Enabled"]);
        public string IdentityServerURL => _config["IdentityServer:Endpoint"];
        public string IdentityServerClientID => _config["IdentityServer:ClientID"];
    }
}
