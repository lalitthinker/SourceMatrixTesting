using IdentityApi.Controllers;

namespace IdentityApi.Services.Permission
{
    public class PermissionService : IPermissionService
    {
        #region feilds
        private readonly ILogger<RoleController> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region ctor
        public PermissionService(ILogger<RoleController> logger, RoleManager<ApplicationRole> roleManager, ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        #endregion

        #region methods

        #region GetAllPermission
        public async Task<ResponseModel> GetAllPermission(GetAllPermissionRequestModel requestModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var query4 = @"exec [dbo].[sp_GetAllPermissionList] @GetAll='true', @RoleId='" + requestModel.RoleId + "', @SearchText = '" + requestModel.SearchText + "'";
                List<ClaimGroupsVM> claimGroups = await _applicationDbContext.CustomGroupsVMs.FromSqlRaw(query4)!.ToListAsync();

                PermisssionModel permisssion = new PermisssionModel();

                permisssion.claimTypesNameslist = claimGroups.Select(claimTypeNameItems => new ClaimTypesName()
                {
                    ClaimTypeName = claimTypeNameItems.ClaimTypeName,
                    ClaimTypeId = claimTypeNameItems.ClaimTypeId,
                    claimGroupNamelist = claimGroups.Where(claimGroupsItems => claimGroupsItems.ClaimTypeName == claimTypeNameItems.ClaimTypeName).Select(claimGroupsNameItems => new ClaimGroupName()
                    {
                        ClaimGroupNames = claimGroupsNameItems.ClaimGroupName,
                        ClaimGroupId = claimGroupsNameItems.ClaimGroupId,
                        ClaimTypeId = claimGroupsNameItems.ClaimTypeId,

                        claimValuesList = claimGroups.Where(claimValuelistItems => claimValuelistItems.ClaimGroupName == claimGroupsNameItems.ClaimGroupName).Select(claimValueItems => new ClaimValue()
                        {
                            CustomClaimId = claimValueItems.Id,
                            ClaimValues = claimValueItems.ClaimValue,
                            IsAccess = claimValueItems.IsAllowed,
                            ClaimTypeId = claimValueItems.ClaimTypeId
                        }).Where(a => a.ClaimTypeId == claimGroupsNameItems.ClaimTypeId).ToList()
                    }).GroupBy(claimGroups => claimGroups.ClaimGroupNames, (key, group) => group.First()).GroupBy(claimGroups => claimGroups.ClaimGroupId, (key, group) => group.First()).ToList()

                }).GroupBy(claimTypes => claimTypes.ClaimTypeName, (key, group) => group.First()).GroupBy(claimTypes => claimTypes.ClaimTypeId, (key, group) => group.First()).ToList();

                response.IsSuccess = true;
                response.Message = $"Total {permisssion.claimTypesNameslist.Count} {(permisssion.claimTypesNameslist.Count > 1 ? "permissions" : "permissions")} found.";
                response.Response = permisssion;
                response.TotalRecords = permisssion.claimTypesNameslist.Count;
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

        #region Get permission by role id
        public async Task<ResponseModel> GetPermissionByRoleId(GetPermissionByRoleIdRequestModel requestModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var query4 = @"exec [dbo].[sp_GetPermissionByRoleId] @GetAll='true',
                                                                      @ApplicationRoleId='" + requestModel.ApplicationRoleId + "'," +
                                                                      "@SearchText='" + requestModel.SearchText + "'";
                List<ClaimGroupsVM> claimGroups = await _applicationDbContext.CustomGroupsVMs.FromSqlRaw(query4)!.ToListAsync();

                PermisssionModel permisssion = new PermisssionModel();
                permisssion.claimTypesNameslist = claimGroups.Select(claimTypeNameItems => new ClaimTypesName()
                {
                    ClaimTypeName = claimTypeNameItems.ClaimTypeName,
                    ClaimTypeId = claimTypeNameItems.ClaimTypeId,
                    claimGroupNamelist = claimGroups.Where(claimGroupsItems => claimGroupsItems.ClaimTypeName == claimTypeNameItems.ClaimTypeName).Select(claimGroupsNameItems => new ClaimGroupName()
                    {
                        ClaimGroupNames = claimGroupsNameItems.ClaimGroupName,
                        ClaimGroupId = claimGroupsNameItems.ClaimGroupId,
                        ClaimTypeId = claimGroupsNameItems.ClaimTypeId,

                        claimValuesList = claimGroups.Where(claimValuelistItems => claimValuelistItems.ClaimGroupName == claimGroupsNameItems.ClaimGroupName).Select(claimValueItems => new ClaimValue()
                        {
                            CustomClaimId = claimValueItems.Id,
                            ClaimValues = claimValueItems.ClaimValue,
                            IsAccess = claimValueItems.IsAllowed,
                            ClaimTypeId = claimValueItems.ClaimTypeId
                        }).Where(a => a.ClaimTypeId == claimGroupsNameItems.ClaimTypeId).ToList()
                    }).GroupBy(claimGroups => claimGroups.ClaimGroupNames, (key, group) => group.First()).GroupBy(claimGroups => claimGroups.ClaimGroupId, (key, group) => group.First()).ToList()

                }).GroupBy(claimTypes => claimTypes.ClaimTypeName, (key, group) => group.First()).GroupBy(claimTypes => claimTypes.ClaimTypeId, (key, group) => group.First()).ToList();

                response.IsSuccess = true;
                response.Message = $"Total {permisssion.claimTypesNameslist.Count} {(permisssion.claimTypesNameslist.Count > 1 ? "permissions" : "permission")} found.";
                response.Response = permisssion;
                response.TotalRecords = permisssion.claimTypesNameslist.Count;
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

        #region Update permission by role id 
        public async Task<ResponseModel> UpdatePermissionByRoleId(InsertRequstModel requestModel)
        {
            ResponseModel response = new();
            try
            {
                List<PermissionMappingRole> model = new List<PermissionMappingRole>();
                if (requestModel.CustomClaimsId.Count == 0)
                {
                    var query = @"exec [dbo].[sp_DeleteAllPermissionByRoleId] @RoleId='" + requestModel.RoleId + "'";
                    var updateCount = _applicationDbContext.Database.ExecuteSqlRaw(query);
                    response.IsSuccess = true;
                    response.Message = $"Permissions Removed Successfully";
                    return response;
                }
                var existingRoles = _applicationDbContext.MappingRoles.Where(a => a.ApplicationRoleId == requestModel.RoleId);
                if (existingRoles != null)
                {
                    _applicationDbContext.MappingRoles.RemoveRange(existingRoles);
                    await _applicationDbContext.SaveChangesAsync();
                }

                foreach (var CheckData in requestModel.CustomClaimsId)
                {
                    model.Add(new PermissionMappingRole
                    {
                        ApplicationRoleId = requestModel.RoleId,
                        CustomClaimsId = CheckData,
                        Name = requestModel.Name
                    });

                }
                _applicationDbContext.MappingRoles.UpdateRange(model);
                await _applicationDbContext.SaveChangesAsync();
                response.IsSuccess = true;
                response.Response = model;
                response.TotalRecords = model.Count;
                if (response.TotalRecords == 0)
                {
                    response.Message = "Failed to Update.";
                }
                else
                {
                    response.Message = "Permissions Updated Successfully.";
                }

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

        #region Get permission access by user Id
        public async Task<ResponseModel> GetPermissionAccessByUserId(string UserId)
        {
            ResponseModel response = new();
            PermissionAccessByUserIdResponseModel permissionAccess = new();
            try
            {
                var query = @"exec [dbo].[sp_GetRoleByUserId] @UserId='" + UserId + "'";
                List<RoleIdModel> roleIds = await _applicationDbContext.RoleIdsModel.FromSqlRaw(query)!.ToListAsync();
                var roleData = roleIds.FirstOrDefault();
                if (roleData != null)
                {
                    var query1 = @"exec [dbo].[sp_GetPrmissionNameById] @RoleId='" + roleData.RoleId + "'";
                    var PermissionNameList = _applicationDbContext.permissionAccessByUsers.FromSqlRaw(query1)!.ToListAsync().Result;

                    if (PermissionNameList != null)
                    {
                        string Names = string.Join(',', PermissionNameList.Select(x => x.PermissionNamesList));
                        permissionAccess.PermissionNamesList = Names.Split(',').ToList();
                    }
                    response.IsSuccess = true;
                    response.Message = $"Total {PermissionNameList.Count} Permissions found.";
                    response.TotalRecords = PermissionNameList.Count;
                    response.Response = permissionAccess;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No Permissions Found.";
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }
        #endregion

        #endregion
    }
}
