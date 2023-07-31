namespace IdentityApi.Models.Users
{
    public class GetAllUserModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CoverPictureUrl { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TimeZone { get; set; }
        public string DeviceToken { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
