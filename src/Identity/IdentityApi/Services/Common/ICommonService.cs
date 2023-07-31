using IdentityApi.Models.Company;

namespace IdentityApi.Services.Common
{
    public interface ICommonService
    {

        public Task<List<ResponseModel>> UploadProfileService(IFormFile uploadProfilesModel);
        Task<string> UploadProfileImageAsync(string uploadFile);
        Task<string> GetImageAsBase64Url(string url);
        Task<List<UserProfileViewModel>> getUsersDetails(RequestAccountModel request);
        Task<List<CompanyModel>> GetCompaniesData(string id);
    }
}
