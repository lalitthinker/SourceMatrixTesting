using IdentityApi.Models.ThemeCustomizationsModel;
using Newtonsoft.Json;
using System.IO;

namespace IdentityApi.Data
{
    public class ApplicationDbContextSeedData
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();

        #region default claim groups
        public static async Task GetDefaultClaimGroups(ApplicationDbContext applicationDbContext)
        {
            applicationDbContext.ClaimGroups.AddRange(new List<ClaimGroup>()
            {
                 new() {Name = "Access" },
                 new() {Name = "Account Control" },
                 new() {Name = "Bills" },
                 new() {Name = "Checks" },
                 new() {Name = "Credit Memos" },
                 new() {Name = "General" },
                 new() {Name = "Notes" },
                 new() {Name = "Ownership" },
                 new() {Name = "Users" },
                 new() {Name = "Roles" },
            });
            await applicationDbContext.SaveChangesAsync();
        }
        #endregion

        #region default claim types
        public static async Task GetDefaultClaimTypes(ApplicationDbContext applicationDbContext)
        {
            applicationDbContext.ClaimTypes.AddRange(new List<ClaimType>()
            {
                new() {Name = "Accounting"},
                new() {Name = "Companies"},
                new() {Name = "Employee Management"},
                new() {Name = "Inventory Management"},
                new() {Name = "Invoice Management"},
                new() {Name = "Main Page Layout"},
                new() {Name = "Offers Management"},
                new() {Name = "Purchasing Management"},
                new() {Name = "Quote Management"},
                new() {Name = "Reports"},
                new() {Name = "Requirements Management"},
                new() {Name = "RMA Management"},
                new() {Name = "Sales Order Management"},
                new() {Name = "Show Messages"},
                new() {Name = "System Access"},
                new() {Name = "Vender Returns Management"},
                new() {Name = "Shipping Receiving Management"},
                new() {Name = "Admin"},
                new() {Name = "Parts Management"},
                new() {Name = "Sourcing Center Management"},
                new() {Name = "Work Order Management"},
                new() {Name = "Task Management"},
                new() {Name = "Proforma Management"}
            });
            await applicationDbContext.SaveChangesAsync();
        }
        #endregion

