namespace IdentityApi.Models.FavoriteDockModels
{
    public class FavoriteDockVM
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public int IconId { get; set; }
        public Boolean IsPinned { get; set; }

    }
}
