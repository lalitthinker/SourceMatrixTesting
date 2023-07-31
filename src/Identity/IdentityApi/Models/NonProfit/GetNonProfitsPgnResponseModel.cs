using System;

namespace IdentityApi.Models.NonProfit
{
    public class GetNonProfitsPgnResponseModel
    {
        public string UserId { get; set; }

        public long FoundationId { get; set; }
        public string FoundationName { get; set; }

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

        public string UserRole { get; set; }
        public string AgeRange { get; set; }

        public string UserStatus { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public int TotalRecords { get; set; }
    }
}
