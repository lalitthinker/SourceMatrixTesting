using IdentityApi.Models.DropDownModel;

namespace IdentityApi.Services.ApiRequests.HttpApiRequests
{
    public interface IHttpApiRequests
    {
        Task<List<DropDownVM>> GetAllSaleCommissionRole();
        Task<string> GetSalePersonInfo();
        Task<List<DropDownVM>> GetAllPurchaseCommissionRole();
        Task UpdateNonProfitPhoto(UpdatePhotoNonProfitRequestModel data);
    }
}
