using IdentityApi.Models.DropDownModel;
using IdentityApi.Services.DropDown;

namespace IdentityApi.Controllers
{
    public class DropDownController : BaseController
    {
        #region fields
        private readonly ApplicationDbContext _context;
        private readonly IDropDownService _dropDownService;

        #endregion

        #region ctor
        public DropDownController(ApplicationDbContext context, IDropDownService dropDownService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dropDownService = dropDownService ?? throw new ArgumentNullException(nameof(dropDownService));
        }
        #endregion

        #region GetUsersWithSalesPersonRoleList
        [HttpGet()]
        [Route("GetUsersWithSalesPersonRoleList")]
        public async Task<IActionResult> GetUsersWithSalesPersonRoleList()
        {
            try
            {
                var query = @"exec [dbo].[sp_GetAllUserWithSalesPersonRole]";
                List<DropDownVM> companyData = await (_context.DropDownVMs.FromSqlRaw(query)!.ToListAsync());
                List<DecryptedDropDownDataModel> dropDownData = new();
                foreach (var item in companyData)
                {
                    List<string> FullName = item.Name.Split(' ').ToList();
                    string FirstName = FullName.FirstOrDefault();
                    string LastName = FullName.LastOrDefault();

                    string DecryptedFirstName = SecurityProvider.DecryptTextAsync(FirstName);
                    string DecryptedLastName = SecurityProvider.DecryptTextAsync(LastName);
                    string DecryptedFullName = DecryptedFirstName + " " + DecryptedLastName;
                    string id = item.Id;
                    DecryptedDropDownDataModel model = new()
                    {
                        Id = id,
                        Name = DecryptedFullName,
                        LabelName = item.LabelName
                    };
                    dropDownData.Add(model);
                }
                return Ok(dropDownData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region GetUsersWithAltSalesPersonRoleList
        [HttpGet()]
        [Route("GetUsersWithAltSalesPersonRoleList")]
        public async Task<IActionResult> GetUsersWithAltSalesPersonRoleList()
        {
            try
            {
                var query = @"exec [dbo].[sp_GetAllUserWithAltSalesPersonRole]";
                List<DropDownVM> companyData = await (_context.DropDownVMs.FromSqlRaw(query)!.ToListAsync());
                List<DecryptedDropDownDataModel> dropDownData = new();
                foreach (var item in companyData)
                {
                    List<string> FullName = item.Name.Split(' ').ToList();
                    string FirstName = FullName.FirstOrDefault();
                    string LastName = FullName.LastOrDefault();

                    string DecryptedFirstName = SecurityProvider.DecryptTextAsync(FirstName);
                    string DecryptedLastName = SecurityProvider.DecryptTextAsync(LastName);
                    string DecryptedFullName = DecryptedFirstName + " " + DecryptedLastName;
                    string id = item.Id;
                    DecryptedDropDownDataModel model = new()
                    {
                        Id = id,
                        Name = DecryptedFullName,
                        LabelName = item.LabelName
                    };
                    dropDownData.Add(model);
                }
                return Ok(dropDownData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region GetUsersWithFullName
        [HttpGet()]
        [Route("GetUsersWithFullName")]
        public async Task<IActionResult> GetUsersWithFullName()
        {
            try
            {
                var query = @"exec [dbo].[sp_GetUsersWithFullName]";
                List<FullNameVM> Data = await (_context.fullNameVMs.FromSqlRaw(query)!.ToListAsync());
                List<DecryptedFullNameModel> dropDownData = new();
                foreach (var item in Data)
                {
                    string firstname = SecurityProvider.DecryptTextAsync(item.FirstName);
                    string lastname = SecurityProvider.DecryptTextAsync(item.LastName);
                    string id = item.Id;
                    DecryptedFullNameModel model = new()
                    {
                        Id = id,
                        FirstName = firstname,
                        LastName = lastname
                    };
                    dropDownData.Add(model);
                }
                return Ok(dropDownData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region GetUsersWithBuyerRoleList
        [HttpGet()]
        [Route("GetUsersWithBuyerRoleList")]
        public async Task<IActionResult> GetUsersWithBuyerRoleList()
        {
            try
            {
                var query = @"exec [dbo].[sp_GetAllUserWithBuyersRole]";
                List<DropDownVM> companyData = await (_context.DropDownVMs.FromSqlRaw(query)!.ToListAsync());
                List<DecryptedDropDownDataModel> dropDownData = new();
                foreach (var item in companyData)
                {
                    List<string> FullName = item.Name.Split(' ').ToList();
                    string FirstName = FullName.FirstOrDefault();
                    string LastName = FullName.LastOrDefault();

                    string DecryptedFirstName = SecurityProvider.DecryptTextAsync(FirstName);
                    string DecryptedLastName = SecurityProvider.DecryptTextAsync(LastName);
                    string DecryptedFullName = DecryptedFirstName + " " + DecryptedLastName;
                    string id = item.Id;
                    DecryptedDropDownDataModel model = new()
                    {
                        Id = id,
                        Name = DecryptedFullName,
                        LabelName = item.LabelName
                    };
                    dropDownData.Add(model);
                }
                return Ok(dropDownData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region GetAllDropDownList
        [HttpGet]
        [SwaggerOperation(Summary = "|  Get dropdown list.", Description = "")]
        [Route("GetAllDropDownList")]
        public async Task<ActionResult<ResponseModel>> GetAllDropDownList()
        {
            return await _dropDownService.GetAllDropDownList();
        }
        #endregion

    }
}