        #region default custom claims
        public static async Task GetDefaultCustomClaim(ApplicationDbContext applicationDbContext)
        {
            applicationDbContext.CustomClaims.AddRange(new List<CustomClaim>()
            {
                new() {ClaimTypeId = 1 , ClaimGroupId = 3 , ClaimValue = "Can View Bills"},
                new() {ClaimTypeId = 1 , ClaimGroupId = 3 , ClaimValue = "Can Edit Bills"},
                new() {ClaimTypeId = 1 , ClaimGroupId = 3 , ClaimValue = "Call Add Bills"},
                new() {ClaimTypeId = 1 , ClaimGroupId = 3 , ClaimValue = "Can Delete Bills"},
                new() {ClaimTypeId = 1 , ClaimGroupId = 4 , ClaimValue = "Can View Check"},
                new() {ClaimTypeId = 1 , ClaimGroupId = 4 , ClaimValue = "Can Edit Check"},
                new() {ClaimTypeId = 1 , ClaimGroupId = 4 , ClaimValue = "Can Add Check"},
                new() {ClaimTypeId = 1 , ClaimGroupId = 4 , ClaimValue = "Can Delete Check"},
                new() {ClaimTypeId = 1 , ClaimGroupId = 5 , ClaimValue = "Can View Credit Memo"},
                new() {ClaimTypeId = 1 , ClaimGroupId = 5 , ClaimValue = "Can Edit Credit Memo"},
                new() {ClaimTypeId = 1 , ClaimGroupId = 5 , ClaimValue = "Can Add Credit Memo"},
                new() {ClaimTypeId = 1 , ClaimGroupId = 5 , ClaimValue = "Can Delete Credit Memo"},
                new() {ClaimTypeId = 1 , ClaimGroupId = 5 , ClaimValue = "Can Authorize Credit Memo"},
                new() {ClaimTypeId = 2 , ClaimGroupId = 1 , ClaimValue = "Can View Company"},
                new() {ClaimTypeId = 2 , ClaimGroupId = 1 , ClaimValue = "Can Add Company"},
                new() {ClaimTypeId = 2 , ClaimGroupId = 1 , ClaimValue = "Can Edit Company"},
                new() {ClaimTypeId = 2 , ClaimGroupId = 1 , ClaimValue = "Can Delete Company"},
                new() {ClaimTypeId = 2 , ClaimGroupId = 2 , ClaimValue = "Can Authorize New Company"},
                new() {ClaimTypeId = 2 , ClaimGroupId = 2 , ClaimValue = "Can Place on Credit Hold"},
                new() {ClaimTypeId = 2 , ClaimGroupId = 8 , ClaimValue = "Can View Public Accounts"},
                new() {ClaimTypeId = 2 , ClaimGroupId = 8 , ClaimValue = "Can View House Accounts"},
                new() {ClaimTypeId = 2 , ClaimGroupId = 8 , ClaimValue = "Can Change House Account"},
                new() {ClaimTypeId = 2 , ClaimGroupId = 8 , ClaimValue = "Can Change Sales Rep"},
                new() {ClaimTypeId = 2 , ClaimGroupId = 7 , ClaimValue = "Can Add Company Note"},
                new() {ClaimTypeId = 2 , ClaimGroupId = 7 , ClaimValue = "Can Edit Company Note"},
                new() {ClaimTypeId = 2 , ClaimGroupId = 7 , ClaimValue = "Can Delete Company Note"},
                new() {ClaimTypeId = 2 , ClaimGroupId = 7 , ClaimValue = "Can Print Company Note"},
                new() {ClaimTypeId = 3 , ClaimGroupId = 6 , ClaimValue = "Can View Employees"},
                new() {ClaimTypeId = 3 , ClaimGroupId = 6 , ClaimValue = "Can Add Employees"},
                new() {ClaimTypeId = 3 , ClaimGroupId = 6 , ClaimValue = "Can Edit Employees"},
                new() {ClaimTypeId = 3 , ClaimGroupId = 6 , ClaimValue = "Can Delete Employees"},
                new() {ClaimTypeId = 4 , ClaimGroupId = 6 , ClaimValue = "Can View Inventory"},
                new() {ClaimTypeId = 4 , ClaimGroupId = 6 , ClaimValue = "Can Add Inventory"},
                new() {ClaimTypeId = 4 , ClaimGroupId = 6 , ClaimValue = "Can Edit Inventory"},
                new() {ClaimTypeId = 4 , ClaimGroupId = 6 , ClaimValue = "Can Delete Inventory"},
                new() {ClaimTypeId = 4 , ClaimGroupId = 6 , ClaimValue = "Can Upload Data"},
                new() {ClaimTypeId = 4 , ClaimGroupId = 6 , ClaimValue = "Can Export Data "},
                new() {ClaimTypeId = 4 , ClaimGroupId = 6 , ClaimValue = "Can Upload Images"},
                new() {ClaimTypeId = 4 , ClaimGroupId = 6 , ClaimValue = "Can Delete Images"},
                new() {ClaimTypeId = 4 , ClaimGroupId = 6 , ClaimValue = "Can Limit GL Accounts View"},
                new() {ClaimTypeId = 5 , ClaimGroupId = 6 , ClaimValue = "Can Access Invoices"},
                new() {ClaimTypeId = 5 , ClaimGroupId = 6 , ClaimValue = "Can Add Invoices"},
                new() {ClaimTypeId = 5 , ClaimGroupId = 6 , ClaimValue = "Can Edit Invoices"},
                new() {ClaimTypeId = 5 , ClaimGroupId = 6 , ClaimValue = "Can View All Invoices"},
                new() {ClaimTypeId = 5 , ClaimGroupId = 6 , ClaimValue = "Can Authorize Invoices"},
                new() {ClaimTypeId = 5 , ClaimGroupId = 6 , ClaimValue = "Can Delete Invoices"},
                new() {ClaimTypeId = 5 , ClaimGroupId = 6 , ClaimValue = "Can Print PickTicket"},
                new() {ClaimTypeId = 5 , ClaimGroupId = 6 , ClaimValue = "Can Limit GL Accounts View"},
                new() {ClaimTypeId = 5 , ClaimGroupId = 6 , ClaimValue = "Can Create Charge Description"},
                new() {ClaimTypeId = 6 , ClaimGroupId = 6 , ClaimValue = "Admin"},
                new() {ClaimTypeId = 6 , ClaimGroupId = 6 , ClaimValue = "Accouting"},
                new() {ClaimTypeId = 6 , ClaimGroupId = 6 , ClaimValue = "Sales"},
                new() {ClaimTypeId = 6 , ClaimGroupId = 6 , ClaimValue = "Purchasing"},
                new() {ClaimTypeId = 7 , ClaimGroupId = 6 , ClaimValue = "Can Access Offers"},
                new() {ClaimTypeId = 7 , ClaimGroupId = 6 , ClaimValue = "Can Add Offers"},
                new() {ClaimTypeId = 7 , ClaimGroupId = 6 , ClaimValue = "Can Edit Offers"},
                new() {ClaimTypeId = 7 , ClaimGroupId = 6 , ClaimValue = "Can Delete Offers"},
                new() {ClaimTypeId = 7 , ClaimGroupId = 6 , ClaimValue = "Can View All Offers"},
                new() {ClaimTypeId = 8 , ClaimGroupId = 6 , ClaimValue = "Can Access/View PO's"},
                new() {ClaimTypeId = 8 , ClaimGroupId = 6 , ClaimValue = "Can Edit PO's"},
                new() {ClaimTypeId = 8 , ClaimGroupId = 6 , ClaimValue = "Can Add PO's"},
                new() {ClaimTypeId = 8 , ClaimGroupId = 6 , ClaimValue = "Can Authorize PO's"},
                new() {ClaimTypeId = 8 , ClaimGroupId = 6 , ClaimValue = "Can Recieve PO's"},
                new() {ClaimTypeId = 8 , ClaimGroupId = 6 , ClaimValue = "Can View All PO's"},
                new() {ClaimTypeId = 8 , ClaimGroupId = 6 , ClaimValue = "Can Delete PO's"},
                new() {ClaimTypeId = 8 , ClaimGroupId = 6 , ClaimValue = "Can Authorize PO changes"},
                new() {ClaimTypeId = 8 , ClaimGroupId = 6 , ClaimValue = "Can Create Charge Description"},
                new() {ClaimTypeId = 8 , ClaimGroupId = 6 , ClaimValue = "Can Limit GL Accounts View"},
                new() {ClaimTypeId = 9 , ClaimGroupId = 6 , ClaimValue = "Can Add Quotes"},
                new() {ClaimTypeId = 9 , ClaimGroupId = 6 , ClaimValue = "Can Edit Quotes"},
                new() {ClaimTypeId = 9 , ClaimGroupId = 6 , ClaimValue = "Can Access Quotes"},
                new() {ClaimTypeId = 9 , ClaimGroupId = 6 , ClaimValue = "Can Delete Quotes"},
                new() {ClaimTypeId = 9 , ClaimGroupId = 6 , ClaimValue = "Can View All Quote"},
                new() {ClaimTypeId = 9 , ClaimGroupId = 6 , ClaimValue = "Can View All Offer Lists"},
                 new() {ClaimTypeId = 10 , ClaimGroupId = 6 , ClaimValue = "Can Access Reports"},
                 new() {ClaimTypeId = 10 , ClaimGroupId = 6 , ClaimValue = "Accounting Reports"},
                 new() {ClaimTypeId = 10 , ClaimGroupId = 6 , ClaimValue = "Company_Financial"},
                 new() {ClaimTypeId = 10 , ClaimGroupId = 6 , ClaimValue = "Customers_Recievable"},
                 new() {ClaimTypeId = 10 , ClaimGroupId = 6 , ClaimValue = "Inventory"},
                 new() {ClaimTypeId = 10 , ClaimGroupId = 6 , ClaimValue = "Purchase Order Reports"},
                 new() {ClaimTypeId = 10 , ClaimGroupId = 6 , ClaimValue = "Vendors_Payables"},
                 new() {ClaimTypeId = 10 , ClaimGroupId = 6 , ClaimValue = "Sales Order Reports"},
                 new() {ClaimTypeId = 10 , ClaimGroupId = 6 , ClaimValue = "Listings"},
                 new() {ClaimTypeId = 11 , ClaimGroupId = 6 , ClaimValue = "Can Access Active Reqs"},
                 new() {ClaimTypeId = 11 , ClaimGroupId = 6 , ClaimValue = "Can Add Requirements"},
                 new() {ClaimTypeId = 11 , ClaimGroupId = 6 , ClaimValue = "Can Edit Requirements"},
                 new() {ClaimTypeId = 11 , ClaimGroupId = 6 , ClaimValue = "Can Delete Requirements"},
                 new() {ClaimTypeId = 11 , ClaimGroupId = 6 , ClaimValue = "Can View All Requirements"},
                 new() {ClaimTypeId = 12 , ClaimGroupId = 6 , ClaimValue = "Can Access RMA's"},
                 new() {ClaimTypeId = 12 , ClaimGroupId = 6 , ClaimValue = "Can Add RMA's"},
                 new() {ClaimTypeId = 12 , ClaimGroupId = 6 , ClaimValue = "Can View All RMA's"},
                 new() {ClaimTypeId = 12 , ClaimGroupId = 6 , ClaimValue = "Can Authorize RMA's"},
                 new() {ClaimTypeId = 12 , ClaimGroupId = 6 , ClaimValue = "Can Delete RMA's"},
                 new() {ClaimTypeId = 12 , ClaimGroupId = 6 , ClaimValue = "Can Recieve RMA's"},
                 new() {ClaimTypeId = 12 , ClaimGroupId = 6 , ClaimValue = "Can Upload RMA Receive"},
                 new() {ClaimTypeId = 13 , ClaimGroupId = 6 , ClaimValue = "Can Access Sales Orders"},
                 new() {ClaimTypeId = 13 , ClaimGroupId = 6 , ClaimValue = "Can View All Sales Orders"},
                 new() {ClaimTypeId = 13 , ClaimGroupId = 6 , ClaimValue = "Can Add Sales Order"},
                 new() {ClaimTypeId = 13 , ClaimGroupId = 6 , ClaimValue = "Can Edit Sales Order"},
                 new() {ClaimTypeId = 13 , ClaimGroupId = 6 , ClaimValue = "Can Authorize Sales Orders"},
                 new() {ClaimTypeId = 13 , ClaimGroupId = 6 , ClaimValue = "Can Delete Sales Orders"},
                 new() {ClaimTypeId = 13 , ClaimGroupId = 6 , ClaimValue = "Can Create Charge Description"},
                 new() {ClaimTypeId = 13 , ClaimGroupId = 6 , ClaimValue = "Can Limit GL Accounts View"},
                 new() {ClaimTypeId = 15 , ClaimGroupId = 6 , ClaimValue = "Can Access Settings"},
                 new() {ClaimTypeId = 14 , ClaimGroupId = 6 , ClaimValue = "Authorized New Company"},
                 new() {ClaimTypeId = 14 , ClaimGroupId = 6 , ClaimValue = "Part Posted to Found Part"},
                 new() {ClaimTypeId = 14 , ClaimGroupId = 6 , ClaimValue = "Authorized PO"},
                 new() {ClaimTypeId = 14 , ClaimGroupId = 6 , ClaimValue = "Authorized SO"},
                 new() {ClaimTypeId = 14 , ClaimGroupId = 6 , ClaimValue = "Order Status Notes Changed"},
                 new() {ClaimTypeId = 14 , ClaimGroupId = 6 , ClaimValue = "Recieved PO"},
                 new() {ClaimTypeId = 14 , ClaimGroupId = 6 , ClaimValue = "Authorized Invoice"},
                 new() {ClaimTypeId = 14 , ClaimGroupId = 6 , ClaimValue = "Shipping Confirmation "},
                 new() {ClaimTypeId = 14 , ClaimGroupId = 6 , ClaimValue = "Call Tracker"},
                 new() {ClaimTypeId = 14 , ClaimGroupId = 6 , ClaimValue = "Warehouse Message"},
                 new() {ClaimTypeId = 16 , ClaimGroupId = 6 , ClaimValue = "Can Access Vendor Returns"},
                 new() {ClaimTypeId = 16 , ClaimGroupId = 6 , ClaimValue = "Can View All Vendor Returns"},
                 new() {ClaimTypeId = 16 , ClaimGroupId = 6 , ClaimValue = "Can Add Vendor Returns"},
                 new() {ClaimTypeId = 16 , ClaimGroupId = 6 , ClaimValue = "Can Create Vendor Returns"},
                 new() {ClaimTypeId = 16 , ClaimGroupId = 6 , ClaimValue = "Can Delete Vendor Returns"},
                 new() {ClaimTypeId = 16 , ClaimGroupId = 6 , ClaimValue = "Can Authorize Vendor Returns"},
                 new() {ClaimTypeId = 16 , ClaimGroupId = 6 , ClaimValue = "Can Edit Vendor Returns"},
                 new() {ClaimTypeId = 17 , ClaimGroupId = 6 , ClaimValue = "Can Access/View Shipping Center"},
                 new() {ClaimTypeId = 18 , ClaimGroupId = 9 , ClaimValue = "Can Manage Users"},
                 new() {ClaimTypeId = 18 , ClaimGroupId = 10 , ClaimValue = "Can Manage Roles"},
                 new() {ClaimTypeId = 18 , ClaimGroupId = 6 , ClaimValue = "Can Manage Settings"},
                 new() {ClaimTypeId = 19 , ClaimGroupId = 6 , ClaimValue = "Can View Parts"},
                 new() {ClaimTypeId = 19 , ClaimGroupId = 6 , ClaimValue = "Can Add Parts"},
                 new() {ClaimTypeId = 19 , ClaimGroupId = 9 , ClaimValue = "Can Edit Parts"},
                 new() {ClaimTypeId = 19 , ClaimGroupId = 6 , ClaimValue = "Can Delete Parts"},
                 new() {ClaimTypeId = 20 , ClaimGroupId = 6 , ClaimValue = "Can View Sourcing Center"},
                 new() {ClaimTypeId = 20 , ClaimGroupId = 6 , ClaimValue = "Can Add Sourcing Center"},
                 new() {ClaimTypeId = 20 , ClaimGroupId = 6 , ClaimValue = "Can Edit Sourcing Center"},
                 new() {ClaimTypeId = 20 , ClaimGroupId = 6 , ClaimValue = "Can Delete Sourcing Center"},
                 new() {ClaimTypeId = 21 , ClaimGroupId = 6 , ClaimValue = "Can Add Work Order"},
                 new() {ClaimTypeId = 21 , ClaimGroupId = 6 , ClaimValue = "Can Authorize Work Orders"},
                 new() {ClaimTypeId = 21 , ClaimGroupId = 6 , ClaimValue = "Can Delete Work Orders"},
                 new() {ClaimTypeId = 21 , ClaimGroupId = 6 , ClaimValue = "Can Edit Work Order"},
                 new() {ClaimTypeId = 21 , ClaimGroupId = 6 , ClaimValue = "Can View All Work Orders"},
                 new() {ClaimTypeId = 12 , ClaimGroupId = 6 , ClaimValue = "Can Edit RMA's"},
                 new() {ClaimTypeId = 18 , ClaimGroupId = 6 , ClaimValue = "Can Add Lookup"},
                 new() {ClaimTypeId = 18 , ClaimGroupId = 6 , ClaimValue = "Can Edit Lookup"},
                  new() {ClaimTypeId = 1 , ClaimGroupId = 6 , ClaimValue = "Can Post Payments"},
                  new() {ClaimTypeId = 1 , ClaimGroupId = 6 , ClaimValue = "Can Manage Email Template"},
                  new() {ClaimTypeId = 22 , ClaimGroupId = 6 , ClaimValue = "Can Manage Tasks"},
                  new() {ClaimTypeId = 2 , ClaimGroupId = 1 , ClaimValue = "Can View All Companies"},
                  new() {ClaimTypeId = 20 , ClaimGroupId = 6 , ClaimValue = "Can Access Sourcing Center"},
                  new() {ClaimTypeId = 23 , ClaimGroupId = 6 , ClaimValue = "Can Access Proforma"},
                 new() {ClaimTypeId = 23 , ClaimGroupId = 6 , ClaimValue = "Can Add Proforma"},
                 new() {ClaimTypeId = 23 , ClaimGroupId = 6 , ClaimValue = "Can Edit Proforma"},
                 new() {ClaimTypeId = 23 , ClaimGroupId = 6 , ClaimValue = "Can Delete Proforma"},
                 new() {ClaimTypeId = 21 , ClaimGroupId = 6 , ClaimValue = "Can View All Proforma"},
                 new() {ClaimTypeId = 2 , ClaimGroupId = 6 , ClaimValue = "Can Access Work Order"},
                 new() {ClaimTypeId = 18 , ClaimGroupId = 6 , ClaimValue = "Can Delete Lookup"},

            });
            await applicationDbContext.SaveChangesAsync();
        }
        #endregion

