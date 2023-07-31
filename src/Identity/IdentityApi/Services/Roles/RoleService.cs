using IdentityApi.Controllers;
using IdentityApi.Services.CommonSp;

namespace IdentityApi.Services.Roles
{
    public class RoleService : IRoleService
    {
        #region Fields
        private readonly ILogger<RoleController> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ICommonSPServices _commonSPServices;
        #endregion

        #region Ctor
        public RoleService(ILogger<RoleController> logger, RoleManager<ApplicationRole> roleManager, ApplicationDbContext applicationDbContext, ICommonSPServices commonSPServices)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
            _commonSPServices = commonSPServices ?? throw new ArgumentNullException(nameof(commonSPServices));
        }
        #endregion

        #region Get all roles
        public async Task<ResponseModel> GetAllRoles(RequestAccountModel request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var query = @"exec [dbo].[sp_GetAllRole] @GetAll='true', 
                                                         @SortColumn = '" + request.SortColumn + "'," +
                                                         "@SortDirection = '" + request.SortDirection + "'," +
                                                         " @page = '" + request.PageNumber + "'," +
                                                         "@PageSize = '" + request.PageSize + "', " +
                                                         "@SearchText = '" + request.SearchText + "'," +
                                                         "@Userstatus = '" + request.Userstatus + "'";
                List<RoleVM> rolesData = await _applicationDbContext.RoleVMs.FromSqlRaw(query)!.ToListAsync();
                response.Message = $"Total {rolesData.Count} Roles Data records found.";
                response.IsSuccess = true;
                response.Response = rolesData;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                throw;
            }
            return response;
        }
        #endregion

        #region GetAllRoleByUserCount
        public async Task<ResponseModel> GetAllRolesByUserCount(PaginationModel request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var query = @"exec [dbo].[sp_GetAllUserRoleCount] @GetAll='true', @SortColumn = '" + request.SortColumn + "',@SortDirection = '" + request.SortDirection + "', @page = '" + request.PageNumber + "',@PageSize = '" + request.PageSize + "', @SearchText = '" + request.SearchText + "'";
                List<RoleVM> rolesData = await _applicationDbContext.RoleVMs.FromSqlRaw(query)!.ToListAsync();
                response.Response = rolesData;
                response.Message = $"Total Roles Data {rolesData.Count} records found.\nTotal Roles Data {rolesData.Count} records found.";
                response.IsSuccess = true;
                response.TotalRecords = rolesData.Count;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        #region get role by id
        public async Task<ResponseModel> GetRoleById(RequestRoleModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                if (string.IsNullOrEmpty(model.roleId))
                    return response;

                // Get User details
                ApplicationRole roleInfo = await _roleManager.FindByIdAsync(model.roleId);
                if (roleInfo == null)
                {
                    response.Message = "Role Info Not Exists!";
                    return response;
                }

                Role responseModel = new()
                {
                    Id = roleInfo.Id,
                    Name = roleInfo.Name,
                    RoleColor = roleInfo.RoleColor,
                    IsActive = roleInfo.IsActive,
                    IsSystemRole = roleInfo.IsSystemRole
                };

                response.Message = $"Total 1 Role found.";
                response.IsSuccess = true;
                response.Response = responseModel;
                response.TotalRecords = 1;
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

        #region Role update
        public async Task<ResponseModel> RoleUpdate(Role identityRole)
        {
            ResponseModel response = new();
            try
            {
                if (string.IsNullOrEmpty(identityRole.Id))
                    return response;

                // Get User details
                ApplicationRole roleInfo = await _roleManager.FindByIdAsync(identityRole.Id);
                if (roleInfo == null)
                {
                    response.Message = "Role Info Not Exists!";
                    return response;
                }

                if (roleInfo.Name == "SalesPerson" || roleInfo.Name == "Buyers")
                {
                    if (roleInfo.Name != identityRole.Name)
                    {
                        response.IsSuccess = false;
                        response.Message = "This Role Name Cannot be changed.";
                        return response;
                    }
                }

                roleInfo.Name = identityRole.Name;
                roleInfo.RoleColor = identityRole.RoleColor;
                roleInfo.IsActive = identityRole.IsActive;
                roleInfo.NormalizedName = identityRole.Name.Normalize();

                if (roleInfo.IsActive == false)
                {
                    var Data = await _commonSPServices.GetUsersByRoleIdForUpdatingRole(identityRole.Id);
                    if (Data.Count != 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Failed : Role Already In Use.";
                        return response;
                    }
                }

                IdentityResult userResult = await _roleManager.UpdateAsync(roleInfo);
                if (userResult.Errors.Any())
                {
                    response.Response = userResult.Errors;
                    return response;
                }

                Role responseModel = new()
                {
                    Id = roleInfo.Id,
                    Name = roleInfo.Name,
                    RoleColor = roleInfo.RoleColor,
                    IsActive = roleInfo.IsActive
                };

                response.Message = "Role Update successfully";
                response.IsSuccess = true;
                response.Response = responseModel;
                response.TotalRecords = 1;
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

        #region Role Create
        public async Task<ResponseModel> RoleCreate(Role identityRole)
        {
            ResponseModel response = new();
            try
            {
                // Get roles details
                ApplicationRole roleInfo = await _roleManager.FindByNameAsync(identityRole.Name);
                if (roleInfo != null)
                {
                    response.Message = "Role Info Already Exists!";
                    return response;
                }

                roleInfo = new();
                roleInfo.Name = identityRole.Name;
                roleInfo.NormalizedName = identityRole.Name.Normalize();
                roleInfo.RoleColor = identityRole.RoleColor;
                roleInfo.IsSystemRole = false;
                roleInfo.IsActive = true;
                roleInfo.Id = Guid.NewGuid().ToString();
                IdentityResult userResult = await _roleManager.CreateAsync(roleInfo);
                if (userResult.Errors.Any())
                {
                    response.Response = userResult.Errors;
                    return response;
                }
                List<ApplicationRole> roles = _roleManager.Roles.ToList();
                List<Role> rolelist = new();
                foreach (var item in roles)
                {
                    identityRole = new()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        IsActive = item.IsActive,
                        RoleColor = item.RoleColor,
                    };
                    rolelist.Add(identityRole);
                }
                response.Message = "Role created successfully";
                response.IsSuccess = true;
                response.Response = rolelist;
                response.TotalRecords = rolelist.Count;
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

        #region Delete Confirm
        public async Task<ResponseModel> DeleteConfirmed(RoleDelect Requestmodel)
        {
            ResponseModel response = new();
            try
            {
                if (string.IsNullOrEmpty(Requestmodel.RoleId))
                    return response;

                // Get User details
                ApplicationRole roleInfo = await _roleManager.FindByIdAsync(Requestmodel.RoleId);
                if (roleInfo == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Role Info Not Exists!";

                    return response;
                }

                var existingRoles = _applicationDbContext.MappingRoles.Where(a => a.ApplicationRoleId == Requestmodel.RoleId);

                if (existingRoles != null)
                {
                    _applicationDbContext.MappingRoles.RemoveRange(existingRoles);
                }

                IdentityResult userResult = await _roleManager.DeleteAsync(roleInfo);
                if (userResult.Errors.Any())
                {
                    response.IsSuccess = false;
                    response.Response = userResult.Errors;
                    return response;
                }
                List<ApplicationRole> roles = _roleManager.Roles.ToList();
                List<Role> rolelist = new();
                foreach (var item in roles)
                {
                    Role identityRole = new()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        RoleColor = item.RoleColor,
                        IsActive = item.IsActive
                    };
                    rolelist.Add(identityRole);
                }
                response.Message = "Role deleted successfully";
                response.IsSuccess = true;
                response.Response = rolelist;
                response.TotalRecords = rolelist.Count;
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

        #region Role Save
        public async Task<ResponseModel> RoleSave(Role request)
        {
            ResponseModel response = new();
            try
            {
                if (request.Id is null)
                {
                    // Check if name already exists
                    var role = await _roleManager.FindByNameAsync(request.Name);
                    if (role is not null)
                    {
                        response.Message = "Role name already exists!";
                        return response;
                    }

                    // create role
                    ApplicationRole newrole = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = request.Name,
                        RoleColor = request.RoleColor,
                        IsActive = request.IsActive,
                        NormalizedName = request.Name.Normalize()
                    };
                    IdentityResult result = await _roleManager.CreateAsync(newrole);
                    // Check if saved successfully
                    if (result.Errors.Any())
                    {
                        response.Message = "Role is not created";
                        response.Response = result.Errors;
                        return response;
                    }

                    response.Message = "Role created successfully!";
                }
                // Update
                else
                {
                    // Check if record exists
                    var role = await _roleManager.FindByIdAsync(request.Id);
                    if (role is null)
                    {
                        response.Message = "Role details not found";
                        return response;
                    }

                    // Check if role name already exists
                    var rolewithname = await _roleManager.FindByNameAsync(request.Name);
                    if (rolewithname is not null && rolewithname.Id != request.Id)
                    {
                        response.Message = "Role name already exists!";
                        return response;
                    }

                    // Update role value
                    role.Name = request.Name;
                    role.RoleColor = request.RoleColor;
                    role.IsActive = request.IsActive;
                    await _roleManager.UpdateAsync(role);

                    response.Message = "Role edited successfully!";
                }

                // saved successfully, return list of roles
                var roles = _roleManager.Roles.OrderBy(s => s.Name).ToList();
                List<Role> rolelist = new();
                foreach (var item in roles)
                {
                    rolelist.Add(new()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        RoleColor = item.RoleColor,
                        IsActive = item.IsActive
                    });
                }

                response.IsSuccess = true;
                response.Response = rolelist;
                response.TotalRecords = rolelist.Count;
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

        #region GetActiveRoles
        public async Task<ResponseModel> GetActiveRoles(RequestAccountModel request)
        {
            ResponseModel response = new();
            try
            {
                var query = @"exec [dbo].[sp_GetActiveRoles] @GetAll='true', @SortColumn = '" + request.SortColumn + "',@SortDirection = '" + request.SortDirection + "', @page = '" + request.PageNumber + "',@PageSize = '" + request.PageSize + "', @SearchText = '" + request.SearchText + "'";
                List<RoleVM> rolesData = await _applicationDbContext.RoleVMs.FromSqlRaw(query)!.ToListAsync();

                #region for users count 
                var query1 = @"exec [sp_GetAllUsersWithFilter] @GetAll='" + true + "', " +
                                                              "@PageSize = '" + 1 + "'," +
                                                                "@SortColumn = '" + "" + "'," +
                                                                "@SortDirection = '" + "" + "'," +
                                                                "@Page = '" + 1 + "'," +
                                                                "@Date = '" + "" + "'," +
                                                                "@Userstatus='" + request.Userstatus + "'," +
                                                                //"@SearchText  = '" + searchText + "', " +
                                                                "@Id = '" + request.Id + "' ";
                var Data = await _applicationDbContext.UserByRolesId.FromSqlRaw(query1)!.ToListAsync();
                #endregion

                foreach (var item in rolesData)
                {
                    item.AllUsers = Data.Count;
                }

                response.Response = rolesData;
                response.Message = $"Total Roles Data {rolesData.Count} records found.";
                response.IsSuccess = true;
                response.TotalRecords = rolesData.Count;
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
