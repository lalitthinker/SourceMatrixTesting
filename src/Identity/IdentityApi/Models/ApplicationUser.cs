using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Title { get; set; }
        public long? FoundationId { get; set; }
        public string FoundationName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public decimal SaleQuota { get; set; }
        public string SaleCommissionRate { get; set; }
        public bool IsProfitBasedCommission { get; set; }
        public decimal PurchaseQuota { get; set; }
        public string PurchaseCommissionRate { get; set; }
        public string SuiteApt { get; set; }
        public string TimeZone { get; set; } = "UTC";
        public string DeviceToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DirectPhoneNumber { get; set; }
        public string NumberExtension { get; set; }
        public int AgeRangeId { get; set; }
        [NotMapped]
        public AgeRange AgeRange
        {
            get { return (AgeRange)AgeRangeId; }
            set { AgeRangeId = (int)value; }
        }

        public string CoverPictureUrl { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string UserRole { get; set; }
        public string Description { get; set; }

        public int UserStatusId { get; set; }
        [NotMapped]
        public UserStatus UserStatus
        {
            get { return (UserStatus)UserStatusId; }
        }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime DeletedDate { get; set; }
        //New Extra Field
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        public string UserSetting { get; set; } = string.Empty;
        public int ImageZoomRatio { get; set; }
    }

    public enum UserStatus
    {
        [Description("Pending")]
        Pending = 0,

        [Description("Active")]
        Active = 1,

        [Description("InActive")]
        Inactive = 2,

        [Description("Rejected")]
        Rejected = 3,

        [Description("Suspended")]
        Suspended = 4,

        [Description("PasswordChangeRequired")]
        PasswordChangeRequired = 5,
    }

    public enum UserRole
    {
        [Description("Admin")]
        Normal = 0,

        [Description("Corporation")]
        Corporation = 1,

        [Description("Foundation")]
        Foundation = 2,

        [Description("Admin")]
        Admin = 3
    }
}
