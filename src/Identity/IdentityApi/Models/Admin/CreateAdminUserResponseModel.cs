namespace IdentityApi.Models.Admin
{
    public class CreateAdminUserResponseModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public decimal SaleQuota { get; set; }
        public string SaleCommissionRate { get; set; }
        public string DirectPhoneNumber { get; set; }
        public string NumberExtension { get; set; }
        public bool IsProfitBasedCommission { get; set; }
        public decimal PurchaseQuota { get; set; }
        public string PurchaseCommissionRate { get; set; }
        public string SuiteApt { get; set; }
        public bool UserStatus { get; set; }
        public string UserRole { get; set; }
        public string ProfilePictureFile { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        public string CoverPictureUrl { get; set; }
        public string CreatedDate { get; set; }
        public bool IsChecked { get; set; }
        public bool IsExpandable { get; set; }
        //Tauqir (22-05-2023)
        public int ImageZoomRatio { get; set; }
        public List<RequestAllUsersRolesModel> RoleName { get; set; }


    }
}
