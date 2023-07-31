using System;
using Microsoft.IdentityModel.Tokens;

namespace IdentityApi
{
    public class AppConstants
    {
        public static string ConnectionString { get; set; }
        public const string CorsPolicyName = "CorsPolicy";
        public static AuthToken AuthToken { get; set; } = new AuthToken();
        public static BaseUrl BaseUrl { get; set; } = new BaseUrl();
    }

    public class BaseUrl
    {
        public string OcelotGW { get; init; }

        public string IdentityAPI { get; init; }
        public string SourceMatrixAPI { get; init; }
        public string ResourceAPI { get; init; }
        public string PaymentAPI { get; init; }

        public string AdminWeb { get; init; }
    }

    public class AuthToken
    {
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public string Key { get; init; }

        public long Lifetime
        {
            get { return 15 * 24 * 60 * 60; } // days*hours*minutes*seconds
            //get { return 60 * 15; } // bearer lifetime 60 * 15 // 15 mins
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }

}
