using Microsoft.AspNetCore.Http;

namespace IdentityApi.Models
{
    public class UploadImageRequestModel
    {
        public IFormFile File { get; set; }
    }
}
