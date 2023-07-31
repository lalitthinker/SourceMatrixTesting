using IdentityApi.Controllers;
using IdentityApi.Services.CommonSp;
using IdentityApi.Services.Excel;
using IdentityApi.Services.PDF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebUtilities;
using System.IO;

namespace IdentityApi.Services.Accounts
{
    public class AccountService : IAccountService
    {
        #region fields
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
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IExcelService _excelService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPDFService _pdfService;
        private readonly ICommonSPServices _commonSPServices;
        #endregion

        #region ctor
        public AccountService(IPDFService pdfService,
                              ILoginService<ApplicationUser> loginService,
                              IIdentityServerInteractionService interaction,
                              IClientStore clientStore,
                              ILogger<AccountController> logger,
                              UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IConfiguration configuration,
                              IEmailService emailService,
                              ApplicationDbContext applicationDbContext,
                              IHttpApiRequests httpApiRequests,
                              ICommonService commonService,
                              IExcelService excelService,
                              IWebHostEnvironment hostingEnvironment,
                              ICommonSPServices commonSPServices)

        {
            _pdfService = pdfService ?? throw new ArgumentNullException(nameof(pdfService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
            _interaction = interaction ?? throw new ArgumentNullException(nameof(interaction));
            _clientStore = clientStore ?? throw new ArgumentNullException(nameof(clientStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _httpApiRequests = httpApiRequests ?? throw new ArgumentNullException(nameof(httpApiRequests));
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
            _excelService = excelService ?? throw new ArgumentNullException(nameof(excelService));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _commonSPServices = commonSPServices ?? throw new ArgumentNullException(nameof(commonSPServices));
        }
        #endregion

        #region register
        public async Task<ResponseModel> Register(RegisterViewModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Cache values
                string Email = model.Email;
                string UserName = model.Email;
                string FirstName = model.FirstName;
                string LastName = model.LastName;
                var ProfilePictureResponse = await _commonService.UploadProfileImageAsync(model.ProfilePictureFile);

                ApplicationUser User = new()
                {
                    FirstName = SecurityProvider.EncryptTextAsync(FirstName),
                    LastName = SecurityProvider.EncryptTextAsync(LastName),
                    UserName = SecurityProvider.EncryptTextAsync(UserName),
                    Email = SecurityProvider.EncryptTextAsync(Email),
                    TimeZone = "UTC",
                    DeviceToken = "",
                    EmailConfirmed = true,
                    IsApproved = true,
                    CoverPictureUrl = ProfilePictureResponse,
                    IsDeleted = false,
                    IsLoggedIn = false,
                    UserRole = UserRole.Admin.ToString(),
                    UserStatusId = (int)UserStatus.Active,
                    CreatedDate = DateTime.UtcNow,
                };

                IdentityResult userResult = await _userManager.CreateAsync(User, model.Password);
                if (userResult.Errors.Any())
                {
                    response.Message = "Something went wrong. User registration failed!";
                    response.Errors = userResult.Errors.Select(e => e.Description).ToList();
                    return response;
                }

                // Return Success Response
                response.IsSuccess = true;
                response.Message = "User is registered successfully!";
                response.Response = null;
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

        #region Validate User
        public async Task<ResponseModel> ValidateUser(RegisterViewModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Cache values
                string Email = model.Email;
                string UserName = model.Username;
                string Password = model.Password;
                string ConfirmPassword = model.ConfirmPassword;
                List<IdentityError> Errors = new();
                if (Email is null)
                {
                    Errors.Add(new() { Code = "Email", Description = "Email cannot be empty." });
                }
                else
                {
                    // Get Encrypted value
                    string EncryptedEmail = SecurityProvider.EncryptTextAsync(Email);
                    // Check whether user email already exists
                    ApplicationUser User = await _userManager.FindByEmailAsync(EncryptedEmail);
                    if (User != null)
                    {
                        Errors.Add(new() { Code = "Email", Description = "Email already registered!" });
                    }
                }
                if (UserName is null)
                {
                    Errors.Add(new() { Code = "UserName", Description = "UserName cannot be empty." });
                }
                else
                {
                    // Check whether user email already exists
                    ApplicationUser User = await _userManager.FindByNameAsync(UserName);
                    if (User != null)
                    {
                        Errors.Add(new() { Code = "UserName", Description = "UserName already taken!" });
                    }
                }
                if (Password is null)
                {
                    Errors.Add(new() { Code = "Password", Description = "The password cannot be empty." });
                }
                // check password has correct combination
                if (!ValidationWithRegEx.ValidatePassword(Password))
                {
                    Errors.Add(new() { Code = "Password", Description = "Passwords must have at least one non alphanumeric character." });
                    Errors.Add(new() { Code = "Password", Description = "Passwords must have at least one digit ('0'-'9')." });
                    Errors.Add(new() { Code = "Password", Description = "Passwords must have at least one uppercase ('A'-'Z')." });
                }
                if (Password != ConfirmPassword)
                {
                    Errors.Add(new() { Code = "Confirm Password", Description = "The password and confirmation password do not match" });
                }
                // Check if any error found
                if (Errors.Any())
                {
                    // Yes! error found. Return with error descriptions.
                    response.Message = "Validation failed!";
                    response.Errors = Errors.Select(e => e.Description).ToList();
                    return response;
                }
                /// No error found! Validation successful. 
                /// Now send OTP to verify email.
                // Generate 4-Digit OTP
                string otp = OTP.Generate4DigitOTP();
                DateTime OTPSentTime = DateTime.UtcNow;

                // Send email with OTP
                var result = await _emailService.SendEmail(new EmailModel()
                {
                    Subject = "OTP from Knobl to verify Email",
                    Body = "OTP: " + otp,
                    To = Email
                });

                if (result)
                {
                    //sending email succeeded. Prepare response
                    SendOTPResponseModel SendOTPResponseModel = new()
                    {
                        Email = Email,
                        OTP = otp,
                        OTPSentAt = OTPSentTime
                    };

                    // Return Success Response
                    response.IsSuccess = true;
                    response.Message = "Validation successful! Email verification OTP has been sent on the email.";
                    response.Response = SendOTPResponseModel;
                    return response;
                }

                // sending email failed
                response.Message = "Sending OTP failed. Please try again.";
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

        #region Update Profile
        public async Task<ResponseModel> UpdateProfile(UpdateProfileModel model)
        {
            ResponseModel response = new();
            {
                try
                {
                    // Check if User exists
                    var User = _userManager.Users.Where(u => u.Id == model.UserId).FirstOrDefault();

                    if (User is null)
                    {
                        response.Message = "No user found";
                        return response;
                    }

                    UpdatePhotoNonProfitRequestModel httpRequestModel = new();
                    bool IsPhotoChanged = false;
                    // FirstName
                    string FirstName = model.FirstName;
                    if (!string.IsNullOrEmpty(FirstName))
                    {
                        User.FirstName = FirstName;
                    }

                    // LastName
                    string LastName = model.LastName;
                    if (!string.IsNullOrEmpty(LastName))
                    {
                        User.LastName = LastName;
                    }

                    // Get CoverPictureName if changed
                    if (model.IsChangedCoverPictureName)
                    {
                        string CoverPictureName = string.IsNullOrEmpty(model.CoverPictureName) ? null : model.CoverPictureName;
                        User.CoverPictureUrl = CoverPictureName;
                        httpRequestModel.IsChangedCoverPictureName = true;
                        httpRequestModel.CoverPictureName = CoverPictureName;
                        IsPhotoChanged = false;
                    }

                    // Get ProfilePictureName if changed
                    if (model.IsChangedProfilePictureName)
                    {
                        string ProfilePictureName = string.IsNullOrEmpty(model.ProfilePictureName) ? null : model.ProfilePictureName;
                        User.ProfilePictureUrl = ProfilePictureName;
                        httpRequestModel.IsChangedProfilePictureName = true;
                        httpRequestModel.ProfilePictureName = ProfilePictureName;
                        IsPhotoChanged = false;
                    }

                    // get only limited chars of Description
                    string Description = model.Description;
                    User.Description = string.IsNullOrEmpty(Description) || Description.Length <= 250 ? Description : Description.Substring(0, 250); // first 250 chars

                    User.UpdatedDate = DateTime.UtcNow;
                    IdentityResult userResult = await _userManager.UpdateAsync(User);

                    if (userResult.Errors.Any())
                    {
                        // User Manager action failed
                        response.Message = "Something went wrong. User profile update failed!";
                        response.Errors = userResult.Errors.Select(e => e.Description).ToList();
                        return response;
                    }

                    // Set original values of user data
                    UserVM userVM = new()
                    {
                        Id = User.Id,
                        UserName = User.UserName,
                        FirstName = User.FirstName,
                        LastName = User.LastName,
                        Email = User.Email,
                        ProfilePictureUrl = User.ProfilePictureUrl,
                        ProfilePictureThumbUrl = User.CoverPictureUrl,
                        UserRole = User.UserRole,
                        Description = User.Description,
                        UserStatus = User.UserStatus.ToString(),
                        TimeZone = User.TimeZone,
                        DeviceToken = User.DeviceToken,
                        IsApproved = User.IsApproved,
                        LastLoginDate = User.LastLoginDate,
                        IsLoggedIn = User.IsLoggedIn,
                        CreatedDate = User.CreatedDate
                    };

                    // Return Success Response
                    response.IsSuccess = true;
                    response.Message = "User profile is updated successfully!";
                    response.Response = userVM;
                    return response;
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = ex.Message;
                }
                return response;
            }
        }
        #endregion

        #region resend OTP
        public async Task<ResponseModel> ResendOTP(ResendOTPViewModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Cache Email value
                string Email = model.Email;

                //// Check whether user email already exists
                var user = await _userManager.FindByEmailAsync(SecurityProvider.EncryptTextAsync(Email));

                if (user != null)
                {
                    response.Message = "Email already registered! Please try again with another email.";
                    return response;
                }

                // Generate 4-Digit OTP
                string otp = OTP.Generate4DigitOTP();

                DateTime OTPSentTime = DateTime.UtcNow;

                // Send email with OTP
                bool result = await _emailService.SendEmail(new EmailModel()
                {
                    Subject = "Resend: OTP from Knobl for Email Verification",
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
                    response.IsSuccess = true;
                    response.Message = "OTP has been sent on the email.";
                    response.Response = SendOTPResponseModel;
                    return response;
                }

                // sending email failed
                response.Message = "Sending OTP failed. Please try again.";
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

        #region WebAdminLogin
        public async Task<ResponseModel> WebAdminLogin(LoginViewModel request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                HttpClient _httpClient = new();

                // Cache values
                string EncryptedUserName =/* await _encryptionService.EncryptUsernameAsync(request.Username);*/request.Username;
                string Password = request.Password?.Trim();

                // Get User details
                ApplicationUser User = await _userManager.FindByNameAsync(EncryptedUserName);
                if (User is null)
                {
                    response.Message = "Invalid Username OR Password";
                    return response;
                }

                // Get Identity Server Base Url
                string IdentityServerBaseUrl = AppConstants.BaseUrl.IdentityAPI;

                // Validate User and Get Password Token
                var tokenResponse = await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = IdentityServerBaseUrl + "/connect/token",
                    ClientId = "SourceMatrixClient",
                    ClientSecret = "secret",
                    Scope = "IdentityAPI SourceMatrixAPI ResourceAPI",
                    GrantType = "password",
                    UserName = EncryptedUserName,
                    Password = Password,
                });

                // check if response has any error
                if (tokenResponse.IsError)
                {
                    response.Message = (tokenResponse.ErrorDescription == "invalid_username_or_password") ? "Invalid Username OR Password" : tokenResponse.ErrorDescription.Replace('_', ' ');
                    return response;
                }

                // Update login info
                User.LastLoginDate = DateTime.UtcNow;
                User.IsLoggedIn = true;
                User.TimeZone = string.IsNullOrEmpty(request.TimeZone) ? "UTC" : request.TimeZone;
                if (!string.IsNullOrEmpty(request.DeviceToken))
                {
                    User.DeviceToken = request.DeviceToken;
                }
                _ = await _userManager.UpdateAsync(User);

                // Set original values of user data
                UserVM userVM = new()
                {
                    Id = User.Id,
                    UserName =/* await _encryptionService.DecryptUsernameAsync(EncryptedUserName)*/EncryptedUserName,
                    FirstName = SecurityProvider.DecryptTextAsync(User.FirstName),
                    LastName = SecurityProvider.DecryptTextAsync(User.LastName),
                    Email = SecurityProvider.DecryptTextAsync(User.Email),
                    ProfilePictureUrl = await _commonService.GetImageAsBase64Url(ResourcePath.GetUrl(ResourceType.UserCoverImage, User.CoverPictureUrl)),
                    ProfilePictureThumbUrl = await _commonService.GetImageAsBase64Url(ResourcePath.GetUrl(ResourceType.UserCoverImage, User.CoverPictureUrl)),
                    UserRole = User.UserRole,
                    Description = User.Description,
                    UserStatus = User.UserStatus.ToString(),
                    TimeZone = User.TimeZone,
                    DeviceToken = User.DeviceToken,
                    IsApproved = User.IsApproved,
                    LastLoginDate = User.LastLoginDate,
                    IsLoggedIn = User.IsLoggedIn,
                    CreatedDate = User.CreatedDate
                };

                // Prepare response
                LoginResponseModel loginResponseModel = new()
                {
                    User = userVM,
                    AccessToken = tokenResponse.AccessToken,
                    ExpiresIn = tokenResponse.ExpiresIn,
                    Scope = tokenResponse.Scope,
                    TokenType = tokenResponse.TokenType
                };

                // Return Success Response
                response.IsSuccess = true;
                response.Message = "Login successful!";
                response.Response = loginResponseModel;
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

        #region Forgot possword
        public async Task<ResponseModel> ForgotPassword(ForgotPasswordViewModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Cache Email value
                string Email = model.Email;

                // Get Encrypted value
                string EncryptedEmail = SecurityProvider.EncryptTextAsync(Email);

                // Check whether user already exists with the email
                ApplicationUser User = await _userManager.FindByEmailAsync(EncryptedEmail);
                if (User == null)
                {
                    response.Message = "No such email registered";
                    return response;
                }

                // Generate 4-Digit OTP
                string otp = OTP.Generate4DigitOTP();

                // Get Reset Password Token
                var token = await _userManager.GeneratePasswordResetTokenAsync(User);

                DateTime OTPSentTime = DateTime.UtcNow;

                // Send email with OTP
                var result = await _emailService.SendEmail(new EmailModel()
                {
                    Subject = "Forgot Password: OTP from Knobl to recover password",
                    Body = "OTP: " + otp,
                    To = Email
                });

                //sending email succeeded
                if (result)
                {
                    //Prepare Response
                    ForgotPasswordResponseModel ForgotPasswordResponseModel = new()
                    {
                        Email = Email,
                        OTP = otp,
                        Token = token,
                        OTPSentAt = OTPSentTime
                    };

                    // Return Success Response
                    response.IsSuccess = true;
                    response.Message = "OTP has been sent on the email.";
                    response.Response = ForgotPasswordResponseModel;
                    return response;
                }

                // sending email failed
                response.Message = "Sending OTP failed. Please try again.";
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

        #region Reset Password
        public async Task<ResponseModel> ResetPassword(ResetPasswordViewModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Cache values
                string Email = model.Email;
                string ReceivedToken = model.Token;
                string Password = model.Password;

                // Get Encrypted value
                string EncryptedEmail = SecurityProvider.EncryptTextAsync(Email);

                // Check whether user already exists with the email
                ApplicationUser User = await _userManager.FindByEmailAsync(EncryptedEmail);
                if (User == null)
                {
                    response.IsSuccess = false;
                    response.Message = "No such email registered";
                    return response;
                }

                // Decode Reset-Password-Token for web
                var DecodedToken = WebEncoders.Base64UrlDecode(ReceivedToken);
                string OriginalToken = Encoding.UTF8.GetString(DecodedToken);

                // Verify Reset Password Token and Then Update Password
                var IdentityResult = await _userManager.ResetPasswordAsync(User, ReceivedToken, Password);

                if (IdentityResult.Succeeded)
                {
                    // Send email for Password Reset Action
                    bool result = await _emailService.SendEmail(new EmailModel()
                    {
                        Subject = "Knobl : Reset Password Confirmation",
                        Body = "Your password has been reset successfully!",
                        To = Email
                    });

                    // Sending email failed
                    if (!result)
                    {
                        response.IsSuccess = false;
                        response.Response = "Reset Password Confirmation Email not sent.";
                        return response;
                    }

                    // Return Success Response
                    response.IsSuccess = true;
                    response.Message = "Your password has been reset successfully!";
                    return response;
                }

                // User Manager action failed
                response.IsSuccess = false;
                response.Message = "Something went wrong. Server could not process request.";
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

        #region Verify Email
        public async Task<ResponseModel> VerifyEmail(VerifyEmailViewModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Cache Email value
                string Email = model.Email;

                // Get Encrypted value
                string EncryptedEmail = SecurityProvider.EncryptTextAsync(Email);

                // Check whether user already exists with the email
                ApplicationUser User = await _userManager.FindByEmailAsync(EncryptedEmail);
                if (User != null)
                {
                    response.Message = "Email already registered! Please try again with another email.";
                    return response;
                }

                // Generate 4-Digit OTP
                string otp = OTP.Generate4DigitOTP();

                DateTime OTPSentTime = DateTime.UtcNow;

                // Send email with OTP
                var result = await _emailService.SendEmail(new EmailModel()
                {
                    Subject = "OTP from Knobl for Email Verification",
                    Body = "OTP: " + otp,
                    To = Email
                });

                if (result) //sending email succeeded
                {
                    // Prepare Response
                    SendOTPResponseModel SendOTPResponseModel = new()
                    {
                        Email = Email,
                        OTP = otp,
                        OTPSentAt = OTPSentTime
                    };
                    // Return Success Response
                    response.IsSuccess = true;
                    response.Message = "Email verification OTP has been sent on the email.";
                    response.Response = SendOTPResponseModel;
                    return response;
                }

                // sending email failed
                response.Message = "Sending OTP failed. Please try again.";
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

        #region ForgotUsername
        public async Task<ResponseModel> ForgotUsername(ForgotUsernameViewModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Cache Email value
                string Email = model.Email;

                // Get Encrypted value
                string EncryptedEmail = SecurityProvider.EncryptTextAsync(Email);

                // Check whether user already exists with the email
                ApplicationUser User = await _userManager.FindByEmailAsync(EncryptedEmail);
                if (User == null)
                {
                    response.Message = "No such email registered";
                    return response;
                }

                // Generate 4-Digit OTP
                string otp = OTP.Generate4DigitOTP();

                DateTime OTPSentTime = DateTime.UtcNow;

                // Send email with OTP
                var result = await _emailService.SendEmail(new EmailModel()
                {
                    Subject = "Forgot Username: OTP from Knobl to recover username",
                    Body = "OTP: " + otp,
                    To = Email
                });

                //sending email succeeded
                if (result)
                {
                    //Prepare Response
                    SendOTPResponseModel SendOTPResponseModel = new()
                    {
                        Email = Email,
                        OTP = otp,
                        OTPSentAt = OTPSentTime
                    };

                    // Return Success Response
                    response.IsSuccess = true;
                    response.Message = "OTP to recover username has been sent on the email";
                    response.Response = SendOTPResponseModel;
                    return response;
                }

                // sending email failed
                response.Message = "Sending OTP failed. Please try again.";
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

        #region RecoveryUsername
        public async Task<ResponseModel> RecoverUsername(ForgotUsernameViewModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Cache Email value
                string Email = model.Email;

                // Get Encrypted value
                string EncryptedEmail = SecurityProvider.EncryptTextAsync(Email);

                // Check whether user already exists with the email
                ApplicationUser User = await _userManager.FindByEmailAsync(EncryptedEmail);
                if (User == null)
                {
                    response.Message = "No such email registered";
                    return response;
                }

                // Send email with Username
                bool result = await _emailService.SendEmail(new EmailModel()
                {
                    Subject = "Knobl : Username recovery",
                    Body = "Your username associated with this email: " + User.UserName,
                    To = Email
                });

                //sending email succeeded
                if (result)
                {
                    // Return Success Response
                    response.IsSuccess = true;
                    response.Message = "Username has been sent on the email.";
                    response.Response = new { Email };
                    return response;
                }
                // sending email failed
                response.Message = "Sending username on email failed. Please try again.";
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

        #region Change Password
        public async Task<ResponseModel> ChangePassword(ChangePasswordViewModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Check whether user already exists with the userId
                ApplicationUser User = await _userManager.FindByIdAsync(model.UserId);
                if (User == null)
                {
                    response.Message = "Invalid user.";
                    return response;
                }
                // Cache passwords
                var oldPassword = model.OldPassword;
                var newPassword = model.NewPassword;
                var confirmNewPassword = model.ConfirmNewPassword;

                if (newPassword != confirmNewPassword)
                {
                    response.Message = "The new password and confirmation password do not match";
                    return response;
                }

                if (newPassword == oldPassword)
                {
                    response.Message = "The new password and old password can not be same";
                    return response;
                }

                // Verify old password and then change to new Password
                var identityResult = await _userManager.ChangePasswordAsync(User, oldPassword, newPassword);

                if (identityResult.Errors.Any())
                {
                    // User Manager action failed
                    response.Message = identityResult.GetErrorsAsString();
                    response.Response = identityResult.Errors.Select(e => e.Description).ToList();
                    return response;
                }

                // Return Success Response
                response.IsSuccess = true;
                response.Message = "Your password has been changed successfully!";
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

        #region Admin reset password
        public async Task<ResponseModel> AdminRestPassword(ForgotPasswordViewModel model)
        {
            ResponseModel response = new();
            try
            {
                List<IdentityError> Errors = new();
                // Cache Email value
                string Email = SecurityProvider.EncryptTextAsync(model.Email);

                var newPassword = model.NewPassword;
                var confirmNewPassword = model.ConfirmNewPassword;

                if (newPassword != confirmNewPassword)
                {
                    Errors.Add(new() { Code = "Password", Description = "The new password and confirmation password do not match." });
                    response.Message = Errors.Select(e => e.Description).FirstOrDefault();
                    response.IsSuccess = false;
                    return response;
                }

                if (!ValidationWithRegEx.ValidatePassword(newPassword))
                {
                    Errors.Add(new() { Code = "Password", Description = "Passwords must have at least one non alphanumeric character." });
                    Errors.Add(new() { Code = "Password", Description = "Passwords must have at least one digit ('0'-'9')." });
                    Errors.Add(new() { Code = "Password", Description = "Passwords must have at least one uppercase ('A'-'Z')." });
                    response.Message = Errors.Select(e => e.Description).FirstOrDefault();
                    response.IsSuccess = false;
                    return response;
                }

                // Check whether user already exists with the email
                ApplicationUser User = await _userManager.FindByEmailAsync(Email);
                if (User == null)
                {
                    response.IsSuccess = false;
                    response.Message = "No such email registered";
                    return response;
                }
                var decryptedEmail = SecurityProvider.DecryptTextAsync(Email);
                // Get Reset Password Token
                var token = await _userManager.GeneratePasswordResetTokenAsync(User);
                if (model.ThroughEmail == true)
                {
                    // Send reset-password link to user email
                    string InvitationId = Guid.NewGuid().ToString();
                    string webBaseUrl = _configuration["BaseUrl:AdminWeb"];
                    string ResetPasswordLink = $"{webBaseUrl}/reset-password?email={decryptedEmail}&token={token}";

                    // Body
                    StringBuilder body = new StringBuilder();
                    body.Append("Hello,");
                    body.AppendLine();
                    body.AppendLine();
                    body.AppendLine();

                    body.Append($"Please follow this link to reset your Source Matrix® Web Application password for your {decryptedEmail} account.");
                    body.AppendLine();
                    body.AppendLine();

                    body.Append(ResetPasswordLink);
                    body.AppendLine();
                    body.AppendLine();

                    body.Append("If you didn’t ask to reset your password, you can ignore this email.");
                    body.AppendLine();
                    body.AppendLine();

                    body.Append("Cheers,");
                    body.AppendLine();
                    body.AppendLine();

                    body.Append("Your Source Matrix® team");
                    string Subject = "Class Source Matrix® Application Password Reset";
                    string Body = body.ToString();
                    var result1 = await _emailService.SendEmailBySendGrid(Email, Subject, Body, "Customer");
                    //sending email succeeded

                    if (result1)
                    {
                        // Return Success Response 
                        response.IsSuccess = true;
                        response.Message = $"Reset email has been sent to {decryptedEmail}";
                        response.Response = "";
                        return response;
                    }
                }
                else
                {
                    var IdentityResult = await _userManager.ResetPasswordAsync(User, token, newPassword);

                    DateTime OTPSentTime = DateTime.UtcNow;
                    if (IdentityResult.Succeeded)
                    {
                        // Return Success Response
                        response.IsSuccess = true;
                        response.Message = "Your password has been reset successfully!";
                        response.Response = "";
                        return response;
                    }
                    if (!IdentityResult.Succeeded)
                    {
                        // Password Reset Action failed
                        response.IsSuccess = false;
                        response.Message = "Something went wrong. Server could not process request.";
                        return response;
                    }
                }
                response.IsSuccess = false;
                response.Message = "This Email Id Does Not Exist.";
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

        #region Get all Users
        public async Task<ResponseModel> GetAllUsersAsync(RequestAccountModel request)
        {
            ResponseModel response = new();
            try
            {
                List<UserByRoleIdVM> allUserWithDateFilter = new();
                var query1 = "";
                if (request.RoleId == "")
                {
                    allUserWithDateFilter = await _commonSPServices.GetAllUsersWithFilter(request);
                }
                else
                {
                    allUserWithDateFilter = await _commonSPServices.GetUsersByRoleId(request);
                }
                if (allUserWithDateFilter.Count == 0)
                {
                    response.Message = "No Record Found!!";
                    return response;
                }

                List<UserProfile> users = new();
                foreach (var User in allUserWithDateFilter)
                {
                    request.RoleId = "";
                    List<RequestAllUsersRolesModel> userByRoleIdDataAsync = await _commonSPServices.GetRoleById(request);
                    string FirstName = SecurityProvider.DecryptTextAsync(User.FirstName);
                    string LastName = SecurityProvider.DecryptTextAsync(User.LastName);
                    string UserName = SecurityProvider.DecryptTextAsync(User.UserName);
                    string CoverUrl = await _commonService.GetImageAsBase64Url(ResourcePath.GetUrl(ResourceType.UserCoverImage, User.CoverPictureUrl));

                    string Email = SecurityProvider.DecryptTextAsync(User.Email);
                    string PhoneNumber = string.IsNullOrEmpty(User.PhoneNumber) ? "" : SecurityProvider.DecryptTextAsync(User.PhoneNumber);

                    UserProfile u = new()
                    {
                        Id = User.Id,
                        FullName = (FirstName + " " + LastName).Trim(),
                        FirstName = FirstName,
                        LastName = LastName,
                        UserName = UserName,
                        CoverPictureUrl = CoverUrl,
                        //ProfilePictureUrl = ProfileUrl,
                        PhoneNumber = PhoneNumber,
                        Email = Email,
                        TimeZone = User.TimeZone,
                        DeviceToken = User.DeviceToken,
                        Description = User.Description,
                        SaleQuota = string.Concat(User.SaleQuota.Replace(".00", "")),
                        PurchaseQuota = string.Concat(User.PurchaseQuota.Replace(".00", "")),
                        City = User.City,
                        Title = User.Title,
                        IsExpandable = false,
                        CreatedDate = User.CreatedDate,
                        UserStatus = User.UserStatus,   // == 1 ? true : false
                        EmergencyContactNumber = User.EmergencyContactNumber,
                        EmergencyContactName = User.EmergencyContactName,
                        OfficePhoneNumber = User.OfficePhoneNumber,
                        IsChecked = false,
                        ImageZoomRatio = User.ImageZoomRatio,
                        SalesCommissionRate = User.SalesCommissionRate,
                        PurchaseCommissionRate = User.PurchaseCommissionRate,
                        SalesCommissionRateName = string.IsNullOrEmpty(User.SalesCommissionRate) ? "" : (await _httpApiRequests.GetAllSaleCommissionRole()).Where(x => x.Id == User.SalesCommissionRate).Select(x => x.Name).FirstOrDefault(),
                        PurchaseCommissionRateName = string.IsNullOrEmpty(User.PurchaseCommissionRate) ? "" : (await _httpApiRequests.GetAllPurchaseCommissionRole()).Where(x => x.Id == User.PurchaseCommissionRate).Select(x => x.Name).FirstOrDefault(),
                        DirectPhoneNumber = User.DirectPhoneNumber,
                        NumberExtension = User.NumberExtension,
                        TotalRecords = User.TotalRecords
                    };
                    u.RoleName.AddRange(userByRoleIdDataAsync.Where(x => x.UserId == u.Id).Select(x => new RequestAllUsersRolesModel { RoleName = x.RoleName, RoleColor = x.RoleColor }));
                    userByRoleIdDataAsync.Select(x => x.RoleName).Where(y => y.Equals(u.Id));
                    users.Add(u);
                }

                // apply SEARCH filters
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var query = @"exec [sp_GetAllUsersWithFilter] @GetAll='" + true + "', " +
                                                                "@PageSize = '" + 0 + "'," +
                                                                "@SortColumn = '" + "" + "'," +
                                                                "@SortDirection = '" + "" + "'," +
                                                                "@Page = '" + 0 + "'," +
                                                                "@Date = '" + "" + "'," +
                                                                "@Userstatus='" + 2 + "'," +
                                                                "@Id = '" + "" + "' ";
                    var Data = await _applicationDbContext.UserByRolesId.FromSqlRaw(query)!.ToListAsync();

                    string searchText = request.SearchText.ToLower();
                    //users = users.FindAll(s => s.FirstName.ToLower().Contains(searchText)
                    //                        || s.LastName.ToLower().Contains(searchText));
                    //users.ForEach(s => s.TotalRecords = users.Count);
                    List<UserProfile> us = new();
                    foreach (var user in Data)
                    {
                        List<RequestAllUsersRolesModel> userByRoleIdDataAsync = await _commonSPServices.GetRoleById(request);
                        string FirstName = SecurityProvider.DecryptTextAsync(user.FirstName);
                        string LastName = SecurityProvider.DecryptTextAsync(user.LastName);
                        string UserName = SecurityProvider.DecryptTextAsync(user.UserName);
                        string CoverUrl = await _commonService.GetImageAsBase64Url(ResourcePath.GetUrl(ResourceType.UserCoverImage, user.CoverPictureUrl));

                        string Email = SecurityProvider.DecryptTextAsync(user.Email);
                        string PhoneNumber = string.IsNullOrEmpty(user.PhoneNumber) ? "" : SecurityProvider.DecryptTextAsync(user.PhoneNumber);
                        UserProfile up = new()
                        {
                            Id = user.Id,
                            FullName = (FirstName + " " + LastName).Trim(),
                            FirstName = FirstName,
                            LastName = LastName,
                            UserName = UserName,
                            CoverPictureUrl = CoverUrl,
                            PhoneNumber = PhoneNumber,
                            Email = Email,
                            TimeZone = user.TimeZone,
                            DeviceToken = user.DeviceToken,
                            Description = user.Description,
                            SaleQuota = string.Concat(user.SaleQuota.Replace(".00", "")),
                            PurchaseQuota = string.Concat(user.PurchaseQuota.Replace(".00", "")),
                            City = user.City,
                            Title = user.Title,
                            IsExpandable = false,
                            CreatedDate = user.CreatedDate,
                            UserStatus = user.UserStatus,   // == 1 ? true : false
                            EmergencyContactNumber = user.EmergencyContactNumber,
                            EmergencyContactName = user.EmergencyContactName,
                            OfficePhoneNumber = user.OfficePhoneNumber,
                            IsChecked = false,
                            ImageZoomRatio = user.ImageZoomRatio,
                            TotalRecords = user.TotalRecords
                        };
                        up.RoleName.AddRange(userByRoleIdDataAsync.Where(x => x.UserId == up.Id).Select(x => new RequestAllUsersRolesModel { RoleName = x.RoleName, RoleColor = x.RoleColor }));
                        userByRoleIdDataAsync.Select(x => x.RoleName).Where(y => y.Equals(up.Id));
                        us.Add(up);
                    }

                    us = us.FindAll(s => s.FirstName.ToLower().Contains(searchText)
                                            || s.LastName.ToLower().Contains(searchText));
                    us.ForEach(s => s.TotalRecords = us.Count);

                    response.IsSuccess = true;
                    response.Message = $"Total {us.Count} {(us.Count > 1 ? "users" : "user")} found.";
                    if (us.Count == 0)
                    {
                        response.TotalRecords = us.Count;
                    }
                    else
                    {
                        response.TotalRecords = us.FirstOrDefault().TotalRecords;
                    }
                    response.Response = us.OrderBy(a => a.FullName).ToList();
                    return response;
                }
                //if (!request.GetAll)
                //{
                //    int skip = request.PageSize * (request.PageNumber - 1);
                //    int take = request.PageSize;
                //    users = users.Skip(skip).Take(take).ToList();
                //}

                if (users.Count == 0)
                {
                    response.Message = "No Users Found";
                    response.IsSuccess = false;
                    return response;
                }

                // Return Success Response
                response.IsSuccess = true;
                response.Message = $"Total {users.Count} {(users.Count > 1 ? "users" : "user")} found.";
                response.Response = users.OrderBy(a => a.FullName).ToList();
                if (users.Count == 0)
                {
                    response.TotalRecords = users.Count;
                }
                else
                {
                    response.TotalRecords = users.FirstOrDefault().TotalRecords;
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

        #region SearchByUsersPgnAsync
        public async Task<ResponseModel> SearchByUsersPgnAsync(RequestWithPaginationModel request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                List<ApplicationUser> userList = _userManager.Users.Where(u => !u.IsDeleted && u.UserRole == "Normal").ToList();
                List<UserProfileVM> users = new();

                foreach (var User in userList)
                {
                    string FirstName = SecurityProvider.DecryptTextAsync(User.FirstName);
                    string LastName = SecurityProvider.DecryptTextAsync(User.LastName);
                    UserProfileVM u = new()
                    {
                        Id = User.Id,
                        FullName = (FirstName + " " + LastName).Trim(),
                        UserName = User.UserName,
                        CoverPictureUrl = ResourcePath.GetUrl(ResourceType.UserCoverImage, User.CoverPictureUrl),
                        CoverPictureThumbUrl = ResourcePath.GetThumbUrl(ResourceType.UserCoverImage, User.CoverPictureUrl),
                        ProfilePictureUrl = ResourcePath.GetUrl(ResourceType.UserProfileImage, User.ProfilePictureUrl),
                        ProfilePictureThumbUrl = ResourcePath.GetThumbUrl(ResourceType.UserProfileImage, User.ProfilePictureUrl),
                        Email = SecurityProvider.DecryptTextAsync(User.Email),
                        UserRole = User.UserRole,
                        Description = User.Description
                    };
                    users.Add(u);
                }

                List<UserProfileVM> list = users.OrderBy(s => s.FullName).ToList();

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    string searchText = request.SearchText.ToLower();
                    list = list.FindAll(s => s.FullName.ToLower().Contains(searchText) || s.UserName.ToLower().Contains(searchText));
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

        #region Get User Async
        public async Task<ResponseModel> GetUserAsync(string id)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var User = _userManager.Users.Where(u => u.Id == id).FirstOrDefault();
                if (User is null)
                {
                    response.IsSuccess = true;
                    response.Message = "No user found";
                    response.Response = null;
                    return response;
                }
                UserVM userVM = new()
                {
                    Id = User.Id,
                    UserName = User.UserName,
                    FirstName = SecurityProvider.DecryptTextAsync(User.FirstName),
                    LastName = SecurityProvider.DecryptTextAsync(User.LastName),
                    Email = SecurityProvider.DecryptTextAsync(User.Email),
                    ProfilePictureUrl = User.CoverPictureUrl,
                    ProfilePictureThumbUrl = await _commonService.GetImageAsBase64Url(ResourcePath.GetThumbUrl(ResourceType.UserProfileImage, User.CoverPictureUrl)),
                    UserRole = User.UserRole,
                    Description = User.Description,
                    UserStatus = User.UserStatus.ToString(),
                    TimeZone = User.TimeZone,
                    DeviceToken = User.DeviceToken,
                    IsApproved = User.IsApproved,
                    LastLoginDate = User.LastLoginDate,
                    IsLoggedIn = User.IsLoggedIn,
                    CreatedDate = User.CreatedDate,
                    EmergencyContactName = User.EmergencyContactName,
                    EmergencyContactNumber = User.EmergencyContactNumber,
                    OfficePhoneNumber = User.OfficePhoneNumber
                };

                // Return Success Response
                response.IsSuccess = true;
                response.Message = "User found successfully";
                response.Response = userVM;
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

        #region Get User brief Async
        public async Task<ResponseModel> GetUserBriefAsync(string id)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var User = _userManager.Users.Where(u => u.Id == id).FirstOrDefault();
                if (User is null)
                {
                    return null;
                }

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
                response.Response = userVM;
                // Return Success Response
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

        #region Register non Profit
        public async Task<ResponseModel> RegisterNonProfit(RegisterNonProfitRequestModel model)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Cache values
                string FirstName = model.FirstName;
                string LastName = model.LastName;
                string Email = model.Email;
                string PhoneNumber = model.PhoneNumber;
                string UserName = model.Username;
                string Password = model.Password;
                string ConfirmPassword = model.ConfirmPassword;

                long FoundationId = model.FoundationId;
                string FoundationName = model.FoundationName;

                List<IdentityError> Errors = new();

                if (string.IsNullOrEmpty(Email))
                {
                    Errors.Add(new() { Code = "Email", Description = "Email cannot be empty." });
                }
                else
                {
                    // Get Encrypted value
                    string EncryptedEmail = SecurityProvider.EncryptTextAsync(Email);

                    // Check whether user email already exists
                    ApplicationUser UserByEmail = await _userManager.FindByEmailAsync(EncryptedEmail);

                    if (UserByEmail != null)
                    {
                        Errors.Add(new() { Code = "Email", Description = "Email already registered!" });
                    }
                }

                if (string.IsNullOrEmpty(UserName))
                {
                    Errors.Add(new() { Code = "UserName", Description = "UserName cannot be empty." });
                }
                else
                {
                    // Check whether user email already exists
                    ApplicationUser UserByName = await _userManager.FindByNameAsync(UserName);

                    if (UserByName != null)
                    {
                        Errors.Add(new() { Code = "UserName", Description = "UserName already taken!" });
                    }
                }

                if (string.IsNullOrEmpty(Password))
                {
                    Errors.Add(new() { Code = "Password", Description = "The password cannot be empty." });
                }

                // check password has correct combination
                if (!ValidationWithRegEx.ValidatePassword(Password))
                {
                    Errors.Add(new() { Code = "Password", Description = "Passwords must have at least one non alphanumeric character." });
                    Errors.Add(new() { Code = "Password", Description = "Passwords must have at least one digit ('0'-'9')." });
                    Errors.Add(new() { Code = "Password", Description = "Passwords must have at least one uppercase ('A'-'Z')." });
                }

                if (Password != ConfirmPassword)
                {
                    Errors.Add(new() { Code = "Confirm Password", Description = "The password and confirmation password do not match" });
                }

                // Check if any error found
                if (Errors.Any())
                {
                    // Yes! error found. Return with error descriptions.
                    response.Message = "Validation failed!";
                    response.Errors = Errors.Select(e => e.Description).ToList();
                    return response;
                }

                /// No error found! Validation successful. Create User account with status as PENDING
                /// 

                ApplicationUser User = new()
                {
                    FirstName = SecurityProvider.EncryptTextAsync(FirstName),
                    LastName = SecurityProvider.EncryptTextAsync(LastName),
                    Email = SecurityProvider.EncryptTextAsync(Email),
                    PhoneNumber = string.IsNullOrEmpty(PhoneNumber) ? null : SecurityProvider.EncryptTextAsync(PhoneNumber),
                    UserName = UserName,
                    Description = model.Description,
                    ProfilePictureUrl = model.ProfilePictureUrl,
                    FoundationId = (FoundationId == 0) ? null : FoundationId,
                    FoundationName = FoundationName,
                    AgeRangeId = (int)AgeRange.NotRequired,
                    EmailConfirmed = true,
                    IsApproved = true,
                    IsDeleted = false,
                    IsLoggedIn = false,
                    UserRole = UserRole.Foundation.ToString(),
                    UserStatusId = (int)UserStatus.Pending,
                    CreatedDate = DateTime.UtcNow,
                };

                IdentityResult userResult = await _userManager.CreateAsync(User, model.Password);
                if (userResult.Errors.Any())
                {
                    // User Manager action failed
                    //AddErrors(userResult);
                    response.Message = "Registration failed!";
                    response.Errors = userResult.Errors.Select(e => e.Description).ToList();
                    return response;
                }

                IdentityResult roleResult = await _userManager.AddToRoleAsync(User, UserRole.Foundation.ToString());
                if (roleResult.Errors.Any())
                {
                    // User Manager action failed
                    //AddErrors(roleResult);
                    response.Message = "Registration failed!!";
                    response.Errors = roleResult.Errors.Select(e => e.Description).ToList();
                    return response;
                }

                // Get User details
                ApplicationUser NewUser = await _userManager.FindByNameAsync(UserName);
                if (NewUser is null)
                {
                    response.Message = "Registration failed.";
                    return response;
                }

                // Return Success Response
                RegisterNonProfitResponseModel responseModel = new()
                {
                    UserId = NewUser.Id
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

        #region Activate Non Profit
        public async Task<ResponseModel> ActivateNonProfit(string userId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                // Check if User exists
                var User = _userManager.Users.Where(u => u.Id == userId).FirstOrDefault();

                if (User is null)
                {
                    response.Message = "No user found";
                    return response;
                }
                User.UserStatusId = (int)UserStatus.Active;
                User.UpdatedDate = DateTime.UtcNow;
                IdentityResult userResult = await _userManager.UpdateAsync(User);

                if (userResult.Errors.Any())
                {
                    // User Manager action failed
                    response.Message = "Something went wrong. User profile update failed!";
                    response.Errors = userResult.Errors.Select(e => e.Description).ToList();
                    return response;
                }

                // Return Success Response
                response.IsSuccess = true;
                response.Message = "User is activated successfully!";
                response.Response = new { Result = true };
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

        #region Export all users
        public async Task<ResponseModel> ExportGetAllUsers(RequestAccountModel request)
        {
            ResponseModel Response = new ResponseModel();
            List<ExportResponse> exportResponse = new();
            try
            {
                List<UserProfile> users = new();
                List<UserProfile> users2 = new();
                string Base64stringHeader = "";
                var UserList = await _commonService.getUsersDetails(request);
                foreach (var Userdetail in UserList)
                {
                    List<RequestAllUsersRolesModel> userByRoleIdDataAsync = await _commonSPServices.GetRoleById(request);
                    var data = userByRoleIdDataAsync.Where(x => x.UserId == Userdetail.Id).ToList();
                    var roleData = string.Join(',', data.Select(x => x.RoleName));
                    string FirstName = SecurityProvider.DecryptTextAsync(Userdetail.FirstName);
                    string LastName = SecurityProvider.DecryptTextAsync(Userdetail.LastName);

                    UserProfile u = new()
                    {
                        Id = Userdetail.Id,
                        FirstName = FirstName,
                        LastName = LastName,
                        FullName = (FirstName + " " + LastName).Trim(),
                        UserName = SecurityProvider.DecryptTextAsync(Userdetail.UserName),
                        CoverPictureUrl = await _commonService.GetImageAsBase64Url(ResourcePath.GetUrl(ResourceType.UserCoverImage, Userdetail.CoverPictureUrl)),
                        PhoneNumber = string.IsNullOrEmpty(Userdetail.PhoneNumber) ? null : SecurityProvider.DecryptTextAsync(Userdetail.PhoneNumber),
                        Email = SecurityProvider.DecryptTextAsync(Userdetail.Email),
                        TimeZone = Userdetail.TimeZone,
                        DeviceToken = Userdetail.DeviceToken,
                        Description = Userdetail.Description,
                        SaleQuota = Convert.ToString(Userdetail.SaleQuota.Replace(".00", "")),
                        PurchaseQuota = Convert.ToString(Userdetail.PurchaseQuota.Replace(".00", "")),
                        IsExpandable = false,
                        CreatedDate = Userdetail.CreatedDate,
                        UserStatus = Userdetail.UserStatus,   // == 1 ? true : false
                        EmergencyContactNumber = Userdetail.EmergencyContactNumber,
                        EmergencyContactName = Userdetail.EmergencyContactName,
                        OfficePhoneNumber = Userdetail.OfficePhoneNumber,
                        IsChecked = false,
                        Roles = roleData
                    };
                    users.Add(u);
                }
                if (UserList.Count > 0)
                {
                    if (request.SelectedId.Count > 0)
                    {
                        foreach (var listItem in request.SelectedId)
                        {
                            var List2 = users.Where(a => a.Id == listItem).FirstOrDefault();
                            users2.Add(List2);
                        }
                    }
                    if (users2.Count != 0)
                    {
                        if (request.ExportAsPdf == true)
                        {
                            using (var stream = new MemoryStream())
                            {
                                _pdfService.CreatePdf(stream, users2);
                                Base64stringHeader = Convert.ToBase64String(stream.ToArray());
                            }
                        }
                        else
                        {
                            MemoryStream memoryStream = new();
                            var templateFileInfo = Path.Combine(_hostingEnvironment.ContentRootPath, "Template", "SampleTemplate.xlsx");
                            var stream = _excelService.CreateExcelIntoExcelTemplate(users2, templateFileInfo, "Users List");
                            Base64stringHeader = Convert.ToBase64String(stream.ToArray());
                        }
                    }
                    else
                    {
                        if (request.ExportAsPdf == true)
                        {
                            using (var stream = new MemoryStream())
                            {
                                _pdfService.CreatePdf(stream, users);
                                Base64stringHeader = Convert.ToBase64String(stream.ToArray());
                            }
                        }
                        else
                        {
                            MemoryStream memoryStream = new();
                            var templateFileInfo = Path.Combine(_hostingEnvironment.ContentRootPath, "Template", "SampleTemplate.xlsx");
                            var stream = _excelService.CreateExcelIntoExcelTemplate(users, templateFileInfo, "Users List");
                            Base64stringHeader = Convert.ToBase64String(stream.ToArray());
                        }
                    }
                    var export = new ExportResponse();
                    export.HeaderData = Base64stringHeader;
                    export.FileName = "Users List";
                    exportResponse.Add(export);
                    Response.Response = exportResponse;
                    Response.IsSuccess = true;
                    Response.Message = "Success";
                }
                else
                {
                    Response.IsSuccess = false;
                    Response.Message = "Record Not Found";
                }
                return Response;
            }
            catch (Exception ex)
            {
                Response.IsSuccess = false;
                Response.Message = ex.Message;
                return Response;
            }
        }
        #endregion
    }
}
