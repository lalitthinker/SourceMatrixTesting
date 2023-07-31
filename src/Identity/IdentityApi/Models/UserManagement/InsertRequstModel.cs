namespace IdentityApi.Models.UserManagement
{
    public class InsertRequstModel 
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
        public List<int> CustomClaimsId { get; set; }
    }
}
