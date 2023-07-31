namespace IdentityApi.Models.Permission
{
    public class PermisssionModel
    {
        public List<ClaimTypesName> claimTypesNameslist { get; set; }

    }

    public class ClaimTypesName
    {
        public ClaimTypesName()
        {
            claimGroupNamelist = new List<ClaimGroupName>();
        }

        public string ClaimTypeName { get; set; }
        public int ClaimTypeId { get; set; }
        public bool IsAllowed => claimGroupNamelist.Where(x => x.IsAllowed).Count() == claimGroupNamelist.Count;
        public int GroupCount => claimGroupNamelist.Count;
        public bool IsExpandable { get; set; }
        public bool IsEdited { get; set; }
         //public int PermissionSwitch => claimGroupNamelist.FirstOrDefault().AccessCount;
        public int PermissionSwitch => claimGroupNamelist.Where(x=> x.AccessCount == 1).Count() != 0 && claimGroupNamelist.Where(x => x.AccessCount == 0).Count() != 0 ? 2 : claimGroupNamelist.FirstOrDefault().AccessCount == 0 ? 0 : 1;
        public List<ClaimGroupName> claimGroupNamelist { get; set; }
    }

    public class ClaimGroupName
    {
        public int ClaimGroupId { get; set; }
        public string ClaimGroupNames { get; set; }
        public int AccessCount => claimValuesList.Where(x => x.IsAccess == true).Count() != 0 && claimValuesList.Where(x => x.IsAccess == false).Count() != 0 ? 2 : claimValuesList.FirstOrDefault().IsAccess == false ? 0 : 1;
        public bool IsAllowed => claimValuesList.Where(x => x.IsAccess).Count() == claimValuesList.Count;
        public bool IsExpandable { get; set; }
        public int ClaimTypeId { get; set; }
        public List<ClaimValue> claimValuesList { get; set; }
    }

    public class ClaimValue
    {
        public bool IsAccess { get; set; }
        public string ClaimValues { get; set; }
        public int CustomClaimId { get; set; }
        public int ClaimTypeId { get; set; }
    }
}
