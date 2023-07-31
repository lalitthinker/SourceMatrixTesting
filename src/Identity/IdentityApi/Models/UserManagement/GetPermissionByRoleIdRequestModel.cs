using IdentityApi.Models.Pagination;

namespace IdentityApi.Models.UserManagement
{
    public class GetPermissionByRoleIdRequestModel : PaginationVM
    {
        public string ApplicationRoleId { get; set; }   
    }
}
