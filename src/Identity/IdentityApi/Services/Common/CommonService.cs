using IdentityApi.Models.Company;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;

namespace IdentityApi.Services.Common
{
    public class CommonService : ICommonService
    {
        #region Fields
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHttpApiRequests _httpApiRequests;
        #endregion

        #region ctor
        public ILogger<CommonService> _logger { get; }
        public IConfiguration Configuration { get; }
        public CommonService(IHttpClientFactory httpClientFactory,
                             IConfiguration configuration,
                             ILogger<CommonService> logger,
                             ApplicationDbContext applicationDbContext,
                             IHttpApiRequests httpApiRequests)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
            _httpApiRequests = httpApiRequests ?? throw new ArgumentNullException(nameof(httpApiRequests));
        }
        #endregion
        public async Task<List<ResponseModel>> UploadProfileService(IFormFile request)
        {
            try
            {

                string BaseUrl = Configuration["BaseUrl:ResourceAPI"];
                string requestURI = $"{BaseUrl}/api/v1/ResourceApi/Documents/uploadProfile";


                using (var client = new HttpClient())
                {
                    //client.BaseAddress = new Uri(BaseUrl);
                    var httpResponseMessage = new HttpResponseMessage();
                    MultipartFormDataContent multiForm = new MultipartFormDataContent();
                    using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestURI))

                    using (Stream fs = request.OpenReadStream())
                    {
                        multiForm.Add(new StreamContent(fs), "file", request.FileName);
                        requestMessage.Content = multiForm;
                        using (HttpClient httpClient = new HttpClient())
                        {
                            using (var content = new MultipartFormDataContent())
                            {
                                content.Add(new StreamContent(fs), "FormFile", request.FileName);
                                content.Add(new StringContent(request.ContentType), "UploadType");

                                var httpRequest = Task.Run(async () =>
                                {
                                    httpResponseMessage = await httpClient.PostAsync(requestURI, content);
                                });
                                httpRequest.Wait();
                                //   await HandleResponse(httpResponseMessage);
                                string serialized = await httpResponseMessage.Content.ReadAsStringAsync();
                            }
                        }
                    }
                    //var response = await client.PostAsync(requestURI, multiForm);
                    //var response = client.SendAsync(requestMessage).GetAwaiter().GetResult();
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    string apiResponse = await response.Content.ReadAsStringAsync();
                    //    var documentList = JsonConvert.DeserializeObject<Response>(apiResponse);
                    //}
                }


                //using (HttpClient client = _httpClientFactory.CreateClient(nameof(BaseUrl)))
                //{
                //    client.BaseAddress = new Uri(BaseUrl + requestURI);
                //    client.DefaultRequestHeaders.Clear();
                //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //    client.DefaultRequestHeaders.Add("ContentType", "application/json");
                //    //client.DefaultRequestHeaders.Add("ContentType", "application/x-www-form-urlencoded");

                //    using HttpResponseMessage response = await client.PostAsJsonAsync(requestURI, request);
                //    if (response.IsSuccessStatusCode)
                //    {
                //        string apiResponse = await response.Content.ReadAsStringAsync();
                //        var documentList = JsonConvert.DeserializeObject<Response>(apiResponse);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return new List<ResponseModel>();
        }

        public async Task<string> UploadProfileImageAsync(string uploadFile)
        {
            string filename = null;
            try
            {
                if (uploadFile == null)
                {
                    filename = null;
                }
                else
                {
                    //using Stream fileStream = uploadFile.OpenReadStream();
                    filename = await UploadImageAsync(uploadFile);
                    _logger.LogInformation($"\n--> Uploaded File: {uploadFile}\n");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"\n--> Error while uploading file: {uploadFile}\n");
                _logger.LogInformation($"\n--> Exception occured: {ex.Message}\n");
            }

            return filename;
        }

        private async Task<string> UploadImageAsync(string pictureStream)
        {
            ResponseModel responseMoldel = new();
            string BaseUrl = Configuration["BaseUrl:ResourceAPI"];
            string uri = $"{BaseUrl}/api/v1/ResourceApi/Documents/uploadProfile";
            string result = null;

            using (HttpClient httpClient = new())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("ContentType", "application/json");
                var values = new Dictionary<string, string>
                                {
                                { "ProfileImage", pictureStream }
                                };
                var formContent = new FormUrlEncodedContent(values);

                using (var content = new MultipartFormDataContent())
                {

                    HttpResponseMessage response = await httpClient.PostAsync(uri, formContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        ProfileResposeModel re = JsonConvert.DeserializeObject<ProfileResposeModel>(apiResponse);

                        return re.profileImagePath;
                    }
                }

                return result;
            }
        }

        public async Task<string> GetImageAsBase64Url(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }
            string imageData = "";
            string decodedString = "";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    using (Stream stream = await client.GetStreamAsync(url))
                    {
                        if (stream == null)
                            return null;
                        byte[] buffer = new byte[16384];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            while (true)
                            {
                                int num = await stream.ReadAsync(buffer, 0, buffer.Length);
                                int read;
                                if ((read = num) > 0)
                                    ms.Write(buffer, 0, read);
                                else
                                    break;
                            }
                            imageData = @"data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                            // byte[] data = Convert.FromBase64String(imageData);
                            //decodedString = System.Text.ASCIIEncoding.ASCII.GetString(data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return imageData;
            }
        }

        public async Task<List<UserProfileViewModel>> getUsersDetails(RequestAccountModel request)
        {
            try
            {
                var query = @"exec [sp_GetAllUsersWithFilter] @GetAll           = '" + true + "'," +
                                                              "@PageSize        = '" + request.PageSize + "'," +
                                                              "@SortColumn      = '" + request.SortColumn + "'," +
                                                              "@SortDirection   = '" + request.SortDirection + "'," +
                                                              "@Page            = '" + request.PageNumber + "'," +
                                                              "@Date            = '" + request.Date + "'," +
                                                              "@Userstatus      = '" + request.Userstatus + "'," +
                                                              "@SearchText      = '" + request.SearchText + "'";
                var Data = await _applicationDbContext.userProfileViewModels.FromSqlRaw(query)!.ToListAsync();
                return Data.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
        }

        public async Task<List<CompanyModel>> GetCompaniesData(string id)
        {
            try
            {
                var salesPersonData = await _httpApiRequests.GetSalePersonInfo();
                var data = JsonConvert.DeserializeObject<List<CompanyModel>>(salesPersonData);
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
        }
    }



    public class UploadProfileVM
    {
        public List<IFormFile> ProfilePictureFile { get; set; }
    }

}
