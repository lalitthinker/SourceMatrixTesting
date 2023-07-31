using IdentityApi.Models.Pagination;

namespace IdentityApi.Models.Users
{
    public class RequestUserByRoleIdModel : PaginationModel
    {
        public string RoleId { get; set; }

        public int Userstatus { get; set; }
    }
}
