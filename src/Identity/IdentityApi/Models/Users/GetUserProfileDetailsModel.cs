using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityApi.Models.Users
{
    public class GetUserProfileDetailsModel
    {
        public GetUserProfileDetailsModel()
        {
            RoleName = new List<RequestAllUsersRolesModel>();
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public string FullName { get; set; }
        public string CoverPictureUrl { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string OfficePhoneNumber { get; set; }
        public string DirectPhoneNumber { get; set; }
        public string NumberExtension { get; set; }
        public string PhoneNumber { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string SalesCommissionRate { get; set; }
        public string PurchaseCommissionRate { get; set; }
        public string SaleQuota { get; set; }
        public string PurchaseQuota { get; set; }
        public List<RequestAllUsersRolesModel> RoleName { get; set; }
        [NotMapped]
        public string OfficePhoneNumberPlusExt { get; set; }
        [NotMapped]
        public string SaleCommissionRoleName { get; set; } = string.Empty;
        [NotMapped]
        public string PurchaseCommissionRoleName { get; set; } = string.Empty;
    }
}
