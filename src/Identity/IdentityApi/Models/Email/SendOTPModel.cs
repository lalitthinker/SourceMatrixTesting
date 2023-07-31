using System.ComponentModel.DataAnnotations;

namespace IdentityApi.Models.Email
{
    public class SendOTPModel
    {
        [Required]
        [EmailAddress]
        public string To { get; init; }

        [Required]
        public string Subject { get; init; }

        public int OTPDigitNumber { get; set; }
    }
}
