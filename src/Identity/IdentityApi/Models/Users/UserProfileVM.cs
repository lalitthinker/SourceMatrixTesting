namespace IdentityApi.Models.Users
{
    public class UserProfileVM
    {
        public string Id { get; set; }

        public string FullName { get; set; }
        public string UserName { get; set; }

        public string CoverPictureUrl { get; set; }
        public string CoverPictureThumbUrl { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string ProfilePictureThumbUrl { get; set; }

        public string Email { get; set; }

        public string UserRole { get; set; }
        public string Description { get; set; }
    }

    public class ProfileResposeModel
    {
        public string profileImagePath { get; set; }
    }
}
