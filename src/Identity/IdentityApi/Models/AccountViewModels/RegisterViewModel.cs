using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityApi.Models.AccountViewModels
{
    public record RegisterViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        [DataType(DataType.Text)]
        public string FirstName { get; init; }

        [Required]
        [Display(Name = "Last Name")]
        [DataType(DataType.Text)]
        public string LastName { get; init; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; init; }

        //[StringLength(10, ErrorMessage = "The {0} must be {1} characters long.", MinimumLength = 10)]
        //[Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; init; }

        public string ProfilePictureFile { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Username")]
        public string Username { get; init; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; init; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; init; }

        //[Display(Name = "User Address")]
        //public UserAddress UserAddress { get; init; }
    }

    public record UserAddress
    {
        public string Street { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public string Country { get; init; }
        public string ZipCode { get; init; }
    }
}
