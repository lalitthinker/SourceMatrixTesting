namespace IdentityApi.Services.User
{
    public interface IUserService
    {
        public Task<ResponseModel> GetUserByRoleId(RequestUserByRoleIdModel request);
        public Task<ResponseModel> GetUserById(RequestWithPaginationModel request);
        public Task<ResponseModel> GetFoundationUsersPgnAsync(GetNonProfitsPgnRequestModel request);
        public Task<ResponseModel> CreateUser(CreateAdminUserRequestModel model);
        public Task<ResponseModel> UpdateUser(UpdateAdminUserRequestModel model);
        public Task<ResponseModel> UpdateUserStatus(UpdateUserStatusRequest model);
        public Task<ResponseModel> DeleteUser(DeleteUserModel deleteUserModel);
        public Task<ResponseModel> SaveUserSetting(SaveUserSettingCommand command);
        public Task<ResponseModel> AddAssignSalesAndPurchaseCommission(AddAssignSalesAndPurchaseCommissionCommand command);
        public Task<ResponseModel> GetUserSetting(string UserId);
        public Task<ResponseModel> GetUserProfileDetailsById(RequestAccountModel request);
    }
}
