using IdentityApi.Domain;
using IdentityApi.Migrations.ApplicationDb;
using IdentityApi.Models.FavoriteDockModels;
using IdentityApi.Models.ThemeCustomizationsModel;
using IdentityApi.Services.FavoriteDocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers
{

    public class FavoriteDockController :  BaseController
    {
        #region feilds
        private readonly ILogger<FavoriteDockController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IFavoriteDockService _favoriteDockService;
        #endregion

        #region ctor
        public FavoriteDockController(ILogger<FavoriteDockController> logger, ApplicationDbContext applicationDbContext, IFavoriteDockService favoriteDockService)
        {
            _logger = logger;
            _applicationDbContext = applicationDbContext;
            _favoriteDockService = favoriteDockService;
        }
        #endregion

        #region create 
        [HttpPost()]
        [Route("InsertUpdateFavoriteDock")]
        [SwaggerOperation(Summary = "insert  FavoriteDock' ", Description = "")]
        public async Task<ActionResult<ResponseModel>> InsertFavoriteDock([FromBody] List<FavoriteDockVM>  favoriteDockVM)
        { 
            return await _favoriteDockService.InsertFavoriteDock(favoriteDockVM);
        }
        #endregion

        #region getAll
        [HttpPost()]
        [Route("GetAllFavoriteDocks")]
        [SwaggerOperation(Summary = "Get All Theme FavoriteDocks' ", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetAllThemeCustomizations(GetAllFavoriteDockModel request)
        {
            return await _favoriteDockService.GetAllThemeCustomizations(request);
        }
        #endregion

        
    }
}
