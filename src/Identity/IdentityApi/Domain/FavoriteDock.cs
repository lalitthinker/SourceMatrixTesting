using IdentityApi.Models.Entity;

namespace IdentityApi.Domain
{
    public class FavoriteDock : EntityBase
    {
        public string UserId { get; set; }
        public int IconId { get; set; }
        public Boolean IsPinned { get; set; }
    }
}
