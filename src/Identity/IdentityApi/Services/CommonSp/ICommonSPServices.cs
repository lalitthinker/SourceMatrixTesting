namespace IdentityApi.Services.CommonSp
{
    public interface ICommonSPServices
    {
        Task<List<UserByRoleIdVM>> GetAllUsersWithFilter(RequestAccountModel request);
        Task<List<RequestAllUsersRolesModel>> GetRole(string item);
        Task<List<RequestAllUsersRolesModel>> GetRoleById(RequestAccountModel request);
        Task<List<UserByRoleIdVM>> GetUsersByRoleId(RequestAccountModel request);
        Task<List<UserByRoleIdVM>> GetUsersByRoleIdForUpdatingRole(string RoleId);
        Task<bool> SetSaleCommission(string saleCommissionId, List<string> userIds);
        Task<bool> SetPurchaseCommission(string purchaseCommissionId, List<string> userIds);
    }
}
