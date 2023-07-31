using IdentityApi.Models.DropDownModel;

namespace IdentityApi.Services.DropDown
{
    public class DropDownService : IDropDownService
    {
        #region Fields
        private readonly IHttpApiRequests _httpApiRequests;
        #endregion

        #region Ctor
        public DropDownService(IHttpApiRequests httpApiRequests)
        {
            _httpApiRequests = httpApiRequests ?? throw new ArgumentNullException(nameof(httpApiRequests));
        }
        #endregion

        #region Get all dropdownlist 
        public async Task<ResponseModel> GetAllDropDownList()
        {
            ResponseModel response = new();
            DropDownRecordModel dropDownRecord = new();
            try
            {
                dropDownRecord.SaleCommissionsList = await _httpApiRequests.GetAllSaleCommissionRole();
                dropDownRecord.PurchaseCommissionsList = await _httpApiRequests.GetAllPurchaseCommissionRole();

                response.Response = dropDownRecord;
                response.IsSuccess = true;
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
