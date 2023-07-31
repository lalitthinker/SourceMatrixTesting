namespace IdentityApi.Services.Password
{
    public class PasswordService : IPasswordService
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region ctor
        public PasswordService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        #endregion

        #region ChangePassword
        public async Task<ResponseModel> ChangePassword(ChangePasswordViewModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Check whether user already exists with the userId
                ApplicationUser User = await _userManager.FindByIdAsync(model.UserId);
                if (User == null)
                {
                    response.Message = "Invalid user.";
                    return response;
                }

                // Cache passwords
                var oldPassword = model.OldPassword;
                var newPassword = model.NewPassword;
                var confirmNewPassword = model.ConfirmNewPassword;

                if (newPassword != confirmNewPassword)
                {
                    response.Message = "The new password and confirmation password do not match";
                    return response;
                }

                if (newPassword == oldPassword)
                {
                    response.Message = "The new password and old password can not be same";
                    return response;
                }

                // Verify old password and then change to new Password
                var identityResult = await _userManager.ChangePasswordAsync(User, oldPassword, newPassword);

                if (identityResult.Errors.Any())
                {
                    // User Manager action failed
                    response.Message = identityResult.GetErrorsAsString();
                    response.Response = identityResult.Errors.Select(e => e.Description).ToList();
                    return response;
                }

                // Return Success Response
                response.IsSuccess = true;
                response.Message = "Your password has been changed successfully!";
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion
    }


}
