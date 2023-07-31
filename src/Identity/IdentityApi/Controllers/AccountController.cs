
using IdentityApi.Services.Accounts;

namespace IdentityApi.Controllers
{
    public class AccountController : BaseController
    {
        #region feilds
        private readonly ILoginService<ApplicationUser> _loginService;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IHttpApiRequests _httpApiRequests;
        private readonly ICommonService _commonService;
        private readonly IAccountService _accountService;

        private readonly ApplicationDbContext _applicationDbContext;
        #endregion

        #region ctor
        public AccountController(ILoginService<ApplicationUser> loginService,
                                 IIdentityServerInteractionService interaction,
                                 IClientStore clientStore,
                                 ILogger<AccountController> logger,
                                 UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IConfiguration configuration,
                                 IEmailService emailService,
                                 IHttpApiRequests httpApiRequests,
                                 ICommonService commonService,
                                 ApplicationDbContext applicationDbContext,
                                 IAccountService accountService)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _interaction = interaction ?? throw new ArgumentNullException(nameof(interaction));
            _clientStore = clientStore ?? throw new ArgumentNullException(nameof(clientStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _httpApiRequests = httpApiRequests ?? throw new ArgumentNullException(nameof(httpApiRequests));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }
        #endregion

        #region Register User
        /// <summary>DONE
        /// Route: api/v1/identityapi/account/register
        /// Register : create user after validation by using "validateuser API"
        /// </summary>

        [HttpPost()]
        [Route("Register")]
        [SwaggerOperation(Summary = "create user after validation by using 'validateuser API' ", Description = "")]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            ResponseModel response = new();
            try
            {
                var data = await _accountService.Register(model);
                return Ok(data);
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;
                return BadRequest(response);
            }
        }
        #endregion

        #region Validate User
        /// <summary>DONE
        /// Route: api/v1/identityapi/account/register/validate
        /// Register : validate user
        /// </summary>

        [HttpPost()]
        [Route("Register/Validate")]
        [SwaggerOperation(Summary = "validate user", Description = "")]
        public async Task<ActionResult<ResponseModel>> ValidateUser(RegisterViewModel model)
        {
            return await _accountService.ValidateUser(model);
        }
        #endregion

        #region Profile Update
        /// <summary>
        /// Route: api/v1/identityapi/account/profile/update
        /// profile : update account details for an user
        /// </summary>

        [HttpPost()]
        [Route("profile/update")]
        [SwaggerOperation(Summary = " update account details for an user", Description = "")]
        public async Task<ActionResult<ResponseModel>> UpdateProfile(UpdateProfileModel model)
        {
            return await _accountService.UpdateProfile(model);
        }
        #endregion

        #region Register/ResendOTP
        /// <summary>DONE
        /// Route: api/v1/identityapi/account/register/resendotp
        /// Register : Resend OTP through email
        /// </summary>

        [HttpPost()]
        [Route("Register/ResendOtp")]
        [SwaggerOperation(Summary = "Resend OTP through email", Description = "")]
        public async Task<ActionResult<ResponseModel>> ResendOTP(ResendOTPViewModel model)
        {
            return await _accountService.ResendOTP(model);
        }
        #endregion

        #region Login
        /// <summary>DONE
        /// POST: api/v1/identityapi/account/admin/login
        /// Login : login with username and password
        /// For: Admin-Web
        /// </summary>

        [HttpPost()]
        [Route("admin/login")]
        [SwaggerOperation(Summary = "Login with username and password - For Admin-Web", Description = "")]
        public async Task<ActionResult<ResponseModel>> WebAdminLogin(LoginViewModel request)
        {
            return await _accountService.WebAdminLogin(request);
        }
        #endregion

        #region Forgot Password
        /// <summary>DONE
        /// Route: api/v1/identityapi/account/forgotpassword
        /// Forgot Password : Send OTP and Get Reset-Password-Token
        /// </summary>

        [HttpPost()]
        [Route("ForgotPassword")]
        [SwaggerOperation(Summary = "Forgot Password : Send OTP and Get Reset-Password-Token", Description = "")]
        public async Task<ActionResult<ResponseModel>> ForgotPassword(ForgotPasswordViewModel model)
        {
            return await _accountService.ForgotPassword(model);
        }
        #endregion

        #region Reset Password
        /// <summary>DONE
        /// Route: api/v1/identityapi/account/resetpassword
        /// Forgot Password : Reset Password with verify Reset-Password-Token
        /// Handle postback for api/v1/account/forgotpassword
        /// </summary>

