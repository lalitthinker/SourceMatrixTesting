using System.ComponentModel;

namespace IdentityApi.Models.Enums
{
    public enum AgeRange
    {
        [Description("18 - 25")]
        Between18_25 = 0,

        [Description("26 - 35")]
        Between26_35 = 1,

        [Description("36 - 50")]
        Between36_50 = 2,

        [Description("51 - 65")]
        Between51_65 = 3,

        [Description("Over 65")]
        Over65 = 4,

        [Description("Not required")]
        NotRequired = 5,
    }
}
