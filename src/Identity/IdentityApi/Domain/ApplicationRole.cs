using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Domain
{
    public class ApplicationRole : IdentityRole<string>
    {
        public ApplicationRole() : base()
        {
            IsActive = true;
        }
        public ApplicationRole(string name, bool isActive) : base(name)
        {
            IsActive = isActive;
        }
        public bool IsActive { get; set; } = true;
        public bool IsSystemRole { get; set; }
        public string RoleColor { get; internal set; } = string.Empty;
    }
}