        [HttpPost()]
        [Route("ResetPassword")]
        [ProducesDefaultResponseType]
        [SwaggerOperation(Summary = "Reset Password with verify Reset-Password-Token", Description = "")]
        public async Task<ActionResult<ResponseModel>> ResetPassword(ResetPasswordViewModel model)
        {
            return await _accountService.ResetPassword(model);
        }
        #endregion

        #region VerifyEmail
        /// <summary>DONE
        /// Route: api/v1/identityapi/account/verifyemail
        /// Verify Email : Send OTP through email
        /// </summary>

        [HttpPost()]
        [Route("VerifyEmail")]
        [ProducesDefaultResponseType]
        [SwaggerOperation(Summary = "Send OTP through email", Description = "")]
        public async Task<ActionResult<ResponseModel>> VerifyEmail(VerifyEmailViewModel model)
        {
            return await _accountService.VerifyEmail(model);
        }
        #endregion

        #region ForgotUserName
        /// <summary>DONE
        /// Route: api/v1/identityapi/account/forgotusername
        /// Forgot Username : Send OTP on email to validate user
        /// </summary>

        [HttpPost()]
        [Route("ForgotUsername")]
        [SwaggerOperation(Summary = "Send OTP on email to validate user", Description = "")]
        public async Task<ActionResult<ResponseModel>> ForgotUsername(ForgotUsernameViewModel model)
        {
            return await _accountService.ForgotUsername(model);
        }
        #endregion

        #region RecoverUserName
        /// <summary>DONE
        /// Route: api/v1/identityapi/account/recoverusername
        /// Forgot Username : Send username through email
        /// </summary>

        [HttpPost()]
        [Route("RecoverUsername")]
        [ProducesDefaultResponseType]
        [SwaggerOperation(Summary = "Send username through email", Description = "")]
        public async Task<ActionResult<ResponseModel>> RecoverUsername(ForgotUsernameViewModel model)
        {
            return await _accountService.RecoverUsername(model);
        }
        #endregion

        #region Change Password
        /// <summary>
        /// Route: api/v1/identityapi/account/changepassword
        /// Change Password : User must be logged in
        /// </summary>

        [HttpPost()]
        [Route("ChangePassword")]
        [SwaggerOperation(Summary = "User be logged-in to change password.", Description = "")]
        public async Task<ActionResult<ResponseModel>> ChangePassword(ChangePasswordViewModel model)
        {
            return await _accountService.ChangePassword(model);
        }
        #endregion

        #region Admin Reset Password
        ///<summary>
        /// Admin ResetPassword
        /// </summary>

        [HttpPost()]
        [Route("AdminRestPassword")]
        [SwaggerOperation(Summary = "Admin Password Reset", Description = "")]
        public async Task<ActionResult<ResponseModel>> AdminRestPassword(ForgotPasswordViewModel model)
        {
            return await _accountService.AdminRestPassword(model);
        }
        #endregion

        #region Logout
        ///<summary>
        /// Logout 
        /// </summary>

