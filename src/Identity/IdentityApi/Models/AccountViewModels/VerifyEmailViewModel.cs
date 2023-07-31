using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApi.Models.AccountViewModels
{
    public record VerifyEmailViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }
    }
}
