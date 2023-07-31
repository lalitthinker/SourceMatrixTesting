namespace IdentityApi.Helper
{
    public class ResourcePath
    {

        private const string ResourceBaseUrl = "http://3.18.144.73:6083";

        public static string GetUrl(string filePath, string fileName)
        {
            string resourcename = string.IsNullOrEmpty(fileName) ? "default.png" : fileName;
            return $"{ResourceBaseUrl}{filePath}{resourcename}";
        }

        public static string GetThumbUrl(string filePath, string fileName)
        {
            string resourcename = string.IsNullOrEmpty(fileName) ? "default.png" : fileName;
            return $"{ResourceBaseUrl}{filePath}Thumbnails/thumb_{resourcename}";
        }

       
    }

    public static class ResourceType
    {
        public const string UserProfileImage = @"/Uploads/Profiles/";
        public const string UserCoverImage = @"/Uploads/Profiles/";
        public const string Documents = @"/Uploads/Documents/";
    }
}