        [HttpGet()]
        [Route("logout")]
        [SwaggerOperation(Summary = "Logout", Description = "")]
        public async Task<IActionResult> Logout()
        {
            ResponseModel response = new();
            try
            {
                if (ModelState.IsValid)
                {
                    HttpClient _httpClient = new();
                    string token = Request.Headers["Authorization"].ToString();

                    string tokenn = token[7..];

                    // Get Identity Server Base Url
                    string IdentityServerBaseUrl = AppConstants.BaseUrl.IdentityAPI;

                    // Revoke Token
                    var tokenResponse = await _httpClient.RevokeTokenAsync(new TokenRevocationRequest
                    {
                        Address = IdentityServerBaseUrl + "/connect/revocation",
                        ClientId = "SourceMatrixClient",
                        ClientSecret = "secret",
                        Token = tokenn,
                        TokenTypeHint = "access_token"
                    });

                    // revoke Token
                    //var tokenResponse = await _httpClient.GetAsync($"http://18.118.103.88:5100/connect/endsession?id_token_hint=eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ0QjgyODgwMkRBRkUxRTAxRTk2QTdBMTFFRjRBRjVEIiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2MjczMTUzMjQsImV4cCI6MTYyNzMxODkyNCwiaXNzIjoibnVsbCIsImNsaWVudF9pZCI6InhhbWFyaW5DbGllbnQiLCJzdWIiOiI2NWMyOWFmYS00NGM5LTRiODYtYmRkNi02YjBlZTcxZDAzYjEiLCJhdXRoX3RpbWUiOjE2MjczMTUzMjQsImlkcCI6ImxvY2FsIiwicHJlZmVycmVkX3VzZXJuYW1lIjoidXNlckBrYXJtYWd5LmNvbSIsInVuaXF1ZV9uYW1lIjoidXNlckBrYXJtYWd5LmNvbSIsIlVzZXJJZCI6IjY1YzI5YWZhLTQ0YzktNGI4Ni1iZGQ2LTZiMGVlNzFkMDNiMSIsImZpcnN0X25hbWUiOiJEZW1vIiwibGFzdF9uYW1lIjoiVXNlciIsImFkZHJlc3NfY2l0eSI6IlJlZG1vbmQiLCJhZGRyZXNzX2NvdW50cnkiOiJVUyIsImFkZHJlc3Nfc3RhdGUiOiJXQSIsImFkZHJlc3Nfc3RyZWV0IjoiMTU3MDMgTkUgNjFzdCBDdCIsImFkZHJlc3NfemlwX2NvZGUiOiI5ODA1MiIsImVtYWlsIjoidXNlckBrYXJtYWd5LmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwicGhvbmVfbnVtYmVyIjoiMTIzNDU2Nzg5MCIsInBob25lX251bWJlcl92ZXJpZmllZCI6ZmFsc2UsImp0aSI6IjNBNTA4QjYxNUZGRUUwNzlCQTNDOUExRDU1NDdDNDIzIiwiaWF0IjoxNjI3MzE1MzI0LCJzY29wZSI6WyJpZGVudGl0eUFQSSJdLCJhbXIiOlsicHdkIl19.S4Zvc-w7tssP5_nlxVSEXdi2Gjpu-iAVn9b9_YbJMtqeFT0ZZYUsFoAQU-rXsGmtuQdmza1m5kUcUg-_dW9VQzY_5UN9C0sVcq80vfhy1TxE-owIORaAK6pJ4M30nRCHlxbHChRm0sib7b3envxDliiKiA2KHhwijKdqUhgp_hiQk3HAWDS9ssobxUpLlkfmoJ0mtaPjWXqI0lEa1rDcbyzttBYyNqXqWcGpPhPgEIbJtMcF5JXzIyAHyqxhq-Qx-ke_1akMDG4_C5T_cBATcv8A3DpGp31rGyW2RYBbrz4KZt6RXrzXJsxLadXREhvDjuPBOXPiDG1QtxRcJ5b1SA&post_logout_redirect_uri=http://18.118.103.88:5100");

                    // check if response has any error
                    if (tokenResponse.IsError)
                    {
                        response.Message = tokenResponse.Error;
                        return BadRequest(response);
                    }

                    await _signInManager.SignOutAsync();
                    // Return Success Response
                    response.IsSuccess = true;
                    response.Message = "Logout successful!";
                    return Ok(response);
                }
                response.Message = "Bad request";
                return BadRequest(response);
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;
                return BadRequest(response);
            }
        }
        #endregion

        #region GetUsers
        /// <summary>
        /// Route: api/v1/identityapi/account/getusers
        /// Get All Active Users of Knobl : User must be logged in to use this api
        /// </summary>

