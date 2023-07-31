using IdentityApi.Models.Company;
using IdentityApi.Services.CommonSp;

namespace IdentityApi.Services.User
{
    public class UserService : IUserService
    {
        #region feilds
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ICommonService _commonService;
        private readonly ICommonSPServices _commonSPServices;
        private readonly IHttpApiRequests _httpApiRequests;
        #endregion

        #region ctor
        public UserService(UserManager<ApplicationUser> userManager,
                           ApplicationDbContext applicationDbContext,
                           ICommonService commonService,
                           ICommonSPServices commonSPServices,
                           IHttpApiRequests httpApiRequests)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
            _commonSPServices = commonSPServices ?? throw new ArgumentNullException(nameof(commonSPServices));
            _httpApiRequests = httpApiRequests ?? throw new ArgumentNullException(nameof(httpApiRequests));
        }
        #endregion

        #region methods

        #region GetUserByRoleId
        public async Task<ResponseModel> GetUserByRoleId(RequestUserByRoleIdModel request)
        {
            ResponseModel response = new();
            try
            {
                var searchText = SecurityProvider.EncryptTextAsync(request.SearchText);
                var query = @"exec [dbo].[sp_GetUsersByRoleId] @GetAll='" + request.GetAll + "', @SearchText = '" + searchText + "', @PageSize = '" + request.PageSize + "',@SortColumn = '" + request.SortColumn + "',@SortDirection = '" + request.SortDirection + "', @Page = '" + request.PageNumber + "', @Userstatus= '" + request.Userstatus + "',@Date = '" + request.Date + "' , @RoleId = '" + request.RoleId + "' ";
                List<UserByRoleIdVM> userByRoleIdDataAsync = await _applicationDbContext.UserByRolesId.FromSqlRaw(query)!.ToListAsync();
                if (userByRoleIdDataAsync.Count == 0)
                {
                    response.Message = "No Record Found!!";
                    return response;
                }

                List<UserByRoleIdVM> users = new();

                foreach (var User in userByRoleIdDataAsync)
                {
                    var query1 = @"exec [dbo].[sp_GetRoleById] @RoleId='" + request.RoleId + "' ";
                    List<RequestAllUsersRolesModel> allRolesData = await _applicationDbContext.RequestAllUsersRoles.FromSqlRaw(query1)!.ToListAsync();
                    string FirstName = SecurityProvider.DecryptTextAsync(User.FirstName);
                    string LastName = SecurityProvider.DecryptTextAsync(User.LastName);

                    UserByRoleIdVM u = new()
                    {
                        Id = User.Id,
                        FirstName = FirstName,
                        LastName = LastName,
                        FullName = (FirstName + " " + LastName).Trim(),
                        CoverPictureUrl = await _commonService.GetImageAsBase64Url(ResourcePath.GetUrl(ResourceType.UserCoverImage, User.CoverPictureUrl)),
                        CoverPictureThumbUrl = await _commonService.GetImageAsBase64Url(ResourcePath.GetUrl(ResourceType.UserCoverImage, User.CoverPictureUrl)),
                        ProfilePictureUrl = await _commonService.GetImageAsBase64Url(ResourcePath.GetUrl(ResourceType.UserCoverImage, User.CoverPictureUrl)),
                        ProfilePictureThumbUrl = await _commonService.GetImageAsBase64Url(ResourcePath.GetUrl(ResourceType.UserCoverImage, User.CoverPictureUrl)),
                        PhoneNumber = string.IsNullOrEmpty(User.PhoneNumber) ? null : SecurityProvider.DecryptTextAsync(User.PhoneNumber),
                        Email = SecurityProvider.DecryptTextAsync(User.Email),
                        TimeZone = User.TimeZone,
                        DeviceToken = User.DeviceToken,
                        Description = User.Description,
                        CreatedDate = User.CreatedDate,
                        //UserStatues = User.UserStatus1.ToString(),
                        UserStatus = Convert.ToBoolean(User.UserStatus),
                        EmergencyContactName = User.EmergencyContactName,
                        EmergencyContactNumber = User.EmergencyContactNumber,
                        OfficePhoneNumber = User.OfficePhoneNumber,
                        PurchaseQuota = User.PurchaseQuota,
                        SaleQuota = User.SaleQuota,
                        ImageZoomRatio = User.ImageZoomRatio
                    };
                    u.RoleName.AddRange(allRolesData.Where(x => x.UserId == u.Id).Select(x => new RequestAllUsersRolesModel { RoleName = x.RoleName, RoleColor = x.RoleColor }));
                    allRolesData.Select(x => x.RoleName).Where(y => y.Equals(u.Id));
                    users.Add(u);
                }
                response.TotalRecords = users.Count();
                response.Response = users.OrderBy(a => a.FullName).ToList();
                response.Message = $"Total User Data {users.Count} records found.";
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        #region GetUserById
        public async Task<ResponseModel> GetUserById(RequestWithPaginationModel request)
        {
            ResponseModel response = new();
            try
            {
                List<ApplicationUser> userList = _userManager.Users.Where(u => u.Id == request.UserId).ToList();

                List<WebUserVM> users = new();
                List<string> RoleName = new();

                foreach (var User in userList)
                {
                    string FirstName = SecurityProvider.DecryptTextAsync(User.FirstName);
                    string LastName = SecurityProvider.DecryptTextAsync(User.LastName);

                    string sQuote = Convert.ToString(User.SaleQuota);
                    string salesQuote = "";
                    decimal s = decimal.Parse(sQuote.Replace(".00", ""));
                    salesQuote = s.ToString();

                    string pQuote = Convert.ToString(User.PurchaseQuota);
                    string purchaseQuote = "";
                    decimal p = decimal.Parse(pQuote.Replace(".00", ""));
                    purchaseQuote = p.ToString();

                    var Roles = await _userManager.GetRolesAsync(User);
                    WebUserVM u = new()
                    {
                        UserId = User.Id,
                        FirstName = FirstName,
                        LastName = LastName,
                        FullName = (FirstName + " " + LastName).Trim(),
                        UserName = User.UserName,
                        CoverPictureUrl = await _commonService.GetImageAsBase64Url(ResourcePath.GetUrl(ResourceType.UserCoverImage, User.CoverPictureUrl)),
                        CoverPictureThumbUrl = await _commonService.GetImageAsBase64Url(ResourcePath.GetUrl(ResourceType.UserCoverImage, User.CoverPictureUrl)),
                        ProfilePictureUrl = await _commonService.GetImageAsBase64Url(ResourcePath.GetUrl(ResourceType.UserProfileImage, User.CoverPictureUrl)),
                        ProfilePictureThumbUrl = await _commonService.GetImageAsBase64Url(ResourcePath.GetUrl(ResourceType.UserProfileImage, User.CoverPictureUrl)),
                        Email = SecurityProvider.DecryptTextAsync(User.Email),
                        Description = User.Description,
                        Title = User.Title,
                        SuiteApt = User.SuiteApt,
                        ZipCode = User.ZipCode,
                        City = User.City,
                        Country = User.Country,
                        State = User.State,
                        SaleQuota = salesQuote,
                        SaleCommissionRate = User.SaleCommissionRate,
                        IsProfitBasedCommission = User.IsProfitBasedCommission,
                        PurchaseQuota = purchaseQuote,
                        PurchaseCommissionRate = User.PurchaseCommissionRate,
                        AgeRange = User.AgeRange.ToString(),
                        PhoneNumber = string.IsNullOrEmpty(User.PhoneNumber) ? null : SecurityProvider.DecryptTextAsync(User.PhoneNumber),
                        UserStatus = User.UserStatus.ToString(),
                        IsLoggedIn = User.IsLoggedIn,
                        LastLoginDate = User.LastLoginDate,
                        CreatedDate = User.CreatedDate,
                        TotalRecords = userList.Count,
                        EmergencyContactName = User.EmergencyContactName,
                        EmergencyContactNumber = User.EmergencyContactNumber,
                        OfficePhoneNumber = User.OfficePhoneNumber,
                        ImageZoomRatio = User.ImageZoomRatio
                    };
                    u.RoleName.AddRange(Roles.ToList());
                    users.Add(u);
                }

                List<WebUserVM> list = users.OrderBy(s => s.FullName).ToList();

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    string searchText = request.SearchText.ToLower();
                    list = list.FindAll(s => s.FullName.ToLower().Contains(searchText) || s.UserName.ToLower().Contains(searchText) || s.Email.ToLower().Contains(searchText));
                    list.ForEach(s => s.TotalRecords = list.Count);
                }

                if (!request.GetAll)
                {
                    int skip = request.PageSize * (request.PageNumber - 1);
                    int take = request.PageSize;
                    list = list.Skip(skip).Take(take).ToList();
                }

                // Return Success Response
                response.IsSuccess = true;
                response.Message = $"Total {list.Count} {(list.Count > 1 ? "users" : "user")} found.";
                response.Response = list;
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

        #region GetNonProfits
        public async Task<ResponseModel> GetFoundationUsersPgnAsync(GetNonProfitsPgnRequestModel request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                List<ApplicationUser> userList = _userManager.Users.Where(u => u.UserRole == UserRole.Foundation.ToString() && u.UserStatusId == (int)UserStatus.Active && !u.IsDeleted).ToList();
                List<GetNonProfitsPgnResponseModel> users = new();

                foreach (var User in userList)
                {
                    string FirstName = SecurityProvider.DecryptTextAsync(User.FirstName);
                    string LastName = SecurityProvider.DecryptTextAsync(User.LastName);

                    GetNonProfitsPgnResponseModel u = new()
                    {
                        UserId = User.Id,
                        FoundationId = User.FoundationId ?? 0,
                        FoundationName = User.FoundationName,
                        FirstName = FirstName,
                        LastName = LastName,
                        FullName = (FirstName + " " + LastName).Trim(),
                        UserName = User.UserName,
                        CoverPictureUrl = ResourcePath.GetUrl(ResourceType.UserCoverImage, User.CoverPictureUrl),
                        CoverPictureThumbUrl = ResourcePath.GetThumbUrl(ResourceType.UserCoverImage, User.CoverPictureUrl),
                        ProfilePictureUrl = ResourcePath.GetUrl(ResourceType.UserProfileImage, User.ProfilePictureUrl),
                        ProfilePictureThumbUrl = ResourcePath.GetThumbUrl(ResourceType.UserProfileImage, User.ProfilePictureUrl),
                        Email = SecurityProvider.DecryptTextAsync(User.Email),
                        Description = User.Description,
                        UserRole = User.UserRole,
                        AgeRange = User.AgeRange.ToString(),
                        PhoneNumber = string.IsNullOrEmpty(User.PhoneNumber) ? null : SecurityProvider.DecryptTextAsync(User.PhoneNumber),
                        UserStatus = User.UserStatus.ToString(),
                        IsLoggedIn = User.IsLoggedIn,
                        LastLoginDate = User.LastLoginDate,
                        CreatedDate = User.CreatedDate,
                        TotalRecords = userList.Count
                    };
                    users.Add(u);
                }

                List<GetNonProfitsPgnResponseModel> list = users.OrderBy(s => s.FullName).ToList();

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    string searchText = request.SearchText.ToLower();
                    list = list.FindAll(s => (!string.IsNullOrEmpty(s.FoundationName) && s.FoundationName.ToLower().Contains(searchText)) || s.UserName.ToLower().Contains(searchText));
                    list.ForEach(s => s.TotalRecords = list.Count);
                }

                if (!request.GetAll)
                {
                    int skip = request.PageSize * (request.PageNumber - 1);
                    int take = request.PageSize;
                    list = list.Skip(skip).Take(take).ToList();
                }

                // Return Success Response
                response.IsSuccess = true;
                response.Message = $"Total {list.Count} {(list.Count > 1 ? "users" : "user")} found.";
                response.Response = list;
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

        #region Create User
        public async Task<ResponseModel> CreateUser(CreateAdminUserRequestModel model)
        {
            ResponseModel response = new();
            try
            {
                if (model.RoleName.Count == 0)
                {
                    response.Message = "Insert failed : Atleast 1 role must be selected.";
                    response.IsSuccess = false;
                    return response;
                }

                if (string.IsNullOrEmpty(model.OfficePhoneNumber))
                {
                    response.Message = "Field Required : Office Phone Number.";
                    response.IsSuccess = false;
                    return response;
                }

                string FirstName = model.FirstName;
                string LastName = model.LastName;
                string Email = model.Email;
                string PhoneNumber = model.PhoneNumber;
                string UserName = model.Username;
                string Password = model.Password;
                string ConfirmPassword = model.ConfirmPassword;

                List<IdentityError> Errors = new();

                if (string.IsNullOrEmpty(FirstName))
                {
                    response.Message = "Field Required : First Name.";
                    response.IsSuccess = false;
                    return response;
                }

                if (string.IsNullOrEmpty(LastName))
                {
                    response.Message = "Field Required : Last Name.";
                    response.IsSuccess = false;
                    return response;
                }

                if (string.IsNullOrEmpty(Email))
                {
                    response.IsSuccess = false;
                    response.Message = "Email cannot be empty.";
                    return response;
                }
                else
                {
                    // Get Encrypted value
                    string EncryptedEmail = SecurityProvider.EncryptTextAsync(Email);

                    // Check whether user email already exists
                    //ApplicationUser UserByEmail = await _userManager.FindByEmailAsync(EncryptedEmail);
                    var UserByEmail = _applicationDbContext.Users.Where(x => x.Email == EncryptedEmail && x.IsDeleted == false).Select(x => x).ToList();
                    if (UserByEmail.Count != 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Email already registered!";
                        return response;
                    }
                }

                if (string.IsNullOrEmpty(Password))
                {
                    response.IsSuccess = false;
                    response.Message = "The password cannot be empty.";
                    return response;
                }

                if (Password != ConfirmPassword)
                {
                    response.IsSuccess = false;
                    response.Message = "The password and confirmation password do not match.";
                    return response;
                }

                // Check if any error found
                if (Errors.Any())
                {
                    // Yes! error found. Return with error descriptions.
                    response.Message = "Validation failed!";
                    response.IsSuccess = false;
                    response.Errors = Errors.Select(e => e.Description).ToList();
                    return response;
                }

                var ProfilePictureResponse = await _commonService.UploadProfileImageAsync(model.ProfilePictureFile);

                string purchaseQuote = model.PurchaseQuota;
                string pQuoteData = "";
                if (purchaseQuote != null)
                {
                    decimal p = decimal.Parse(purchaseQuote.Replace("-", "").Replace("(", "").Replace(")", ""));
                    pQuoteData = p.ToString(".00");
                }
                else
                {
                    pQuoteData = null;
                }

                string salesQuote = model.SaleQuota;
                string sQuoteData = "";
                if (salesQuote != null)
                {
                    decimal p = decimal.Parse(salesQuote.Replace("-", "").Replace("(", "").Replace(")", ""));
                    sQuoteData = p.ToString(".00");
                }
                else
                {
                    sQuoteData = null;
                }

                ApplicationUser User = new()
                {
                    Title = string.IsNullOrEmpty(model.Title) ? "" : model.Title,
                    FirstName = SecurityProvider.EncryptTextAsync(FirstName),
                    LastName = SecurityProvider.EncryptTextAsync(LastName),
                    Email = SecurityProvider.EncryptTextAsync(Email),
                    PhoneNumber = string.IsNullOrEmpty(PhoneNumber) ? "" : SecurityProvider.EncryptTextAsync(PhoneNumber),
                    UserName = UserName,
                    SuiteApt = model.SuiteApt,
                    CoverPictureUrl = ProfilePictureResponse,
                    ZipCode = model.ZipCode,
                    City = string.IsNullOrEmpty(model.City) ? "" : model.City,
                    Country = string.IsNullOrEmpty(model.Country) ? "" : model.Country,
                    State = string.IsNullOrEmpty(model.State) ? "" : model.State,
                    AgeRangeId = (int)AgeRange.NotRequired,
                    EmailConfirmed = true,
                    IsApproved = true,
                    IsDeleted = false,
                    IsLoggedIn = false,
                    SaleQuota = ((sQuoteData != null) ? (Convert.ToDecimal(sQuoteData)) : 0),
                    SaleCommissionRate = model.SaleCommissionRate,
                    IsProfitBasedCommission = model.IsProfitBasedCommission,
                    PurchaseQuota = ((pQuoteData != null) ? (Convert.ToDecimal(pQuoteData)) : 0),
                    PurchaseCommissionRate = model.PurchaseCommissionRate,
                    UserStatusId = (int)UserStatus.Active,
                    CreatedDate = DateTime.Now,
                    EmergencyContactName = string.IsNullOrEmpty(model.EmergencyContactName) ? "" : model.EmergencyContactName,
                    EmergencyContactNumber = string.IsNullOrEmpty(model.EmergencyContactNumber) ? "" : model.EmergencyContactNumber,
                    OfficePhoneNumber = model.OfficePhoneNumber,
                    ImageZoomRatio = model.ImageZoomRatio,
                    DirectPhoneNumber = string.IsNullOrEmpty(model.DirectPhoneNumber) ? "" : model.DirectPhoneNumber,
                    NumberExtension = string.IsNullOrEmpty(model.NumberExtension) ? "" : model.NumberExtension
                };

                IdentityResult userResult = await _userManager.CreateAsync(User, model.Password);
                if (userResult.Errors.Any())
                {
                    response.Message = "Registration failed!" + userResult.Errors.Select(z => z.Description).FirstOrDefault();
                    response.IsSuccess = false;
                    return response;
                }
                IdentityResult roleResult = null;
                if (userResult != null)
                {
                    foreach (var role in model.RoleName)
                    {
                        roleResult = await _userManager.AddToRoleAsync(User, role);
                    }
                }
                if (roleResult.Errors.Any())
                {
                    // User Manager action failed
                    //AddErrors(roleResult);
                    response.Message = "Registration failed!!";
                    response.IsSuccess = false;
                    response.Errors = roleResult.Errors.Select(e => e.Description).ToList();
                    return response;
                }

                // Get User details
                ApplicationUser NewUser = await _userManager.FindByNameAsync(UserName);
                if (NewUser is null)
                {
                    response.IsSuccess = false;
                    response.Message = "Registration failed.";
                    return response;
                }

                var roles = await _userManager.GetRolesAsync(User);
                List<RequestAllUsersRolesModel> rolelist = new();
                foreach (var item in roles)
                {
                    List<RequestAllUsersRolesModel> userByRoleIdDataAsync = await _commonSPServices.GetRole(item);
                    var data = userByRoleIdDataAsync.FirstOrDefault();
                    RequestAllUsersRolesModel r = new()
                    {
                        RoleName = data.RoleName,
                        RoleColor = data.RoleColor
                    };
                    rolelist.Add(r);
                }

                string datedata = "";
                DateTime dat = DateTime.UtcNow;
                datedata = dat.ToString("dd/MM/yyyy");
                // Return Success Response
                CreateAdminUserResponseModel responseModel = new()
                {
                    Id = User.Id,
                    Title = User.Title,
                    FullName = FirstName + " " + LastName,
                    FirstName = SecurityProvider.DecryptTextAsync(FirstName),
                    LastName = SecurityProvider.DecryptTextAsync(LastName),
                    PhoneNumber = SecurityProvider.DecryptTextAsync(PhoneNumber),
                    Email = SecurityProvider.DecryptTextAsync(User.Email),
                    Description = User.Description,
                    Street = User.Street,
                    City = User.City,
                    State = User.State,
                    Country = User.Country,
                    ZipCode = User.ZipCode,
                    SaleQuota = Convert.ToDecimal(sQuoteData),
                    SaleCommissionRate = User.SaleCommissionRate,
                    IsProfitBasedCommission = User.IsProfitBasedCommission,
                    PurchaseQuota = User.PurchaseQuota,
                    PurchaseCommissionRate = User.PurchaseCommissionRate,
                    SuiteApt = User.SuiteApt,
                    UserStatus = Convert.ToBoolean(User.UserStatusId),
                    UserRole = User.UserRole,
                    ProfilePictureFile = ProfilePictureResponse,
                    EmergencyContactName = User.EmergencyContactName,
                    EmergencyContactNumber = User.EmergencyContactNumber,
                    OfficePhoneNumber = User.OfficePhoneNumber,
                    CoverPictureUrl = model.ProfilePictureFile,
                    CreatedDate = datedata,
                    IsChecked = false,
                    IsExpandable = false,
                    ImageZoomRatio = User.ImageZoomRatio,
                    DirectPhoneNumber = User.DirectPhoneNumber,
                    NumberExtension = User.NumberExtension,
                    RoleName = rolelist.Select(x => new RequestAllUsersRolesModel { RoleName = x.RoleName, RoleColor = x.RoleColor }).ToList()
                };
                response.IsSuccess = true;
                response.Message = "User is registered successfully!";
                response.Response = responseModel;
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

        #region Update User
        public async Task<ResponseModel> UpdateUser(UpdateAdminUserRequestModel model)
        {
            ResponseModel response = new();
            try
            {
                List<IdentityError> Errors = new();
                string Email = model.Email;

                if (model.RoleName.Count == 0)
                {
                    response.Message = "Update failed : Atleast 1 role must be selected.";
                    response.IsSuccess = false;
                    return response;
                }

                if (string.IsNullOrEmpty(model.OfficePhoneNumber))
                {
                    response.Message = "Field Required : Office Phone Number.";
                    response.IsSuccess = false;
                    return response;
                }
                // Check if User exists
                var User = _userManager.Users.Where(u => u.Id == model.UserId).FirstOrDefault();
                if (User is null)
                {
                    response.Message = "No user found";
                    response.IsSuccess = false;
                    return response;
                }

                // FirstName
                string FirstName = model.FirstName;
                if (!string.IsNullOrEmpty(FirstName))
                {
                    User.FirstName = SecurityProvider.EncryptTextAsync(FirstName);
                }
                else
                {
                    response.Message = "Field Required : First Name.";
                    response.IsSuccess = false;
                    return response;
                }

                // LastName
                string LastName = model.LastName;
                if (!string.IsNullOrEmpty(LastName))
                {
                    User.LastName = SecurityProvider.EncryptTextAsync(LastName);
                }
                else
                {
                    response.Message = "Field Required : Last Name.";
                    response.IsSuccess = false;
                    return response;
                }

                // PhoneNumber
                string PhoneNumber = model.PhoneNumber;
                if (!string.IsNullOrEmpty(PhoneNumber))
                {
                    User.PhoneNumber = SecurityProvider.EncryptTextAsync(PhoneNumber);
                }

                var Roles = await _userManager.GetRolesAsync(User);

                string EncryptedEmail = SecurityProvider.EncryptTextAsync(Email);
                string DecryptedEmail = SecurityProvider.DecryptTextAsync(User.Email);
                if (string.IsNullOrEmpty(Email) && DecryptedEmail != model.Email)
                {
                    response.Message = "Email cannot be empty.";
                    response.IsSuccess = false;
                    return response;
                }

                string purchaseQuote = model.PurchaseQuota;
                decimal p = decimal.Parse(purchaseQuote.Replace("-", "").Replace("(", "").Replace(")", ""));
                string pQuoteData = p.ToString("0.00");

                string salesQuote = model.SaleQuota;
                decimal s = decimal.Parse(salesQuote.Replace("-", "").Replace("(", "").Replace(")", ""));
                string sQuoteData = s.ToString("0.00");

                var validationresponse = await CheckEditValidation(model, sQuoteData, pQuoteData, User);
                if (validationresponse.IsSuccess == false)
                {
                    return validationresponse;
                }

                var ProfilePictureResponse = await _commonService.UploadProfileImageAsync(model.ProfilePictureFile);

                // get only limited chars of Description
                string Description = model.Description;
                User.Description = string.IsNullOrEmpty(Description) || Description.Length <= 250 ? Description : Description.Substring(0, 250); // first 250 chars
                User.UpdatedDate = DateTime.UtcNow;
                User.SuiteApt = model.SuiteApt;
                User.ZipCode = model.ZipCode;
                User.City = string.IsNullOrEmpty(model.City) ? "" : model.City;
                User.Title = string.IsNullOrEmpty(model.Title) ? "" : model.Title;
                User.Description = model.Description;
                User.Street = string.IsNullOrEmpty(model.Street) ? "" : model.Street;
                User.Country = string.IsNullOrEmpty(model.Country) ? "" : model.Country;
                User.UserName = model.Email;
                User.CoverPictureUrl = ProfilePictureResponse;
                User.State = string.IsNullOrEmpty(model.State) ? "" : model.State;
                User.Email = SecurityProvider.EncryptTextAsync(model.Email);
                User.SaleQuota = Convert.ToDecimal(sQuoteData);
                User.SaleCommissionRate = model.SaleCommissionRate;
                User.IsProfitBasedCommission = model.IsProfitBasedCommission;
                User.PurchaseQuota = Convert.ToDecimal(pQuoteData);
                User.PurchaseCommissionRate = model.PurchaseCommissionRate;
                User.UserStatusId = model.UserStatus == true ? 1 : 0;
                User.EmergencyContactName = string.IsNullOrEmpty(model.EmergencyContactName) ? "" : model.EmergencyContactName;
                User.EmergencyContactNumber = string.IsNullOrEmpty(model.EmergencyContactNumber) ? "" : model.EmergencyContactNumber;
                User.OfficePhoneNumber = model.OfficePhoneNumber;
                User.ImageZoomRatio = model.ImageZoomRatio;
                User.DirectPhoneNumber = string.IsNullOrEmpty(model.DirectPhoneNumber) ? "" : model.DirectPhoneNumber;
                User.NumberExtension = string.IsNullOrEmpty(model.NumberExtension) ? "" : model.NumberExtension;
                IdentityResult userResult = await _userManager.UpdateAsync(User);
                if (userResult.Errors.Any())
                {
                    response.Message = userResult.Errors.Select(z => z.Description).FirstOrDefault();
                    response.IsSuccess = false;
                    return response;
                }
                IdentityResult roleResult = null;

                if (userResult != null)
                {
                    var existingRoles = await _userManager.GetRolesAsync(User);
                    foreach (var rol in existingRoles)
                    {
                        //Remove existing roles
                        var result2 = await _userManager.RemoveFromRoleAsync(User, rol);
                    }
                    foreach (var role in model.RoleName)
                    {
                        roleResult = await _userManager.AddToRoleAsync(User, role);
                    }
                }

                if (userResult.Errors.Any())
                {
                    // User Manager action failed
                    response.Message = "Something went wrong. User profile update failed!";
                    response.Errors = userResult.Errors.Select(e => e.Description).ToList();
                    response.IsSuccess = false;
                    return response;
                }

                if (Errors.Any())
                {
                    // Yes! error found. Return with error descriptions.
                    response.Message = "Validation failed!";
                    response.IsSuccess = false;
                    response.Errors = Errors.Select(e => e.Description).ToList();
                    return response;
                }

                var roles = await _userManager.GetRolesAsync(User);
                List<RequestAllUsersRolesModel> rolelist = new();
                foreach (var item in roles)
                {
                    List<RequestAllUsersRolesModel> userByRoleIdDataAsync = await _commonSPServices.GetRole(item);
                    var data = userByRoleIdDataAsync.FirstOrDefault();
                    RequestAllUsersRolesModel r = new()
                    {
                        RoleName = data.RoleName,
                        RoleColor = data.RoleColor
                    };
                    rolelist.Add(r);
                }

                string datedata = "";
                DateTime dat = User.CreatedDate;
                datedata = dat.ToString("dd/MM/yyyy");

                // Return Success Response
                UpdateAdminUserResponseModel responseModel = new()
                {
                    Id = User.Id,
                    Title = User.Title,
                    FullName = model.FirstName + " " + model.LastName,
                    FirstName = SecurityProvider.DecryptTextAsync(FirstName),
                    LastName = SecurityProvider.DecryptTextAsync(LastName),
                    PhoneNumber = SecurityProvider.DecryptTextAsync(PhoneNumber),
                    Email = SecurityProvider.DecryptTextAsync(User.Email),
                    Description = User.Description,
                    Street = User.Street,
                    City = User.City,
                    State = User.State,
                    Country = User.Country,
                    ZipCode = User.ZipCode,
                    SaleQuota = Convert.ToString(User.SaleQuota).Replace(".00", ""),
                    SaleCommissionRate = User.SaleCommissionRate,
                    IsProfitBasedCommission = User.IsProfitBasedCommission,
                    PurchaseQuota = Convert.ToString(User.PurchaseQuota).Replace(".00", ""),
                    PurchaseCommissionRate = User.PurchaseCommissionRate,
                    SuiteApt = User.SuiteApt,
                    UserStatus = Convert.ToBoolean(User.UserStatusId),
                    //UserRole = User.UserRole,
                    //ProfilePictureFile = model.ProfilePictureFile,
                    EmergencyContactName = User.EmergencyContactName,
                    EmergencyContactNumber = User.EmergencyContactNumber,
                    OfficePhoneNumber = User.OfficePhoneNumber,
                    IsChecked = model.IsChecked,
                    IsExpandable = model.IsExpandable,
                    CoverPictureUrl = model.ProfilePictureFile,
                    CreatedDate = datedata,
                    RoleName = rolelist.Select(x => new RequestAllUsersRolesModel { RoleName = x.RoleName, RoleColor = x.RoleColor }).ToList(),
                    ImageZoomRatio = User.ImageZoomRatio,
                    DirectPhoneNumber = User.DirectPhoneNumber,
                    NumberExtension = User.NumberExtension
                };

                response.IsSuccess = true;
                response.Message = "User is updated successful!";
                response.Response = responseModel;
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

        #region Update user status
        public async Task<ResponseModel> UpdateUserStatus(UpdateUserStatusRequest model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var User = _userManager.Users.Where(u => u.Id == model.UserId).FirstOrDefault();
                if (User is null)
                {
                    response.Message = "No user found";
                    return response;
                }

                User.UserStatusId = model.UserStatus == true ? 1 : 0;
                IdentityResult userResult = await _userManager.UpdateAsync(User);

                if (userResult.Errors.Any())
                {
                    // User Manager action failedv
                    response.Message = "Something went wrong. User status update failed!";
                    response.Errors = userResult.Errors.Select(e => e.Description).ToList();
                    return response;
                }

                // Return Success Response
                UpdateUserResponseModel responseModel = new()
                {
                    UserId = User.Id
                };

                response.IsSuccess = true;
                response.Message = "User Status is updated successfully!";
                response.Response = responseModel;
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

        #region Delete user
        public async Task<ResponseModel> DeleteUser(DeleteUserModel deleteUserModel)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var user = _userManager.Users.Where(u => u.Id == deleteUserModel.Id).FirstOrDefault();
                var Roles = await _userManager.GetRolesAsync(user);
                List<IdentityError> Errors = new();
                if (user == null)
                {
                    response.Message = "No user found";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    List<CompanyModel> companyData = await _commonService.GetCompaniesData(deleteUserModel.Id);
                    foreach (var company in companyData)
                    {
                        if (company.SalesPerson1UserId == deleteUserModel.Id)
                        {
                            Errors.Add(new() { Code = "User", Description = "User already in use." });
                            break;
                        }
                        if (company.SalesPerson2UserId == deleteUserModel.Id)
                        {
                            Errors.Add(new() { Code = "User", Description = "User already in use." });
                            break;
                        }
                        if (company.Buyer1UserId == deleteUserModel.Id)
                        {
                            Errors.Add(new() { Code = "User", Description = "User already in use." });
                            break;
                        }
                        if (company.Buyer2UserId == deleteUserModel.Id)
                        {
                            Errors.Add(new() { Code = "User", Description = "User already in use." });
                            break;
                        }
                        if (company.BuyersRep == deleteUserModel.Id)
                        {
                            Errors.Add(new() { Code = "User", Description = "User already in use." });
                            break;
                        }
                    }
                    if (Errors.Any())
                    {
                        response.Message = Errors.Select(e => e.Description).FirstOrDefault();
                        response.IsSuccess = false;
                        return response;
                    }

                    user.DeletedDate = DateTime.Now;
                    user.IsDeleted = true;
                    IdentityResult userResult = await _userManager.UpdateAsync(user);

                    var existingRoles = await _userManager.GetRolesAsync(user);
                    //Remove existing roles
                    foreach (var rol in existingRoles)
                    {
                        var result2 = await _userManager.RemoveFromRoleAsync(user, rol);
                    }
                    response.Message = "User Deleted Successful.";
                    response.IsSuccess = true;
                    response.Response = deleteUserModel;
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

        #region CheckEditValidation
        private async Task<ResponseModel> CheckEditValidation(UpdateAdminUserRequestModel model, string sQuoteData, string pQuoteData, ApplicationUser User)
        {
            List<IdentityError> Errors = new();
            ResponseModel response = new();
            if (model.Title != User.Title)
            {
                response.IsSuccess = false;
                response.Message = "Update Failed: Title cannot be updated.";
                return response;
            }
            if (model.City != User.City)
            {
                response.IsSuccess = false;
                response.Message = "Update Failed: City cannot be updated.";
                return response;
            }
            if (model.OfficePhoneNumber != User.OfficePhoneNumber)
            {
                response.IsSuccess = false;
                response.Message = "Update Failed: PhoneNumber cannot be updated.";
                return response;
            }
            if (pQuoteData != Convert.ToString(User.PurchaseQuota))
            {
                response.IsSuccess = false;
                response.Message = "Update Failed: PurchaseQuota cannot be updated.";
                return response;
            }
            if (sQuoteData != Convert.ToString(User.SaleQuota))
            {
                response.IsSuccess = false;
                response.Message = "Update Failed: SaleQuota cannot be updated.";
                return response;
            }
            response.IsSuccess = true;
            return response;
        }
        #endregion

        #region SaveUserSetting
        public async Task<ResponseModel> SaveUserSetting(SaveUserSettingCommand command)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var user = await _userManager.FindByIdAsync(command.UserId);
                user.UserSetting = command.UserSetting.ToString();
                var result = await _userManager.UpdateAsync(user);
                response.Message = "User Settings Updated Successfully.";
                response.IsSuccess = true;
                response.Response = command.UserSetting;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }
        #endregion

        #region GetUserSetting
        public async Task<ResponseModel> GetUserSetting(string UserId)
        {
            ResponseModel response = new();
            try
            {
                GetUserSettingModel settingModel = new();
                var getData = _applicationDbContext.Users.Where(x => x.Id == UserId).Select(x => x.UserSetting).ToList();
                string data = getData.FirstOrDefault()?.ToString();
                if (data == "" || data == null)
                {
                    response.Message = "No User Settings Found!!!";
                    response.IsSuccess = false;
                    return response;
                }
                settingModel.UserSetting = getData.FirstOrDefault();
                response.Message = "Successful!!!";
                response.IsSuccess = true;
                response.Response = settingModel;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        #region AddAssignSalesAndPurchaseCommission
        public async Task<ResponseModel> AddAssignSalesAndPurchaseCommission(AddAssignSalesAndPurchaseCommissionCommand request)
        {
            ResponseModel response = new();
            try
            {
                if (!String.IsNullOrEmpty(request.SaleCommissionId))
                {
                    var assignSaleCommission = await _commonSPServices.SetSaleCommission(request.SaleCommissionId, request.UserIds);
                }
                if (!String.IsNullOrEmpty(request.PurchaseCommissionId))
                {
                    var assignPurchaseCommission = await _commonSPServices.SetPurchaseCommission(request.PurchaseCommissionId, request.UserIds);
                }
                response.IsSuccess = true;
                response.Message = "Commission Role Assigned";
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

        #region Get User Profile Details By Id
        public async Task<ResponseModel> GetUserProfileDetailsById(RequestAccountModel request)
        {
            ResponseModel response = new();
            try
            {
                if (!string.IsNullOrEmpty(request.Id))
                {
                    var query = @"exec [sp_GetAllUsers] @Id = '" + request.Id + "' ";
                    var Data = await _applicationDbContext.getUserProfileDetailsModels.FromSqlRaw(query)!.ToListAsync();

                    List<RequestAllUsersRolesModel> userByRoleIdDataAsync = await _commonSPServices.GetRoleById(request);
                    Func<string, string> phonePlusExt = s => string.IsNullOrEmpty(s) ? string.Empty : string.Format("{0},", s);
                    string FirstName = SecurityProvider.DecryptTextAsync(Data[0].FirstName);
                    string LastName = SecurityProvider.DecryptTextAsync(Data[0].LastName);
                    string PhoneNumber = string.IsNullOrEmpty(Data[0].PhoneNumber) ? "" : SecurityProvider.DecryptTextAsync(Data[0].PhoneNumber);
                    GetUserProfileDetailsModel UserProfileDetails = new()
                    {
                        Id = Data[0].Id,
                        FirstName = FirstName,
                        LastName = LastName,
                        FullName = (FirstName + " " + LastName).Trim(),
                        CoverPictureUrl = await _commonService.GetImageAsBase64Url(ResourcePath.GetUrl(ResourceType.UserCoverImage, Data[0].CoverPictureUrl)),
                        Title = Data[0].Title,
                        City = Data[0].City,
                        Email = SecurityProvider.DecryptTextAsync(Data[0].Email),
                        OfficePhoneNumber = Data[0].OfficePhoneNumber,
                        DirectPhoneNumber = Data[0].DirectPhoneNumber,
                        NumberExtension = Data[0].NumberExtension,
                        PhoneNumber = PhoneNumber,
                        EmergencyContactName = Data[0].EmergencyContactName,
                        EmergencyContactNumber = Data[0].EmergencyContactNumber,
                        SalesCommissionRate = Data[0].SalesCommissionRate,
                        PurchaseCommissionRate = Data[0].PurchaseCommissionRate,
                        SaleQuota = string.Concat(Data[0].SaleQuota.Replace(".00", "")),
                        PurchaseQuota = string.Concat(Data[0].PurchaseQuota.Replace(".00", "")),
                        OfficePhoneNumberPlusExt = (string.Format("{0}{1}", phonePlusExt(Data[0].OfficePhoneNumber), phonePlusExt(Data[0].NumberExtension).Trim(','))),
                        SaleCommissionRoleName = string.IsNullOrEmpty(Data[0].SalesCommissionRate) ? "" : (await _httpApiRequests.GetAllSaleCommissionRole()).Where(x => x.Id == Data[0].SalesCommissionRate).Select(x => x.Name).FirstOrDefault(),
                        PurchaseCommissionRoleName = string.IsNullOrEmpty(Data[0].PurchaseCommissionRate) ? "" : (await _httpApiRequests.GetAllPurchaseCommissionRole()).Where(x => x.Id == Data[0].PurchaseCommissionRate).Select(x => x.Name).FirstOrDefault(),
                    };
                    UserProfileDetails.RoleName.AddRange(userByRoleIdDataAsync.Where(x => x.UserId == UserProfileDetails.Id).Select(x => new RequestAllUsersRolesModel { RoleName = x.RoleName, RoleColor = x.RoleColor }));
                    userByRoleIdDataAsync.Select(x => x.RoleName).Where(y => y.Equals(UserProfileDetails.Id));
                    response.IsSuccess = true;
                    response.Response = UserProfileDetails;
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Id is required";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        #endregion
    }
}