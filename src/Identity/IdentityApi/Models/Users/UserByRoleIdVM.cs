using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityApi.Models.Users
{
    public class UserByRoleIdVM
    {
        public UserByRoleIdVM()
        {
            RoleName = new List<RequestAllUsersRolesModel>();
        }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        [NotMapped]
        public string FullName { get; set; }
        public string CoverPictureUrl { get; set; }
        [NotMapped]
        public string CoverPictureThumbUrl { get; set; }
        public string ProfilePictureUrl { get; set; }
        [NotMapped]
        public string ProfilePictureThumbUrl { get; set; }
        public string Email { get; set; }
        public string TimeZone { get; set; }
        public string DeviceToken { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public List<RequestAllUsersRolesModel> RoleName { get; set; }
        public string Description { get; set; }
        public string CreatedDate { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        public string PurchaseQuota { get; set; }
        public string SaleQuota { get; set; }
        public bool UserStatus { get; set; }
        public string SalesCommissionRate { get; set; } = string.Empty;
        public string PurchaseCommissionRate { get; set; } = string.Empty;
        public string DirectPhoneNumber { get; set; }
        public string NumberExtension { get; set; }
        //[NotMapped, JsonIgnore]
        //public UserStatues UserStatus1
        //{
        //    get { return (UserStatues)UserStatusId; }
        //}
        public int TotalRecords { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDeleted { get; set; }
        //Tauqir (22-05-2023)
        public int ImageZoomRatio { get; set; }
    }
}
