namespace IdentityApi.Domain
{
    public class PermissionMappingRole
    { 
        public long  Id { get; set; }
        public string ApplicationRoleId { get; set; }
        public virtual ApplicationRole ApplicationRole { get; set; }
        public string Name { get; set; }    
        public int CustomClaimsId { get; set; }
        public virtual CustomClaim CustomClaims { get; set; }

       


    }
}
