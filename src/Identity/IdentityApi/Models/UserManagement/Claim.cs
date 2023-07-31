namespace IdentityApi.Models.UserManagement
{
    public partial class CustomClaim
    {
        public int Id { get; set; }
        public int ClaimTypeId { get; set; }
        public virtual ClaimType ClaimType { get; set; }
        public int ClaimGroupId { get; set; }
        public virtual ClaimGroup ClaimGroup { get; set; }
        public string ClaimValue { get; set; }
    }

    public partial class ClaimGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public partial class ClaimType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
