using IdentityApi.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityApi.Models.UserManagement
{
    public class PermissionVM
    {

        public List<Role> rolelist { get; set; }
        public List<PermissionModel> permissionModels { get; set; }
    }
    public class PermissionModel
    {
        public long Id { get; set; }
        public string ClaimTypeName { get; set; }
        public string ClaimGroupName { get; set; }
        public string ClaimValue { get; set; }
        [NotMapped]
        public string RoleName { get; set; }

    }
}
