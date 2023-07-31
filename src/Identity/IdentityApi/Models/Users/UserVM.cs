using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityApi.Models.Users
{
    public class UserVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }      
        public string ProfilePictureUrl { get; set; }
        public string ProfilePictureThumbUrl { get; set; }
        public string Email { get; set; }
        public string TimeZone { get; set; }
        public string DeviceToken { get; set; }
        public string UserRole { get; set; }
        public string Description { get; set; }        
        public string UserStatus { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string EmergencyContactName { get; set; }

        public string EmergencyContactNumber { get; set; }
        public string OfficePhoneNumber{ get; set; }
    }
    public class UserListVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserStatus { get; set; }
        public string UserRole { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        [NotMapped]
        public int TotalRecords { get; set; }

    }
}