        #region default theme customization
        public static async Task GetDefaultThemeCustomization(ApplicationDbContext dbContext, string UserId)
        {

            string text = File.ReadAllText(@"./ThemeCustomizations.json");
            using (StreamReader r = new StreamReader(@"./ThemeCustomizations.json"))
            {
                string json = r.ReadToEnd();
                var data = JsonConvert.DeserializeObject<ThemeCustomizationDataVM>(text);
                string jsonPrimaryMenus = Newtonsoft.Json.JsonConvert.SerializeObject(data.PrimaryMenus, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
                string jsonSecondaryMenus = Newtonsoft.Json.JsonConvert.SerializeObject(data.SecondaryMenus, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
                string jsonFavoriteDocks = Newtonsoft.Json.JsonConvert.SerializeObject(data.FavoriteDocks, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });
                string jsonsSiteWides = Newtonsoft.Json.JsonConvert.SerializeObject(data.SiteWides, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });

                dbContext.ThemeCustomizations.Add(new ThemeCustomization()
                {
                    UserId = UserId,
                    PrimaryMenus = jsonPrimaryMenus,
                    SecondaryMenus = jsonSecondaryMenus,
                    FavoriteDocks = jsonFavoriteDocks,
                    SiteWides = jsonsSiteWides
                });
            }
            await dbContext.SaveChangesAsync();
        }
        #endregion

