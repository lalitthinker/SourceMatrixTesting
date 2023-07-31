using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace IdentityApi.Extensions
{
    public static class IdentityResultExtension
    {
        public static string GetErrorsAsString(this IdentityResult result)
        {
            return string.Join(", ", result.Errors.Select(e => e.Description).ToArray());
        }
    }
}
