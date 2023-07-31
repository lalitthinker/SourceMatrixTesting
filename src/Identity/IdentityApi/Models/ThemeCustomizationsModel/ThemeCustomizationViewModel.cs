namespace IdentityApi.Models.ThemeCustomizationsModel
{
    public class ThemeCustomizationViewModel
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string PrimaryMenu { get; set; } = string.Empty;
        public string SecondaryMenu { get; set; } = string.Empty;
        public string FavoriteDock { get; set; } = string.Empty;
        public string SiteWides { get; set; } = string.Empty;
        public string ExternalId { get; set; }
    }
}
