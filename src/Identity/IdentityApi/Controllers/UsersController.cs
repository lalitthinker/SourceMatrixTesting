using IdentityApi.Services.User;

namespace IdentityApi.Controllers
{
    public class UsersController : BaseController
    {
        #region feilds
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ICommonService _commonService;
        private readonly IUserService _userService;
        #endregion

        #region ctor
        public UsersController(UserManager<ApplicationUser> userManager,
                                                   ApplicationDbContext applicationDbContext,
                                                   ICommonService commonService,
                                                   IUserService userService)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        #endregion

        #region GetUserByRoleId
        [HttpPost()]
        [Route("getUserByRoleId")]
        [SwaggerOperation(Summary = "Get user by RoleId", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetUserByRoleId(RequestUserByRoleIdModel request)
        {
            return await _userService.GetUserByRoleId(request);
        }
        #endregion

        #region GetUserById
        /// <summary>
        /// POST: api/v1/identityapi/users/get
        /// search users : User must be logged in to use this api
        /// used for: admin-web
        /// </summary>

        [HttpPost()]
        [Route("getUserById")]
        [SwaggerOperation(Summary = "Get user by Id", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetUserById(RequestWithPaginationModel request)
        {
            return await _userService.GetUserById(request);
        }
        #endregion

        #region GetNonProfits
        /// <summary>
        /// POST: api/v1/identityapi/users/getnonprofits
        /// search users : User must be logged in to use this api
        /// used for: search functionality in app
        /// </summary>

        [HttpPost()]
        [Route("getnonprofits")]
        [SwaggerOperation(Summary = "used for: search functionality in app", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetFoundationUsersPgnAsync(GetNonProfitsPgnRequestModel request)
        {
            return await _userService.GetFoundationUsersPgnAsync(request);
        }
        #endregion

        #region CreateUser
        /// <summary>
        /// Route: api/v1/identityapi/users/admin/create
        /// Admin-User : create admin user
        /// For: call from admin-web
        /// </summary>

        [HttpPost()]
        [Route("create")]
        [SwaggerOperation(Summary = "Create Admin User", Description = "")]
        public async Task<ActionResult<ResponseModel>> CreateUser(CreateAdminUserRequestModel model)
        {
            return await _userService.CreateUser(model);
        }
        #endregion

        #region UpdateUser
        /// <summary>
        /// Route: api/v1/identityapi/users/admin/update
        /// Admin-User : update admin user
        /// For: call from admin-web
        /// </summary>

        [HttpPost()]
        [Route("update")]
        [SwaggerOperation(Summary = "Update Admin User", Description = "")]
        public async Task<ActionResult<ResponseModel>> UpdateUser(UpdateAdminUserRequestModel model)
        {
            return await _userService.UpdateUser(model);
        }
        #endregion

        #region UpdateUserStatus
        /// <summary>
        /// Route: api/v1/identityapi/users/admin/update
        /// Admin-User : update admin user
        /// For: call from admin-web
        /// </summary>

        [HttpPost()]
        [Route("updateUserStatus")]
        [SwaggerOperation(Summary = "Update Admin User Status", Description = "")]
        public async Task<ActionResult<ResponseModel>> UpdateUserStatus(UpdateUserStatusRequest model)
        {
            return await _userService.UpdateUserStatus(model);
        }
        #endregion

        #region DeleteUser
        [HttpPost]
        [Route("Delete")]
        [SwaggerOperation(Summary = "Delete Admin User", Description = "")]

        public async Task<ActionResult<ResponseModel>> DeleteUser(DeleteUserModel deleteUserModel)
        {
            return await _userService.DeleteUser(deleteUserModel);
        }
        #endregion

        #region GetImage
        [HttpPost()]
        [Route("getImage")]

        public async Task<IActionResult> GetImage(string URL)
        {
            ResponseModel response = new();
            try
            {
                string base64file = await _commonService.GetImageAsBase64Url(URL);
                // Return Success Response
                response.IsSuccess = true;
                response.Message = $"";
                response.Response = base64file;
                return Ok(response);
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;
                return BadRequest(response);
            }
        }
        #endregion

        #region SaveUserSetting
        [HttpPost()]
        [Route("SaveUserSetting")]
        [SwaggerOperation(Summary = "Save User Setting", Description = "")]
        public async Task<ActionResult<ResponseModel>> SaveUserSettings(SaveUserSettingCommand model)
        {
            return await _userService.SaveUserSetting(model);
        }
        #endregion

        #region GetUserSetting
        [HttpGet()]
        [Route("GetUserSetting")]
        [SwaggerOperation(Summary = "Get User Setting", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetUserSetting(string UserId)
        {
            return await _userService.GetUserSetting(UserId);
        }
        #endregion

        #region Add Assign Sales and Purchase Commission
        [HttpPost]
        [Route("AddAssignSalesAndPurchaseCommission")]
        public async Task<ActionResult<ResponseModel>> AddAssignSalesAndPurchaseCommission(AddAssignSalesAndPurchaseCommissionCommand command)
        {
            return await _userService.AddAssignSalesAndPurchaseCommission(command);
        }
        #endregion

        #region Get User Profile Details
        [HttpPost()]
        [Route("GetUserProfileDetailsById")]
        [SwaggerOperation(Summary = "Get User Profile Details By Id", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetUserProfileDetailsById(RequestAccountModel request)
        {
            return await _userService.GetUserProfileDetailsById(request);
        }
        #endregion

    }
}
