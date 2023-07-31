using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Models.Permission
{
    public class PermissionMappingRoleCreateModel
    {

        public string RoleId { get; set; }
        public string Name { get; set; }
        public int CustomClaimsId { get; set; }
    }
}
