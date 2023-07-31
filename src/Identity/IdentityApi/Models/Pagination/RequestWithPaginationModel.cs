namespace IdentityApi.Models.Pagination
{
    public class RequestWithPaginationModel : PaginationModel
    {
        public bool IsAdminUsersRequired { set; get; }
        public string UserId { get; set; }
    }
}