        [HttpPost()]
        [Route("GetUsers")]
        [SwaggerOperation(Summary = "Get All active user : User must be loggedin to use this api", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetAllUsersAsync(RequestAccountModel request)
        {
            return await _accountService.GetAllUsersAsync(request);
        }
        #endregion

        #region ExportUser
        [HttpPost()]
        [Route("ExportGetAllUsers")]
        [SwaggerOperation(Summary = "Get All active user : User must be loggedin to use this api", Description = "")]
        public async Task<ActionResult<ResponseModel>> ExportGetAllUsers(RequestAccountModel request)
        {
            return await _accountService.ExportGetAllUsers(request);
        }
        #endregion

        #region Search/Users
        /// <summary>
        /// POST: api/v1/identityapi/account/search/users
        /// search users : User must be logged in to use this api
        /// </summary>

        [HttpPost()]
        [Route("search/users")]
        [SwaggerOperation(Summary = "search users : User must be logged in to use this api", Description = "")]
        public async Task<ActionResult<ResponseModel>> SearchByUsersPgnAsync(RequestWithPaginationModel request)
        {
            return await _accountService.SearchByUsersPgnAsync(request);
        }
        #endregion

        #region Getusers/ById
        /// <summary>
        /// Route: api/v1/identityapi/account/getuser/{id}
        /// Get Active User detail for Knobl : User must be logged in to use this api
        /// </summary>

        [HttpGet()]
        [Route("GetUser/{id}")]
        [SwaggerOperation(Summary = "Get Active User detail : User must be logged in to use this api", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetUserAsync(string id)
        {
            return await _accountService.GetUserAsync(id);
        }
        #endregion

        #region GetUserBreif/ById
        /// <summary>
        /// Route: api/v1/identityapi/account/getuserbrief/{id}
        /// Get Active User Brief detail for Knobl : User must be logged in to use this api
        /// </summary>

        [HttpGet()]
        [Route("GetUserBrief/{id}")]
        [SwaggerOperation(Summary = "Get Active User Brief detail for Knobl : User must be logged in to use this api", Description = "")]
        public async Task<ActionResult<ResponseModel>> GetUserBriefAsync(string id)
        {
            return await _accountService.GetUserBriefAsync(id);
        }
        #endregion

        #region GetManyUserBreifs
        /// <summary>
        /// Route: api/v1/identityapi/account/getmanyuserbrief
        /// Get Many Active User Brief detail for Knobl : User must be logged in to use this api
        /// </summary>

        [HttpPost()]
        [Route("GetManyUserBrief")]
        [SwaggerOperation(Summary = "Get Many Active User Brief detail for Knobl : User must be logged in to use this api", Description = "")]
        public async Task<IActionResult> GetManyUserBriefAsync([FromBody] UserIdList userIdList)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(null);
                }

                var Users = _userManager.Users.Where(u => userIdList.UserIds.Contains(u.Id)).ToList();

                if (Users.Count == 0)
                {
                    return Ok(null);
                }

                List<UserProfile> userVMList = new();

                foreach (var User in Users)
                {
                    string FirstName = SecurityProvider.DecryptTextAsync(User.FirstName);
                    string LastName = SecurityProvider.DecryptTextAsync(User.LastName);

                    UserProfile userVM = new()
                    {
                        Id = User.Id,
                        FullName = (User.UserRole == UserRole.Normal.ToString()) ? (FirstName + " " + LastName).Trim() : User.FoundationName,
                        FirstName = FirstName,
                        LastName = LastName,
                        UserName = User.UserName,
                        CoverPictureUrl = ResourcePath.GetUrl(ResourceType.UserCoverImage, User.CoverPictureUrl),
                        CoverPictureThumbUrl = ResourcePath.GetThumbUrl(ResourceType.UserCoverImage, User.CoverPictureUrl),
                        ProfilePictureUrl = ResourcePath.GetUrl(ResourceType.UserProfileImage, User.ProfilePictureUrl),
                        ProfilePictureThumbUrl = ResourcePath.GetThumbUrl(ResourceType.UserProfileImage, User.ProfilePictureUrl),
                        PhoneNumber = string.IsNullOrEmpty(User.PhoneNumber) ? null : SecurityProvider.DecryptTextAsync(User.PhoneNumber),
                        Email = SecurityProvider.DecryptTextAsync(User.Email),
                        TimeZone = User.TimeZone,
                        DeviceToken = User.DeviceToken,
                        //UserRole = User.UserRole,
                        Description = User.Description
                    };

                    userVMList.Add(userVM);
                }

                // Return Success Response
                return Ok(userVMList);
            }
            catch (Exception)
            {
                return Ok(null);
            }
        }
        #endregion

        #region Test
        ///<summary>
        ///This Api is for testing purpose.
        ///</summary>


        /// </summary>
        [HttpPost()]
        [Route("test")]
        public async Task<IActionResult> Test([FromForm] UploadImageRequestModel request)
        {
            ResponseModel response = new();
            //var ProfilePictureResponse = await _commonService.UploadProfileImageAsync(request.File);
            //// Return Success Response
            //response.IsSuccess = true;
            //response.Response = new { Filename = ProfilePictureResponse };
            response.Message = "Test successful!";
            return Ok(response);
        }
        #endregion

        #region Decrypt
        /// <summary>
        /// Route: api/v1/identityapi/account/decrypt
        /// Decrypt : get original text
        /// </summary>

        [HttpPost()]
        [Route("Decrypt")]
        [SwaggerOperation(Summary = "Decrypt : get original text", Description = "")]
        public async Task<IActionResult> DecryptTextAsync(DecryptionViewModel model)
        {
            ResponseModel response = new();

            if (model.Text is null)
            {
                response.Message = "Enter text to decrypt.";
                return BadRequest(response);
            }

            string DecryptedText = SecurityProvider.DecryptTextAsync(model.Text);

            // Return Success Response
            response.IsSuccess = true;
            response.Message = "Decryption successful!";
            response.Response = new { InputText = model.Text, DecryptedText };
            return Ok(response);
        }
        #endregion

        #region Encrypt
        /// <summary>
        /// Route: api/v1/identityapi/account/encrypt
        /// encrypt : get encrypted text
        /// </summary>

        [HttpPost()]
        [Route("Encrypt")]
        [SwaggerOperation(Summary = "Encrypt : get encrypted text", Description = "")]
        public async Task<IActionResult> EncryptTextAsync(EncryptionViewModel model)
        {
            ResponseModel response = new();

            if (model.Text is null)
            {
                response.Message = "Enter text to encrypt.";
                return BadRequest(response);
            }
            string EncryptedText = SecurityProvider.EncryptTextAsync(model.Text);
            // Return Success Response
            response.IsSuccess = true;
            response.Message = "Encryption successful!";
            response.Response = new { InputText = model.Text, EncryptedText };
            return Ok(response);
        }

        #endregion

        #region Decrypt UserName
        /// <summary>
        /// Route: api/v1/identityapi/account/decryptusername
        /// Decrypt : get original Username
        /// </summary>

        [HttpPost()]
        [Route("DecryptUsername")]
        [SwaggerOperation(Summary = "Decrypt : get original Username", Description = "")]
        public async Task<IActionResult> DecryptUsernameAsync(DecryptionViewModel model)
        {
            ResponseModel response = new();
            if (model.Text is null)
            {
                response.Message = "Enter Username to decrypt.";
                return BadRequest(response);
            }
            string DecryptedUsername = SecurityProvider.DecryptUsernameAsync(model.Text);
            // Return Success Response
            response.IsSuccess = true;
            response.Message = "Username Decryption successful!";
            response.Response = new { EncryptedUsername = model.Text, DecryptedUsername };
            return Ok(response);
        }
        #endregion

        #region Decrypt All UserName
        /// <summary>
        /// GET: api/v1/identityapi/account/decryptallusername
        /// Decrypt : get original Username
        /// </summary>

        [HttpGet()]
        [Route("DecryptAllUsername")]
        [SwaggerOperation(Summary = "Decrypt : get original Username", Description = "")]
        public async Task<IActionResult> DecryptAllUsernameAsync()
        {
            ResponseModel response = new();

            List<ApplicationUser> userList = _userManager.Users.ToList();

            foreach (var User in userList)
            {
                string DecryptedUsername = SecurityProvider.DecryptUsernameAsync(User.UserName);
                _ = await _userManager.SetUserNameAsync(User, DecryptedUsername);
            }

            // Return Success Response
            response.IsSuccess = true;
            response.Message = $"Total {userList.Count} usernames decrypted successful!";
            response.Response = null;
            return Ok(response);
        }
        #endregion

        #region EncryptNew
        /// <summary>
        /// Route: api/v1/identityapi/account/encryptnew
        /// Encrypt : Encrypt text with new algorithm
        /// </summary>

        [HttpPost()]
        [Route("EncryptNew")]
        [SwaggerOperation(Summary = "Encrypt : Encrypt text with new algorithm", Description = "")]
        public async Task<IActionResult> EncryptNewAlgoAsync(DecryptionViewModel model)
        {
            ResponseModel response = new();

            if (model.Text is null)
            {
                response.Message = "Enter Text to encrypt.";
                return BadRequest(response);
            }

            string EncryptedText = SecurityProvider.EncryptNewAlgoAsync(model.Text);

            // Return Success Response
            response.IsSuccess = true;
            response.Message = "Encryption successful!";
            response.Response = new { OriginalText = model.Text, EncryptedText };
            return Ok(response);
        }
        #endregion

        #region Decrypt New
        /// <summary>
        /// Route: api/v1/identityapi/account/decryptnew
        /// Decrypt : Decrypt text with new algorithm
        /// </summary>

        [HttpPost()]
        [Route("DecryptNew")]
        [SwaggerOperation(Summary = "", Description = "")]
        public async Task<IActionResult> DecryptNewAlgoAsync(DecryptionViewModel model)
        {
            ResponseModel response = new();

            if (model.Text is null)
            {
                response.Message = "Enter Text to decrypt.";
                return BadRequest(response);
            }

            string DecryptedText = SecurityProvider.DecryptNewAlgoAsync(model.Text);

            // Return Success Response
            response.IsSuccess = true;
            response.Message = "Decryption successful!";
            response.Response = new { EncryptedText = model.Text, DecryptedText };
            return Ok(response);
        }
        #endregion

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #region SendOTP
        /// <summary>DONE
        /// POST: api/v1/identityapi/account/sendotp
        /// OTP : send OTP through email
        /// </summary>

        [HttpPost()]
        [Route("sendotp")]
        [SwaggerOperation(Summary = "OTP : send OTP through email", Description = "")]
        public async Task<IActionResult> SendOTP(SendOTPModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Cache values
                    string Email = model.To;
                    string Subject = model.Subject;
                    string otp = "";

                    // Generate OTP
                    if (model.OTPDigitNumber == 6)
                    {
                        otp = OTP.Generate6DigitOTP();
                    }
                    else
                    {
                        otp = OTP.Generate4DigitOTP();
                    }

                    DateTime OTPSentTime = DateTime.UtcNow;

                    // Send email with OTP
                    bool result = await _emailService.SendEmail(new EmailModel()
                    {
                        Subject = Subject,
                        Body = "OTP: " + otp,
                        To = Email
                    });

                    if (result)
                    {
                        //sending email succeeded. Prepare Response
                        SendOTPResponseModel SendOTPResponseModel = new()
                        {
                            Email = Email,
                            OTP = otp,
                            OTPSentAt = OTPSentTime
                        };

                        // Return Success Response                        
                        return Ok(SendOTPResponseModel);
                    }
                    // sending email failed
                    return Ok(null);
                }
                return Ok(null);
            }
            catch (Exception)
            {
                return Ok(null);
            }
        }
        #endregion

