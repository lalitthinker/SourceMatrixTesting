using IdentityApi.Models.FavoriteDockModels;

namespace IdentityApi.Services.FavoriteDocks
{
    public interface IFavoriteDockService
    {
        public Task<ResponseModel> InsertFavoriteDock([FromBody] List<FavoriteDockVM> favoriteDockVM);
        public Task<ResponseModel> GetAllThemeCustomizations(GetAllFavoriteDockModel request);
    }
}
