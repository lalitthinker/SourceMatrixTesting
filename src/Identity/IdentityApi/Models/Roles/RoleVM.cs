namespace IdentityApi.Models.Roles
{
    public class RoleVM
    {
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string RoleColor { get; set; }
        public bool IsSystemRole { get; set; }
        public bool IsActive { get; set; }
        public string TotalUsers { get; set; }
        public int AllUsers { get; set; }
    }
}
