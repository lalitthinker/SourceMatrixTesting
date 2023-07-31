using IdentityApi.Controllers;
using IdentityApi.Models.ThemeCustomizationsModel;

namespace IdentityApi.Services.ThemeCustomizations
{
    public class ThemeCustomizationService : IThemeCustomizationService
    {
        #region fields
        private readonly ILogger<ThemeCustomizationController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        #endregion

        #region ctor
        public ThemeCustomizationService(ILogger<ThemeCustomizationController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        }
        #endregion

        #region InsertUpdate
        public async Task<ResponseModel> InsertThemeCustomization([FromBody] ThemeCustomizationVM customizationVM)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                ThemeCustomization themeCustomizationDataAsync = null;
                themeCustomizationDataAsync = _applicationDbContext.ThemeCustomizations.Where(t => t.UserId == customizationVM.UserId).FirstOrDefault();
                if (themeCustomizationDataAsync == null)
                {
                    themeCustomizationDataAsync = new ThemeCustomization();
                }

                themeCustomizationDataAsync.UserId = customizationVM.UserId;
                themeCustomizationDataAsync.PrimaryMenus = customizationVM.PrimaryMenus.ToString();
                themeCustomizationDataAsync.SecondaryMenus = customizationVM.SecondaryMenus.ToString();
                themeCustomizationDataAsync.FavoriteDocks = customizationVM.FavoriteDocks.ToString();
                themeCustomizationDataAsync.SiteWides = customizationVM.SiteWides.ToString();
                themeCustomizationDataAsync.CreatedAt = DateTime.Now;
                themeCustomizationDataAsync.IsDeleted = false;

                if (themeCustomizationDataAsync == null)
                {
                    themeCustomizationDataAsync.ExternalId = Guid.NewGuid().ToString();
                    _applicationDbContext.ThemeCustomizations.AddRange(themeCustomizationDataAsync);
                    await _applicationDbContext.SaveChangesAsync();
                    response.Message = "Theme Customization Created Successfully";
                    response.IsSuccess = true;
                    response.Response = themeCustomizationDataAsync;
                }
                else

                {
                    _applicationDbContext.ThemeCustomizations.UpdateRange(themeCustomizationDataAsync);
                    await _applicationDbContext.SaveChangesAsync();
                    response.Message = "Theme Customization Updated successfully";
                    response.IsSuccess = true;
                    response.Response = themeCustomizationDataAsync;

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

        #region Getall
        public async Task<ResponseModel> GetAllThemeCustomizations(GetAllThemeCustomizationModel request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var query = @"exec [dbo].[sp_GetAllThemeCustomization] @GetAll='true', @PageSize = '" + request.PageSize + "',@SearchText = '" + request.SearchText + "',@SortColumn = '" + request.SortColumn + "',@SortDirection = '" + request.SortDirection + "', @Page = '" + request.PageNumber + "', @UserId = '" + request.UserId + "'";
                List<ThemeCustomizationViewModel> themeCustomizationData = await _applicationDbContext.ThemeCustomizationViewModels.FromSqlRaw(query)!.ToListAsync();
                response.IsSuccess = true;
                response.Message = $"Total {themeCustomizationData.Count} {(themeCustomizationData.Count > 1 ? "Theme Customization" : "Theme Customizations")} found.";
                response.Response = themeCustomizationData?.FirstOrDefault();
                response.TotalRecords = themeCustomizationData.Count;
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

        #region Delete
        public async Task<ResponseModel> DeleteThemeCustomization(long Id)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var getdeleteThemeCustomizationsData = _applicationDbContext.ThemeCustomizations.Where(t => t.Id == Id).FirstOrDefault();
                if (getdeleteThemeCustomizationsData != null)
                {
                    getdeleteThemeCustomizationsData.IsDeleted = true;
                    _applicationDbContext.ThemeCustomizations.RemoveRange(getdeleteThemeCustomizationsData);
                    await _applicationDbContext.SaveChangesAsync();
                    response.Message = "Theme Customization Data Deleted successfully";
                    response.IsSuccess = true;
                    response.Response = getdeleteThemeCustomizationsData;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }
        #endregion
    }
}
