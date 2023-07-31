using IdentityApi.Services.Roles;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers
{
    public class RoleController : BaseController
    {
        #region Fields
        private readonly ILogger<RoleController> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IRoleService _roleService;
        #endregion
        
        #region Ctor
        public RoleController(ILogger<RoleController> logger, RoleManager<ApplicationRole> roleManager, ApplicationDbContext applicationDbContext,IRoleService roleService)
        {
            _logger = logger;
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
            _roleService = roleService;
        }

        #endregion

        #region GetAllRoles
        /// <summary>
        /// GET: /Role/GetRoles?search={value}
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>

        [HttpPost()]
        [Route("GetAllRoles")]
        [SwaggerOperation(Summary = "Get All Roles", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetAllRoles(RequestAccountModel request)
        {
            return await _roleService.GetAllRoles(request);
        }
        #endregion

        #region GetAllRolesByUserCount
        [HttpPost()]
        [Route("GetAllRolesByUserCount")]
        [SwaggerOperation(Summary = "Get all roles by UserCount", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetAllRolesByUserCount(PaginationModel request)
        {
            return await _roleService.GetAllRolesByUserCount(request);
        }
        #endregion

        #region GetRoleById
        [HttpPost()]
        [Route("GetRoleById")]
        [SwaggerOperation(Summary = "Get role by Id", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetRoleById(RequestRoleModel model)
        {
            return await _roleService.GetRoleById(model);
        }
        #endregion

        #region UpdateRole
        [HttpPost()]
        [Route("RoleUpdate")]
        [SwaggerOperation(Summary = "Role Update", Description = "")]
        public async Task<ActionResult<ResponseModel>> RoleUpdate(Role identityRole)
        {
            return await _roleService.RoleUpdate(identityRole);
        }
        #endregion

        #region CreateRoles
        [HttpPost()]
        [Route("RoleCreate")]
        [SwaggerOperation(Summary = "Create Role", Description = "")]
        public async Task<ActionResult<ResponseModel>> RoleCreate(Role identityRole)
        {
            return await _roleService.RoleCreate(identityRole);
        }
        #endregion

        #region DeleteRoles
        [HttpPost()]
        [Route("Delete")]
        [SwaggerOperation(Summary = "Delete Roles", Description = "")]
        public async Task<ActionResult<ResponseModel>> DeleteConfirmed(RoleDelect Requestmodel)
        {
            return await _roleService.DeleteConfirmed(Requestmodel);
        }
        #endregion

        #region SaveRole
        [HttpPost()]
        [Route("save")]
        [SwaggerOperation(Summary = "Save", Description = "")]
        public async Task<ActionResult<ResponseModel>> RoleSave(Role request)
        {
            return await _roleService.RoleSave(request);
        }
        #endregion

        #region GetActiveRoles
        [HttpPost()]
        [Route("GetActiveRoles")]
        [SwaggerOperation(Summary = "Get Active Roles", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetActiveRoles(RequestAccountModel request)
        {
            return await _roleService.GetActiveRoles(request);
        }
        #endregion
    }
}
