namespace IdentityApi.Models.Users
{
    public class SaveUserSettingCommand
    {
        public string UserId { get; set; } = string.Empty;
        public object UserSetting { get; set; }
    }
}
