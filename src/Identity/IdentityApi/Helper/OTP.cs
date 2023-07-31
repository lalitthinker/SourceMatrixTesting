using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApi.Helper
{
    public static class OTP
    {
        public static string Generate6DigitOTP()
        {
            Random rnd = new();
            return (rnd.Next(100000, 999999)).ToString();
        }
        public static string Generate4DigitOTP()
        {
            Random rnd = new();
            return (rnd.Next(1000, 9999)).ToString();
        }
    }
}
