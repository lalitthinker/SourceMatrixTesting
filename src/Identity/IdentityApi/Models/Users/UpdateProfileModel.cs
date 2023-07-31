namespace IdentityApi.Models.Users
{
    public class UpdateProfileModel
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool IsChangedCoverPictureName { get; set; }
        public string CoverPictureName { get; set; }

        public bool IsChangedProfilePictureName { get; set; }
        public string ProfilePictureName { get; set; }

        public string Description { get; set; }
    }
}
