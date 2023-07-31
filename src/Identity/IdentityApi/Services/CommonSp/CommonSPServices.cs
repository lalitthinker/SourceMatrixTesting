namespace IdentityApi.Services.CommonSp
{
    public class CommonSPServices : ICommonSPServices
    {
        #region fields
        private readonly ApplicationDbContext _applicationDbContext;

        #endregion

        #region ctor
        public CommonSPServices(ApplicationDbContext applicationDbContext
                               )
        {
            _applicationDbContext = applicationDbContext;

        }
        #endregion

        public async Task<List<UserByRoleIdVM>> GetAllUsersWithFilter(RequestAccountModel request)
        {
            try
            {
                var query = @"exec [sp_GetAllUsersWithFilter] @GetAll='" + request.GetAll + "', " +
                                                              "@PageSize = '" + request.PageSize + "'," +
                                                                "@SortColumn = '" + request.SortColumn + "'," +
                                                                "@SortDirection = '" + request.SortDirection + "'," +
                                                                "@Page = '" + request.PageNumber + "'," +
                                                                "@Date = '" + request.Date + "'," +
                                                                "@Userstatus='" + request.Userstatus + "'," +
                                                                //"@SearchText  = '" + searchText + "', " +
                                                                "@Id = '" + request.Id + "' ";
                var Data = await _applicationDbContext.UserByRolesId.FromSqlRaw(query)!.ToListAsync();
                return Data.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
        }

        public async Task<List<UserByRoleIdVM>> GetUsersByRoleIdForUpdatingRole(string RoleId)
        {
            try
            {
                var query = @"exec [sp_GetUsersByRoleId] @GetAll='" + true + "', " +
                                                        "@PageSize = '" + 10 + "'," +
                                                        "@SortColumn = '" + "" + "'," +
                                                        "@SortDirection = '" + "" + "', " +
                                                        "@Page = '" + 1 + "', " +
                                                        "@Userstatus= '" + 1 + "'," +
                                                        "@Date = '" + "" + "' , " +
                                                        "@RoleId = '" + RoleId + "' , " +
                                                        "@Id = '" + 0 + "' ";
                var Data = await _applicationDbContext.UserByRolesId.FromSqlRaw(query)!.ToListAsync();
                return Data.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
        }

        public async Task<List<UserByRoleIdVM>> GetUsersByRoleId(RequestAccountModel request)
        {
            try
            {
                var query = @"exec [sp_GetUsersByRoleId] @GetAll='" + request.GetAll + "', " +
                                                        "@PageSize = '" + request.PageSize + "'," +
                                                        "@SortColumn = '" + request.SortColumn + "'," +
                                                        "@SortDirection = '" + request.SortDirection + "', " +
                                                        "@Page = '" + request.PageNumber + "', " +
                                                        "@Userstatus= '" + request.Userstatus + "'," +
                                                        "@Date = '" + request.Date + "' , " +
                                                        "@RoleId = '" + request.RoleId + "' , " +
                                                        "@Id = '" + request.Id + "' ";
                var Data = await _applicationDbContext.UserByRolesId.FromSqlRaw(query)!.ToListAsync();
                return Data.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
        }

        public async Task<List<RequestAllUsersRolesModel>> GetRoleById(RequestAccountModel request)
        {
            try
            {
                var query = @"exec [sp_GetRoleById] @RoleId='" + request.RoleId + "' ";
                var Data = await _applicationDbContext.RequestAllUsersRoles.FromSqlRaw(query)!.ToListAsync();
                return Data.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
        }

        public async Task<List<RequestAllUsersRolesModel>> GetRole(string item)
        {
            try
            {
                var query = @"exec [sp_GetRole] @RoleName='" + item + "' ";
                var Data = await _applicationDbContext.RequestAllUsersRoles.FromSqlRaw(query)!.ToListAsync();
                return Data.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
        }

        #region Assign Sale Commission
        public async Task<bool> SetSaleCommission(string saleCommissionId, List<string> userIds)
        {
            string values = String.Join(",", userIds.Select(p => p.ToString()).ToArray());
            string query = @"EXEC sp_SetSaleCommission  @SaleCommissionId= '" + saleCommissionId + "'," +
                                                     "@UserIds  = '" + values + "'";
            var updateCount = _applicationDbContext.Database.ExecuteSqlRaw(query);
            return updateCount > 0;
        }
        #endregion

        #region Assign Purchase Commission
        public async Task<bool> SetPurchaseCommission(string purchaseCommissionId, List<string> userIds)
        {
            string values = String.Join(",", userIds.Select(p => p.ToString()).ToArray());
            string query = @"EXEC sp_SetPurchaseCommission  @PurchaseCommissionId= '" + purchaseCommissionId + "'," +
                                                     "@UserIds  = '" + values + "'";
            var updateCount = _applicationDbContext.Database.ExecuteSqlRaw(query);
            return updateCount > 0;
        }
        #endregion
    }
}
