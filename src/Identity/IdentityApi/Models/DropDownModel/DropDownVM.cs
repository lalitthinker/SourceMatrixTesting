using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityApi.Models.DropDownModel
{
    public class DropDownVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LabelName { get; set; } = string.Empty;
        [NotMapped]
        public bool IsActive { get; set; } = false;
        [NotMapped]
        public string ColorCode { get; set; } = string.Empty;
    }

    public class FullNameVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
