using Microsoft.AspNetCore.Authorization;

namespace IdentityApi.Controllers
{
    //[Authorize("ClientIdPolicy")]
    [Route("api/v1/IdentityApi/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
    }
}
