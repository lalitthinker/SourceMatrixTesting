using IdentityApi.Models.DropDownModel;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IdentityApi.Services.ApiRequests.HttpApiRequests
{
    public class HttpApiRequests : IHttpApiRequests
    {
        #region Fields
        private readonly IHttpClientFactory _httpClientFactory;
        public IConfiguration Configuration { get; }
        #endregion

        #region ctor
        public HttpApiRequests(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            Configuration = configuration;
        }

        #endregion

        #region Update Non profit photo
        public async Task UpdateNonProfitPhoto(UpdatePhotoNonProfitRequestModel data)
        {
            string requestURI = $"/api/v1/campaignapi/nonprofit/updatePhoto";
            string Result = null;
            string photoResponse = string.Empty;

            using (HttpClient client = _httpClientFactory.CreateClient("CampaignAPI"))
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("ContentType", "application/json");

                using HttpResponseMessage response = await client.PostAsJsonAsync(requestURI, data);
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                }
            }

            return;
        }
        #endregion

        #region Get Sales Person info
        public async Task<string> GetSalePersonInfo()
        {
            ResponseModel result = new();
            string BaseUrl = Configuration["BaseUrl:SourceMatrixAPI"];
            string requestURI = $"{BaseUrl}/api/v1/SourceMatrixApi/Companies/GetCompanyData";
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
                //using (var content = new MultipartFormDataContent())
                //{
                //    content.Add(new MultipartFormDataContent(), SalePersonRoleId);
                using HttpResponseMessage response = await client.GetAsync(requestURI);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            return null;
        }
        #endregion

        #region Get All Sales Commission Role
        public async Task<List<DropDownVM>> GetAllSaleCommissionRole()
        {
            var saleCommissionData = await SendRequestForSaleCommission();
            var data = JsonConvert.DeserializeObject<List<DropDownVM>>(saleCommissionData);
            return data;
        }
        #endregion

        #region Send Request - Sale Commission Role
        public async Task<string> SendRequestForSaleCommission()
        {
            try
            {
                ResponseModel result = new();
                string BaseUrl = Configuration["BaseUrl:SourceMatrixAPI"];
                string requestURI = $"{BaseUrl}/api/v1/SourceMatrixApi/SalesCommissionRate/GetAllSaleCommissionList";
                string Result = null;
                string photoResponse = string.Empty;
                using (HttpClient client = new())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
                    using HttpResponseMessage response = await client.GetAsync(requestURI);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get All Purchase Commission Role
        public async Task<List<DropDownVM>> GetAllPurchaseCommissionRole()
        {
            var purchaseCommissionData = await SendRequestForPurchaseCommission();
            var data = JsonConvert.DeserializeObject<List<DropDownVM>>(purchaseCommissionData);
            return data;
        }
        #endregion

        #region Send Request - Purchase Commission Role
        public async Task<string> SendRequestForPurchaseCommission()
        {
            try
            {
                ResponseModel result = new();
                string BaseUrl = Configuration["BaseUrl:SourceMatrixAPI"];
                string requestURI = $"{BaseUrl}/api/v1/SourceMatrixApi/PurchaseCommissionRate/GetAllPurchaseCommissionList";
                string Result = null;
                string photoResponse = string.Empty;
                using (HttpClient client = new())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
                    using HttpResponseMessage response = await client.GetAsync(requestURI);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
