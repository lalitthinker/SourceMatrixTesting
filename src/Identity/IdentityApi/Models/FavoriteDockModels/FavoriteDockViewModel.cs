namespace IdentityApi.Models.FavoriteDockModels
{
    public class FavoriteDockViewModel
    {
        

        public long Id { get; set; }
        public string UserId { get; set; }
        public int IconId { get; set; }
        public Boolean IsPinned { get; set; }
        public bool IsDeleted { get; set; }
        public string ExternalId { get; set; }
    }
}
