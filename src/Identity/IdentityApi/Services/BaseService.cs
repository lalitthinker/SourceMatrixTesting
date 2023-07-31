using IdentityApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;

namespace IdentityApi.Services
{
    public class BaseService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpcontextAccessor;

        public BaseService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpcontextAccessor)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _httpcontextAccessor = httpcontextAccessor ?? throw new ArgumentNullException(nameof(httpcontextAccessor));
        }

        protected ApplicationUser GetCurrentUser()
        {
            try
            {
                if (_httpcontextAccessor.HttpContext == null)
                {
                    return _userManager.Users.FirstOrDefault(w => w.UserName == "admin@aegiscomponents.com");
                }
                //#endif

                ClaimsPrincipal user = _httpcontextAccessor.HttpContext.User;
                return _userManager.Users.FirstOrDefault(w => w.UserName == user.FindFirstValue("username"));
                //return await _userManager.FindByNameAsync(user.FindFirstValue("username"));
            }
            catch (Exception e)
            {
                Console.Write(e);
                throw new ArgumentNullException("Problem getting user info");
                //throw;

            }
        }
    }
}
