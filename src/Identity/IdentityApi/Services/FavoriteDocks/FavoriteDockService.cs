using IdentityApi.Controllers;
using IdentityApi.Models.FavoriteDockModels;

namespace IdentityApi.Services.FavoriteDocks
{
    public class FavoriteDockService : IFavoriteDockService
    {
        #region fields
        private readonly ILogger<FavoriteDockController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;
        #endregion

        #region ctor
        public FavoriteDockService(ILogger<FavoriteDockController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        }
        #endregion

        #region InsertFavoriteDock
        public async Task<ResponseModel> InsertFavoriteDock([FromBody] List<FavoriteDockVM> favoriteDockVM)
        {
            ResponseModel response = new();
            try
            {
                List<FavoriteDock> model = new();

                var existingFavoriteDocks = _applicationDbContext.FavoriteDocks.Where(a => a.UserId == favoriteDockVM.Select(a => a.UserId).FirstOrDefault());

                if (existingFavoriteDocks != null)
                {
                    _applicationDbContext.FavoriteDocks.RemoveRange(existingFavoriteDocks);
                    await _applicationDbContext.SaveChangesAsync();
                }

                foreach (var rquestData in favoriteDockVM)
                {
                    model.Add(new FavoriteDock
                    {
                        UserId = rquestData.UserId,
                        IconId = rquestData.IconId,
                        IsPinned = rquestData.IsPinned,
                        CreatedAt = DateTime.Now,
                        IsDeleted = false,
                        ExternalId = Guid.NewGuid().ToString()
                    });
                }
                _applicationDbContext.FavoriteDocks.AddRange(model);
                await _applicationDbContext.SaveChangesAsync();
                if (existingFavoriteDocks == null)
                {
                    response.Message = " FavoriteDocks Created Successfully";
                    response.IsSuccess = true;
                    response.Response = model;
                }
                else
                {
                    response.Message = " FavoriteDocks Updated successfully";
                    response.IsSuccess = true;
                    response.Response = model;
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

        #region GetAllThemeCustomizations
        public async Task<ResponseModel> GetAllThemeCustomizations(GetAllFavoriteDockModel request)
        {
            ResponseModel response = new();
            try
            {
                var query = @"exec [dbo].[sp_GetAllFavoriteDock] @GetAll='true', @PageSize = '" + request.PageSize + "',@SearchText = '" + request.SearchText + "',@SortColumn = '" + request.SortColumn + "',@SortDirection = '" + request.SortDirection + "', @Page = '" + request.PageNumber + "', @UserId = '" + request.UserId + "'";
                List<FavoriteDockViewModel> favoriteDockData = await _applicationDbContext.FavoriteDockViewModels.FromSqlRaw(query)!.ToListAsync();
                response.IsSuccess = true;
                response.Message = $"Total {favoriteDockData.Count} {(favoriteDockData.Count > 1 ? "Favorite DockData" : "Favorite DockData")} found.";
                response.Response = favoriteDockData;
                response.TotalRecords = favoriteDockData.Count;
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
    }
}