        #region default application role
        public static async Task GetDefaultApplicationRole(ApplicationDbContext context)
        {
            List<ApplicationRole> roles = new()
            {
                 new () { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "Admin".Normalize(),IsSystemRole = true, RoleColor = "#0067AC",IsActive = true },
                 new() { Id = Guid.NewGuid().ToString(), Name = "SalesPerson", NormalizedName = "SalesPerson".Normalize(),IsSystemRole = false, RoleColor = "#DE466D",IsActive = true },
                 new() { Id = Guid.NewGuid().ToString(), Name = "Buyers", NormalizedName = "Buyers".Normalize(), IsSystemRole = false, RoleColor = "#8B9DAA",IsActive = true }
            };
            context.MappingRoles.Select(a => a.CustomClaimsId).ToList();
            using RoleStore<ApplicationRole> roleStore = new(context);

            foreach (var role in roles)
            {
                await roleStore.CreateAsync(role);
                if (role.Name == "Admin")
                {
                    await GetDefaultPermissionToRole(context, role.Id);

                }
            }

            await context.SaveChangesAsync();
        }
        #endregion

        #region default permission to role 
        public static async Task GetDefaultPermissionToRole(ApplicationDbContext context, string RoleName)
        {
            List<PermissionMappingRole> model = new List<PermissionMappingRole>();
            var getAllPermission = context.CustomClaims.Select(a => a.Id).ToList();
            var getAdminRoleId = context.Roles.Where(a => a.Name == "Admin").FirstOrDefault()?.Id;
            using RoleStore<ApplicationRole> roleStore = new(context);
            foreach (var customIds in getAllPermission)
            {
                {
                    model.Add(new PermissionMappingRole
                    {
                        ApplicationRoleId = getAdminRoleId,
                        CustomClaimsId = customIds,
                        Name = "Admin"
                    });
                }
            }
            context.MappingRoles.AddRange(model);
            await context.SaveChangesAsync();

        }
        #endregion
    }
}
