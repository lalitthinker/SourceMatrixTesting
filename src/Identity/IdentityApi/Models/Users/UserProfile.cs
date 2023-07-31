using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using static IdentityApi.Extensions.ExcelExtensions;

namespace IdentityApi.Models
{
    public class UserProfile
    {
        public UserProfile()
        {
            RoleName = new List<RequestAllUsersRolesModel>();
        }
        [EpplusIgnore]
        public string Id { get; set; }
        public string FullName { get; set; }
        [EpplusIgnore]
        public string FirstName { get; set; }
        [EpplusIgnore]
        public string LastName { get; set; }
        public string UserName { get; set; }

        [EpplusIgnore]
        [DisplayName("CoverUrl")]
        public string CoverPictureUrl { get; set; }

        [EpplusIgnore]
        [DisplayName("ProfileUrl")]
        public string ProfilePictureUrl { get; set; }

        [DisplayName("Ph.No.")]
        public string PhoneNumber { get; set; }

        public string SaleQuota { get; set; }

        [DisplayName("Purch.Quota")]
        public string PurchaseQuota { get; set; }

        [EpplusIgnore]
        public string Title { get; set; }

        [EpplusIgnore]
        public string City { get; set; }
        public string Email { get; set; }
        public string TimeZone { get; set; }

        [NotMapped]
        public string Roles { get; set; }

        [EpplusIgnore]
        public string DeviceToken { get; set; }

        [EpplusIgnore]
        public List<RequestAllUsersRolesModel> RoleName { get; set; }

        [DisplayName("Desc.")]
        public string Description { get; set; }

        [DisplayName("Created Dt.")]
        public string CreatedDate { get; set; }

        [EpplusIgnore]
        public string DirectPhoneNumber { get; set; }
        [EpplusIgnore]
        public string NumberExtension { get; set; }
        [EpplusIgnore]
        public bool IsExpandable { get; set; }

        [EpplusIgnore]
        public bool IsChecked { get; set; }
        public bool UserStatus { get; set; }

        [DisplayName("Emerg.ContactName")]
        public string EmergencyContactName { get; set; }

        [DisplayName("Emergency No")]
        public string EmergencyContactNumber { get; set; }

        [DisplayName("OfficePh.No.")]
        public string OfficePhoneNumber { get; set; }

        [EpplusIgnore]
        [NotMapped]
        [DisplayName("ProfileThumbUrl")]
        public string ProfilePictureThumbUrl { get; set; }

        [EpplusIgnore]
        [DisplayName("CoverThumbUrl")]
        public string CoverPictureThumbUrl { get; set; }
        [EpplusIgnore]
        //Tauqir (22-05-2023)
        public int ImageZoomRatio { get; set; }
        [EpplusIgnore]
        [NotMapped]
        public string SalesCommissionRate { get; set; } = string.Empty;
        [EpplusIgnore]
        [NotMapped]
        public string PurchaseCommissionRate { get; set; } = string.Empty;
        [EpplusIgnore]
        [NotMapped]
        public string SalesCommissionRateName { get; set; } = string.Empty;
        [EpplusIgnore]
        [NotMapped]
        public string PurchaseCommissionRateName { get; set; } = string.Empty;
        [EpplusIgnore]
        [NotMapped]
        public int TotalRecords { get; set; }

    }
}
