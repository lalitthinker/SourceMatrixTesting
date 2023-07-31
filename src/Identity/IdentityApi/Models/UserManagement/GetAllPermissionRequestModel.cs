using IdentityApi.Models.Pagination;

namespace IdentityApi.Models.UserManagement
{
    public class GetAllPermissionRequestModel : RequestWithPaginationModel
    {
        public string RoleId { get; set; }
    }
}
