namespace IdentityApi.Services.Accounts
{
    public interface IAccountService
    {
        public Task<ResponseModel> Register(RegisterViewModel model);
        Task<ResponseModel> ValidateUser(RegisterViewModel model);
        Task<ResponseModel> UpdateProfile(UpdateProfileModel model);
        Task<ResponseModel> ResendOTP(ResendOTPViewModel model);
        Task<ResponseModel> WebAdminLogin(LoginViewModel request);
        Task<ResponseModel> ForgotPassword(ForgotPasswordViewModel model);
        Task<ResponseModel> ResetPassword(ResetPasswordViewModel model);
        Task<ResponseModel> VerifyEmail(VerifyEmailViewModel model);
        Task<ResponseModel> ForgotUsername(ForgotUsernameViewModel model);
        Task<ResponseModel> RecoverUsername(ForgotUsernameViewModel model);
        Task<ResponseModel> ChangePassword(ChangePasswordViewModel model);
        Task<ResponseModel> AdminRestPassword(ForgotPasswordViewModel model);
        Task<ResponseModel> GetAllUsersAsync(RequestAccountModel request);
        Task<ResponseModel> SearchByUsersPgnAsync(RequestWithPaginationModel request);
        Task<ResponseModel> GetUserAsync(string id);
        Task<ResponseModel> GetUserBriefAsync(string id);
        public Task<ResponseModel> RegisterNonProfit(RegisterNonProfitRequestModel model);
        public Task<ResponseModel> ActivateNonProfit(string userId);
        Task<ResponseModel> ExportGetAllUsers(RequestAccountModel request);
    }
}
