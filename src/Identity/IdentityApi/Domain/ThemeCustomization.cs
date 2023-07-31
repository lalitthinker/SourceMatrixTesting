using IdentityApi.Models.Entity;

namespace IdentityApi.Domain
{
    public class ThemeCustomization : EntityBase
    {
        public string UserId { get; set; }
        public string PrimaryMenus { get; set; }
        public string SecondaryMenus { get; set; }
        public string FavoriteDocks { get; set; }
        public string SiteWides { get; set; }
    }
}
