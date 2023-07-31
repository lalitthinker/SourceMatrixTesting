using System.ComponentModel.DataAnnotations;

namespace IdentityApi.Models.Email
{
    public class SendEmailModel
    {
        [Required]
        [EmailAddress]
        public string To { get; init; }

        [Required]
        public string Subject { get; init; }

        public string Body { get; init; }
    }
}
