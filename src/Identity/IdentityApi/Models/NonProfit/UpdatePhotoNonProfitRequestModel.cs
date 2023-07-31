namespace IdentityApi.Models.NonProfit
{
    public class UpdatePhotoNonProfitRequestModel
    {
        public long FoundationId { get; set; }
        public string UserId { get; set; }

        public bool IsChangedCoverPictureName { get; set; }
        public string CoverPictureName { get; set; }

        public bool IsChangedProfilePictureName { get; set; }
        public string ProfilePictureName { get; set; }
    }
}
