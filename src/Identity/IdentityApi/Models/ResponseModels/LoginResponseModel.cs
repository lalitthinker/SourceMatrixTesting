using IdentityApi.Models.Users;

namespace IdentityApi.Models
{
    public record LoginResponseModel
    {
        public LoginResponseModel()
        {
            User = new UserVM();
        }
        public string AccessToken { get; set; }
        public string Scope { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public UserVM User { get; set; }
    }
}
