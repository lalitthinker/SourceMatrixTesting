using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace IdentityApi.Models
{
    public class ProfileRequestModel
    {
        public List<IFormFile> ProfilePictureFile { get; set; } 
    }
}