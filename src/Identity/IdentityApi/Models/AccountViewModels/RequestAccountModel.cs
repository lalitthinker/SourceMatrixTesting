using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;

namespace IdentityApi.Models.AccountViewModels
{
    public class RequestAccountModel : PaginationModel
    {
        public string Id { get; set; }
        public int Userstatus { get; set; }
        public List<string> SelectedId { get; set; }
        public string RoleId { get; set; }
        public bool ExportAsPdf { get; set; }
    }
}
