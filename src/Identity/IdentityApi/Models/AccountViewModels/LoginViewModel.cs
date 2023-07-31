using System.ComponentModel.DataAnnotations;

namespace IdentityApi.Models.AccountViewModels
{
    public record LoginViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string TimeZone { get; set; }
        public string DeviceToken { get; set; }
    }
}