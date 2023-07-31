namespace IdentityApi.Services.Roles
{
    public interface IRoleService
    {
        Task<ResponseModel> GetAllRoles(RequestAccountModel request);
        Task<ResponseModel> GetAllRolesByUserCount(PaginationModel request);
        Task<ResponseModel> GetRoleById(RequestRoleModel request);
        Task<ResponseModel> RoleUpdate(Role identityRole);
        Task<ResponseModel> RoleCreate(Role identityRole);
        Task<ResponseModel> DeleteConfirmed(RoleDelect Requestmodel);
        Task<ResponseModel> RoleSave(Role request);
        Task<ResponseModel> GetActiveRoles(RequestAccountModel request);
    }
}
