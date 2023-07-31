namespace IdentityApi.Models.Company
{
    public class CompanyModel
    {
        public string ParentCompanyId { get; set; }
        public long CompanyId { get; set; }
        public int AccountTypeId { get; set; }
        public int AccountClassId { get; set; }
        public int VendorTypeId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string LegalName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string AliasName { get; set; } = string.Empty;
        public string WebSite { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string AccountImageUrl { get; set; } = string.Empty;
        public string PrimaryContactPhone { get; set; } = string.Empty;
        public string Phone2 { get; set; } = string.Empty;
        public string Phone3 { get; set; } = string.Empty;
        public string Fax1 { get; set; } = string.Empty;
        public string Fax2 { get; set; } = string.Empty;
        //public bool HideFormVendorList { get; set; }
        public string Fax3 { get; set; } = string.Empty;
        public string PrimaryContactEmail { get; set; } = string.Empty;
        public string Email2 { get; set; } = string.Empty;
        public string SourcingEmail { get; set; } = string.Empty;
        public string SalesPerson1UserId { get; set; } = string.Empty;
        public string SalesPerson2UserId { get; set; } = string.Empty;
        public string Buyer1UserId { get; set; } = string.Empty;
        public string Buyer2UserId { get; set; } = string.Empty;
        public string PrimaryContactName { get; set; } = string.Empty;
        public int BillingContactId { get; set; }
        public int MailingAddressId { get; set; }
        public int ShippingAddressId { get; set; }
        public int BillingAddressId { get; set; }
        public byte BillingTermsId { get; set; }
        public decimal CreditLimit { get; set; }
        public int CurrencyTypeId { get; set; }
        public int VendorTermId { get; set; }
        public decimal VendorCreditLimit { get; set; }
        public int VendorCurrencyTypeId { get; set; }
        public string Scope { get; set; } = string.Empty;
        public string ReferralSource { get; set; } = string.Empty;
        public int TimezoneId { get; set; }
        public string CompanySize { get; set; } = string.Empty;
        public string Revenue { get; set; } = string.Empty;
        public string TradeEntity { get; set; } = string.Empty;
        public string ResellerId { get; set; } = string.Empty;
        public string DunAndBradstreet { get; set; } = string.Empty;
        public string SICCode { get; set; } = string.Empty;
        public string NAICSCode { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public string VATNumber { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string AccountContactName { get; set; } = string.Empty;
        public bool isContactReminder { get; set; }
        public string BankAccountNumber { get; set; } = string.Empty;
        public string BankAddress { get; set; } = string.Empty;
        public string BankCity { get; set; } = string.Empty;
        public string BankState { get; set; } = string.Empty;
        public string BankPostalCode { get; set; } = string.Empty;
        public string BankCountry { get; set; } = string.Empty;
        public string AccountContactPhoneNumber { get; set; } = string.Empty;
        public string BankFax { get; set; } = string.Empty;
        public bool DoNotCall { get; set; }
        public bool DoNotEmail { get; set; }
        public bool IsAVL { get; set; }
        public bool CreditHold { get; set; }
        public bool IsPublic { get; set; }
        public decimal OverallRating { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string Sales12Month { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string QuickNote { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Stage { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
        public int DandBNumber { get; set; }
        public bool IsExpandable { get; set; }
        public string Industry { get; set; } = string.Empty;
        public string BuyersRep { get; set; } = string.Empty;
        public string VATRFCCNPJ { get; set; } = string.Empty;
        public bool MarkAsVendor { get; set; }
        public bool HideFormVendorList { get; set; }
        public string YTDSales { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public string AccountContactEmailAddress { get; set; } = string.Empty;
        public string Employees { get; set; } = string.Empty;
        public string RecentNotes { get; set; } = string.Empty;
        public string DateContacted { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TotalRecords { get; set; }
    }
}
