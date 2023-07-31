using IdentityApi.Models.ThemeCustomizationsModel;

namespace IdentityApi.Services.ThemeCustomizations
{
    public interface IThemeCustomizationService
    {
        public Task<ResponseModel> InsertThemeCustomization([FromBody] ThemeCustomizationVM customizationVM);
        public Task<ResponseModel> GetAllThemeCustomizations(GetAllThemeCustomizationModel request);
        public Task<ResponseModel> DeleteThemeCustomization(long Id);
    }
}
