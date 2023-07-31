using System;

namespace IdentityApi.Models.ResponseModels
{
    public record ForgotPasswordResponseModel
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public string Token { get; set; }
        public DateTime OTPSentAt { get; set; }
    }
}
