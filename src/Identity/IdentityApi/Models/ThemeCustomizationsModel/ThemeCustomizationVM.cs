namespace IdentityApi.Models.ThemeCustomizationsModel
{
    public class ThemeCustomizationVM
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string PrimaryMenus { get; set; } = string.Empty;
        public string SecondaryMenus { get; set; } = string.Empty;
        public string FavoriteDocks { get; set; } = string.Empty;
        public string SiteWides { get; set; } = string.Empty;
    }
    public class ThemeCustomizationDataVM
    {
        public PrimaryMenu PrimaryMenus { get; set; }
        public SecondaryMenu SecondaryMenus { get; set; }
        public FavoritesDock FavoriteDocks { get; set; }
        public SiteWide SiteWides { get; set; }
    }
    public class PrimaryMenu
    {
        public string BackgroundColor { get; set; } 
        public string HoverAndActiveTextAndIconColor { get; set; }
        public string SiteLogo { get; set; }
        public string TextAndIconColor { get; set; } 
    }

    public class SecondaryMenu
    {
        public string BackgroundColor { get; set; } = string.Empty;
        public string TextAndIconColor { get; set; } = string.Empty;
        public string HoverAndActiveTextAndIconColor { get; set; } = string.Empty;
        public string AccentLineColor { get; set; } = string.Empty;
    }

    public class FavoritesDock
    {
        public string BackgroundColor { get; set; } = string.Empty;
        public string IconColor { get; set; } = string.Empty;
    }

    public class SiteWide
    {
        public string PrimaryAccentColor { get; set; } = string.Empty;
        public string SecondaryAccentColor { get; set; } = string.Empty;
        public string HoverAndActiveColor { get; set; } = string.Empty;
    }
}
