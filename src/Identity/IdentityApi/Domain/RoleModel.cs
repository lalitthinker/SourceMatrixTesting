using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityApi.Domain
{
    public class RoleModel
    {
        public int Id { get; set; }
    }
    public class Role
    {
        public int Srno { get; set; }
        [NotMapped]
        public string Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string RoleColor { get; set; } = string.Empty;
        [NotMapped]
        public bool IsActive { get; set; }
        public bool IsSystemRole { get; set; } = false;
    }
    public class RoleDelect
    {

        public string RoleId { get; set; }
    }
}
