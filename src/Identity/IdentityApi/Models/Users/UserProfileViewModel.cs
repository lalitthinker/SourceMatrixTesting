using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityApi.Models.Users
{
    public class UserProfileViewModel
    {
        
        public string Id { get; set; }
        //public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string CoverPictureUrl { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string SaleQuota { get; set; }
        public string PurchaseQuota { get; set; }
        public string Email { get; set; }
        public string TimeZone { get; set; }
        public string DeviceToken { get; set; }
        public List<RequestAllUsersRolesModel> RoleName { get; set; }
        public string Description { get; set; }
        public string CreatedDate { get; set; }
        //public string StrCreatedDate { get; set; }
        //public bool IsExpandable { get; set; }
        public bool UserStatus { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        public bool IsChecked { get; set; }
    }
}
