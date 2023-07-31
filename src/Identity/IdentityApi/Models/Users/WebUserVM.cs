namespace IdentityApi.Models.Users
{
    public class WebUserVM
    {
        public WebUserVM()
        {
            RoleName = new List<string>();
        }


        public string UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }

        public string CoverPictureUrl { get; set; }
        public string CoverPictureThumbUrl { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string ProfilePictureThumbUrl { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }

        public List<string> UserRole { get; set; }
        public string AgeRange { get; set; }

        public string UserStatus { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }

        public string SuiteApt { get; set; }
        public List<string> RoleName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string SaleQuota { get; set; }
        public string SaleCommissionRate { get; set; }
        public bool IsProfitBasedCommission { get; set; }
        public string PurchaseQuota { get; set; }
        public string PurchaseCommissionRate { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        public int ImageZoomRatio { get; set; }
        public int TotalRecords { get; set; }
    }
}
