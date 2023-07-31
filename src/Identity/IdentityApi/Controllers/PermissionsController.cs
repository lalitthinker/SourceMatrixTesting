using IdentityApi.Services.Permission;

namespace IdentityApi.Controllers
{
    public class PermissionsController : BaseController
    {
        #region Fields
        private readonly ILogger<RoleController> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPermissionService _permissionService;
        #endregion

        #region Ctor
        public PermissionsController(ILogger<RoleController> logger, RoleManager<ApplicationRole> roleManager, ApplicationDbContext applicationDbContext, IPermissionService permissionService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
            _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
        }
        #endregion

        #region GetAllPermissions
        [HttpPost()]
        [Route("GetAllPermission")]
        [SwaggerOperation(Summary = "Get All Permissions", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetAllPermission(GetAllPermissionRequestModel requestModel)
        {
            return await _permissionService.GetAllPermission(requestModel);
        }
        #endregion

        #region GetPermissionByRoleId
        [HttpPost()]
        [Route("GetPermissionByRoleId")]
        [SwaggerOperation(Summary = "Get Permission By RoleId", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetPermissionByRoleId(GetPermissionByRoleIdRequestModel requestModel)
        {
            return await _permissionService.GetPermissionByRoleId(requestModel);
        }
        #endregion

        #region UpdatePermissionByRoleId
        [HttpPost()]
        [Route("UpdatePermissionByRoleId")]
        [SwaggerOperation(Summary = "Update Permission By RoleId", Description = "")]
        public async Task<ActionResult<ResponseModel>> UpdatePermissionByRoleId(InsertRequstModel requestModel)
        {
            return await _permissionService.UpdatePermissionByRoleId(requestModel);
        }
        #endregion

        #region GetPermissionAccessByUserId
        [HttpGet()]
        [Route("GetPermissionAccessByUserId")]
        [SwaggerOperation(Summary = "Get Permission  Access By UserId", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetPermissionAccessByUserId(string UserId)
        {
            return await _permissionService.GetPermissionAccessByUserId(UserId);
        }
        #endregion
    }
}



