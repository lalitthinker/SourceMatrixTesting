using System.ComponentModel.DataAnnotations;

namespace IdentityApi.Models.NonProfit
{
    public class RegisterNonProfitRequestModel
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

        public string Description { get; init; }
        public string ProfilePictureUrl { get; init; }

        [Required]
        public long FoundationId { get; set; }
        [Required]
        public string FoundationName { get; set; }
    }
}
