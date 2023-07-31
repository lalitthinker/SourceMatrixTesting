using IdentityApi.Models.ThemeCustomizationsModel;
using IdentityApi.Models.Users;
using IdentityApi.Services.ThemeCustomizations;

namespace IdentityApi.Controllers
{
    public class ThemeCustomizationController : BaseController
    {
        #region feilds
        private readonly ILogger<ThemeCustomizationController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IThemeCustomizationService _themeCustomizationService;
        #endregion

        #region ctor
        public ThemeCustomizationController(ILogger<ThemeCustomizationController> logger, ApplicationDbContext applicationDbContext, IThemeCustomizationService themeCustomizationService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _themeCustomizationService = themeCustomizationService ?? throw new ArgumentNullException(nameof(themeCustomizationService));
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        }
        #endregion

        #region create 
        [HttpPost()]
        [Route("InsertUpdateThemeCustomization")]
        [SwaggerOperation(Summary = "insert Theme customization' ", Description = "")]
        public async Task<ActionResult<ResponseModel>> InsertThemeCustomization([FromBody] ThemeCustomizationVM customizationVM)
        {
            return await _themeCustomizationService.InsertThemeCustomization(customizationVM);
        }
        #endregion

        #region getAll
        [HttpPost()]
        [Route("GetAllThemeCustomization")]
        [SwaggerOperation(Summary = "Get All Theme customization' ", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetAllThemeCustomizations(GetAllThemeCustomizationModel request)
        {
            return await _themeCustomizationService.GetAllThemeCustomizations(request);
        }
        #endregion

        #region Delete
        [HttpDelete()]
        [Route("DeleteThemeCustomization")]
        [SwaggerOperation(Summary = "Delete Theme customization' ", Description = "")]
        public async Task<ActionResult<ResponseModel>> DeleteThemeCustomization(long Id)
        {
            return await _themeCustomizationService.DeleteThemeCustomization(Id);
        }
        #endregion
    }
}
