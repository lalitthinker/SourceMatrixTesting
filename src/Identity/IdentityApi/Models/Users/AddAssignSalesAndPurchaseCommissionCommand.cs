namespace IdentityApi.Models.Users
{
    public class AddAssignSalesAndPurchaseCommissionCommand
    {
        public List<string> UserIds { get; set; }
        public string SaleCommissionId { get; set; } = string.Empty;
        public string PurchaseCommissionId { get; set; } = string.Empty;
    }
}
