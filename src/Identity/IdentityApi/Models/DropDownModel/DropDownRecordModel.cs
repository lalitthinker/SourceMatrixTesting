namespace IdentityApi.Models.DropDownModel
{
    public class DropDownRecordModel
    {
        public DropDownRecordModel()
        {
            SaleCommissionsList = new List<DropDownVM>();
            PurchaseCommissionsList = new List<DropDownVM>();
        }
        public List<DropDownVM> SaleCommissionsList { get; set; }
        public List<DropDownVM> PurchaseCommissionsList { get; set; }
    }
}
