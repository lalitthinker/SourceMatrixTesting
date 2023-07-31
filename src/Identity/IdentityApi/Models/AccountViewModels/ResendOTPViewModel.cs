using System.ComponentModel.DataAnnotations;

namespace IdentityApi.Models.AccountViewModels
{
    public class ResendOTPViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }
    }
}
