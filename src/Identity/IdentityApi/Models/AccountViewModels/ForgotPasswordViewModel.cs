using System.ComponentModel.DataAnnotations;

namespace IdentityApi.Models.AccountViewModels
{
    public record ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }
        [Required]
        public string NewPassword { get; init; }
        public string ConfirmNewPassword { get; init; }
        public bool ThroughEmail { get; init; }

    }
}