        #region SendEmail
        /// <summary>DONE
        /// POST: api/v1/identityapi/account/sendemail
        /// Email : send email with body
        /// </summary>

        [HttpPost()]
        [Route("sendemail")]
        [SwaggerOperation(Summary = "Email : send email with body", Description = "")]
        public async Task<IActionResult> SendEmail(SendEmailModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Cache values
                    string Email = model.To;
                    string Subject = model.Subject;
                    string Body = model.Body;

                    // Send email with OTP
                    bool result = await _emailService.SendEmail(new EmailModel()
                    {
                        Subject = Subject,
                        Body = Body,
                        To = Email
                    });

                    if (result)
                    {
                        // Return Success Response                        
                        return Ok(new { IsEmailSent = true });
                    }

                    // sending email failed
                    return Ok(null);
                }
                return Ok(null);
            }
            catch (Exception)
            {
                return Ok(null);
            }
        }
        #endregion

        #region nonprofit/register
        /// <summary>
        /// Route: api/v1/identityapi/account/nonprofit/register
        /// Non-Profit : register non-profit
        /// For: call from CampaignAPI
        /// </summary>

        [HttpPost()]
        [Route("nonprofit/register")]
        [SwaggerOperation(Summary = "Non-Profit : register non-profit, For: call from CampaignAPI", Description = "")]
        public async Task<IActionResult> RegisterNonProfit(RegisterNonProfitRequestModel model)
        {
            ResponseModel response = new();
            try
            {
                var data = await _accountService.RegisterNonProfit(model);
                return Ok(data);
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;
                return Ok(response);
            }
        }
        #endregion

        #region nonprofit/activate/{userId}
        /// <summary>
        /// GET: api/v1/identityapi/account/nonprofit/activate/{userId}
        /// Non-Profit : update non-profit status to active
        /// For: internal call from CampaignAPI
        /// </summary>

        [HttpGet()]
        [Route("nonprofit/activate/{userId}")]
        [SwaggerOperation(Summary = "", Description = "")]
        public async Task<IActionResult> ActivateNonProfit(string userId)
        {
            ResponseModel response = new();
            try
            {
                var data = await _accountService.ActivateNonProfit(userId);
                return Ok(data);
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;
                return Ok(false);
            }
        }
        #endregion

    }
}
