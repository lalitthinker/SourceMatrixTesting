using System.Text.RegularExpressions;

namespace IdentityApi.Helper
{
    public class ValidationWithRegEx
    {
        public static bool ValidatePassword(string Password)
        {
            // Create a pattern for a password with special characters ! * @ # $ % ^ & + =
            string pattern = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";
            // Create a Regex  
            Regex rg = new(pattern);
            return rg.IsMatch(Password);
        }
    }
}
