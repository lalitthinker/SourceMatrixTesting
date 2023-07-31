using System;

namespace IdentityApi.Models.ResponseModels
{
    public record SendOTPResponseModel
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public DateTime OTPSentAt { get; set; }
    }
}
