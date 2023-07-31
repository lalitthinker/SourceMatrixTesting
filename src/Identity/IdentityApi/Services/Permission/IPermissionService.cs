namespace IdentityApi.Services.Permission
{
    public interface IPermissionService
    {
        public Task<ResponseModel> GetAllPermission(GetAllPermissionRequestModel requestModel);
        public Task<ResponseModel> GetPermissionByRoleId(GetPermissionByRoleIdRequestModel requestModel);
        public Task<ResponseModel> UpdatePermissionByRoleId(InsertRequstModel requestModel);
        public Task<ResponseModel> GetPermissionAccessByUserId(string UserId);
    }
}
