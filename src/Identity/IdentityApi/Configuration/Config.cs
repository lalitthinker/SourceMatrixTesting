using IdentityModel;
using IdentityServer4;
using IdentityServer4.Test;

namespace IdentityApi.Configuration
{
    public class Config
    {
        // ApiResources define the apis in your system
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("IdentityAPI", "Identity API", new[] { JwtClaimTypes.Subject, JwtClaimTypes.Email, JwtClaimTypes.Id }),
                  new ApiResource("SourceMatrixAPI", "SourceMatrix API"),
                    new ApiResource("ResourceAPI", "Resource API")
            };
        }

        public static IEnumerable<ApiScope> GetApiScope()
        {
            return new List<ApiScope>
            {
                new ApiScope("IdentityAPI", "Identity API Scopes"),
                new ApiScope("SourceMatrixAPI", "SourceMatrix API Scopes"),
                  new ApiScope("ResourceAPI", "Resource API Scopes")
            };
        }

        // Identity resources are data like user ID, name, or email address of a user
        // see: http://docs.identityserver.io/en/release/configuration/resources.html
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static Dictionary<string, string> GetUrls(IConfiguration configuration)
        {
            var clientUrls = new Dictionary<string, string>();

            clientUrls.Add("IdentityAPI", AppConstants.BaseUrl.IdentityAPI);
            clientUrls.Add("SourceMatrixAPI", AppConstants.BaseUrl.SourceMatrixAPI);
            clientUrls.Add("ResourceAPI", AppConstants.BaseUrl.ResourceAPI);
            return clientUrls;
        }

        // client want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientsUrl)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "SourceMatrixClient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        "IdentityAPI",
                        "SourceMatrixAPI",
                        "ResourceAPI"
                    },
                    UpdateAccessTokenClaimsOnRefresh=true,
                    AccessTokenLifetime = Convert.ToInt32(AppConstants.AuthToken.Lifetime),
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }
    }
}