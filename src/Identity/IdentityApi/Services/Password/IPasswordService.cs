namespace IdentityApi.Services.Password
{
    public interface IPasswordService
    {
        public Task<ResponseModel> ChangePassword(ChangePasswordViewModel model);

    }
}
