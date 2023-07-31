using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityApi.Models.UserManagement
{
    public class PermissionSPVM
    {
    }
    public class ClaimGroupsVM
    {
        public int Id { get; set; }
        public int ClaimTypeId { get; set; }
        public string ClaimTypeName { get; set; }
        public int ClaimGroupId { get; set; }
        public string ClaimGroupName { get; set; }
        public string ClaimValue { get; set; }
        public bool IsAllowed { get; set; }
        public bool IsExpandable { get; set; }
        [NotMapped]
        public bool IsEdited { get; set; }
        public int PermissionSwitch { get; set; }
    }

    public class ClaimTypesVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ClaimValuesVM
    {
        public int Id { get; set; }
        public int ClaimGroupId { get; set; }
        public string ClaimValue { get; set; }
    }
}
