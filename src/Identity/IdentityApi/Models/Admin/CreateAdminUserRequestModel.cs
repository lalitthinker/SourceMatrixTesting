using System.ComponentModel.DataAnnotations;

namespace IdentityApi.Models.Admin
{
    public class CreateAdminUserRequestModel
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }

        [EmailAddress]
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }

        public string ConfirmPassword { get; init; }
        public string Title { get; set; }
        public string SuiteApt { get; set; }
        public List<string> RoleName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string SaleCommissionRate { get; set; }
        public bool IsProfitBasedCommission { get; set; }
        public string PurchaseQuota { get; set; }
        public string SaleQuota { get; set; }
        public string PurchaseCommissionRate { get; set; }
        public string ProfilePictureFile { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        //public DateTime Date { get; set; }
        //Tauqir (22-05-2023)
        public int ImageZoomRatio { get; set; }
        public string DirectPhoneNumber { get; set; }
        public string NumberExtension { get; set; }
    }
}
