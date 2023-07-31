using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityApi.Models.Users
{
    public class RegisterUser
    {
        public RegisterUser()
        {
            Roles = new List<string>();
        }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string SuiteApt { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ProfilePictureUrl { get; set; }
        public List<string> Roles { get; set; }
    }
}
