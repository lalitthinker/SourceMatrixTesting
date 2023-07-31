using System.ComponentModel.DataAnnotations;

namespace IdentityApi.Models.AccountViewModels
{
    public record ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }

        [Required]
        [Display(Name = "New password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; init; }

        [Required]
        [Display(Name = "Reset Password Token")]
        public string Token { get; init; }
    }
}
