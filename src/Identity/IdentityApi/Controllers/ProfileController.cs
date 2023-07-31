using IdentityApi.Services.Password;

namespace IdentityApi.Controllers
{
    public class ProfileController : BaseController
    {
        #region feilds
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IPasswordService _passwordService;
        

        public ProfileController()
        {
            
        }


        #endregion

        #region ctor
        public ProfileController(IEmailService emailService,
                                 UserManager<ApplicationUser> userManager,
                                 IPasswordService passwordService)
        {
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
        }
        #endregion

        #region CommentedCode
        //    private readonly IIdentityServerInteractionService _interaction;
        //    private readonly IClientStore _clientStore;
        //    private readonly ILogger<AccountController> _logger;
        //    private readonly UserManager<ApplicationUser> _userManager;
        //    private readonly IConfiguration _configuration;
        //    private readonly IEmailService _emailService;

        //    public ProfileController(IIdentityServerInteractionService interaction,
        //                             IClientStore clientStore,
        //                             ILogger<AccountController> logger,
        //                             UserManager<ApplicationUser> userManager,
        //                             IConfiguration configuration,
        //                             IEmailService emailService)
        //    {
        //        _interaction = interaction ?? throw new ArgumentNullException(nameof(interaction));
        //        _clientStore = clientStore ?? throw new ArgumentNullException(nameof(clientStore));
        //        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        //        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        //        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        //    }

        //    /// <summary>
        //    /// Route: api/v1/account/changepassword
        //    /// Change Password : User must be logged in
        //    /// </summary>
        //    [HttpPost()]
        //    [Route("ChangePassword")]
        //    [ProducesDefaultResponseType]
        //    public IActionResult ChangePassword(ForgotPasswordViewModel model)
        //    {
        //        ResponseModel response = new()
        //        {
        //            IsSuccess = false,
        //            Message = "",
        //            Response = null,
        //            Errors = null
        //        };
        //        //var userId = User.Claims.First(c => c.Type == "UserId").Value;
        //        // Cache Email value
        //        //var useremail = model.Email;

        //        //try
        //        //{
        //        //    if (ModelState.IsValid)
        //        //    {
        //        //         Get User details
        //        //        var user = await _userManager.FindByEmailAsync(useremail);

        //        //        if (user == null)
        //        //        {
        //        //            response.Message = "No such email registered";
        //        //            return BadRequest(response);
        //        //        }

        //        //         Generate 6-Digit OTP
        //        //        string otp = OTP.Generate6DigitOTP();

        //        //         Get Reset Password Token
        //        //        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        //        //         Encode Reset Password Token for web
        //        //        var encodedToken = Encoding.UTF8.GetBytes(token);
        //        //        var validToken = WebEncoders.Base64UrlEncode(encodedToken);

        //        //        DateTime OTPSentTime = DateTime.UtcNow;

        //        //         Send email with OTP
        //        //        var result = await _emailService.SendEmail(new EmailModel()
        //        //        {
        //        //            Subject = "OTP from Knobl for Forget Password",
        //        //            Body = "OTP: " + otp,
        //        //            To = useremail
        //        //        });

        //        //        if (result) //sending email succeeded
        //        //        {
        //        //             Save OTP
        //        //            user.UserOTP = otp;
        //        //            user.UserOTPSentAt = OTPSentTime;
        //        //            var user1 = await _userManager.UpdateAsync(user);

        //        //             Prepare Response
        //        //            var ForgotPasswordResponseModel = new ForgotPasswordResponseModel()
        //        //            {
        //        //                Email = useremail,
        //        //                OTP = otp,
        //        //                Token = token,
        //        //                OTPSentAt = OTPSentTime
        //        //            };

        //        //             Return Success Response
        //        //            response.IsSuccess = true;
        //        //            response.Message = "OTP has been sent on the email.";
        //        //            response.Response = ForgotPasswordResponseModel;
        //        //            return Ok(response);
        //        //        }

        //        //         sending email failed
        //        //        response.Message = "Sending OTP failed. Please try again.";
        //        //        return BadRequest(response);
        //        //    }

        //        //    response.Message = "Bad request";
        //        //    return BadRequest(response);
        //        //}
        //        //catch (Exception exception)
        //        //{
        //        //    response.Message = exception.Message;
        //        //    return BadRequest(response);
        //        //}

        //        return Ok(response);
        //    }

        //    /// </summary>
        //    [HttpGet()]
        //    [Route("Logout")]
        //    [ProducesDefaultResponseType]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Logout()
        //    {
        //        ResponseModel response = new();

        //        try
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                HttpClient _httpClient = new();

        //                string token = Request.Headers["Authorization"].ToString();

        //                // Get Identity Server Base Url
        //                string IdentityServerBaseUrl = _configuration.GetValue<string>("IdentityServer:BaseUrl");

        //                // Validate User and Get Password Token
        //                var tokenResponse = await _httpClient.RevokeTokenAsync(new TokenRevocationRequest
        //                {
        //                    Address = IdentityServerBaseUrl + "/connect/endsession",
        //                    ClientId = "xamarinClient",
        //                    ClientSecret = "secret",
        //                    TokenTypeHint = token
        //                });

        //                // check if response has any error
        //                if (tokenResponse.IsError)
        //                {
        //                    response.Message = tokenResponse.Error;
        //                    return BadRequest(response);
        //                }

        //                //await _signInManager.SignOutAsync();
        //                // Return Success Response
        //                response.IsSuccess = true;
        //                response.Message = "Logout successful!";
        //                return Ok(response);
        //            }

        //            response.Message = "Bad request";
        //            return BadRequest(response);
        //        }
        //        catch (Exception exception)
        //        {
        //            response.Message = exception.Message;
        //            return BadRequest(response);
        //        }

        //    }
        #endregion

        #region ChangePassword
        /// <summary>
        /// POST: api/v1/identityapi/profile/changepassword
        /// Change Password : User must be logged in
        /// </summary>
        [HttpPost()]
        [Route("Change Password")]
        [SwaggerOperation(Summary = "Change Password : User must be logged in", Description = "")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            ResponseModel response = new();

            try
            {
                var data = await _passwordService.ChangePassword(model);
                return Ok(data);
            }
            catch (Exception exception)
            {
                response.Message = exception.Message;
                return BadRequest(response);
            }
        }
        #endregion
    }
}
