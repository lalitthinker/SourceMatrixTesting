using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace IdentityApi.Data
{
    public class ApplicationDbContextSeed
    {
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();
        private UserManager<ApplicationUser> _userManager;

        private IWebHostEnvironment _env;
        private ApplicationDbContext _applicationDbContext;
        private ILogger<ApplicationDbContextSeed> _logger;

        public async Task SeedAsync(ApplicationDbContext context,
                                    IWebHostEnvironment env,
                                     UserManager<ApplicationUser> userManager,
                                    ILogger<ApplicationDbContextSeed> logger,
                                    int? retry = 0)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _applicationDbContext = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = env ?? throw new ArgumentNullException(nameof(env));
            int retryForAvaiability = retry.Value;

            try
            {

                // Add Store Procedures
                CreateStoreProcedure();

                // Add Functions
                CreateFunctions();

                if (!context.ClaimGroups.Any())
                {
                    await ApplicationDbContextSeedData.GetDefaultClaimGroups(context);
                }

                if (!context.ClaimTypes.Any())
                {
                    await ApplicationDbContextSeedData.GetDefaultClaimTypes(context);
                }

                if (!context.CustomClaims.Any())
                {
                    await ApplicationDbContextSeedData.GetDefaultCustomClaim(context);
                }
                //add roles
                if (!context.Roles.Any())
                {
                    await ApplicationDbContextSeedData.GetDefaultApplicationRole(context);
                }
                //add users
                if (!context.Users.Any())
                {
                    await GetDefaultAdminUserAsync();
                }
                if (!context.ThemeCustomizations.Any())
                {
                    await ApplicationDbContextSeedData.GetDefaultThemeCustomization(context, context.Users.FirstOrDefault().Id);
                }
                if (!context.MappingRoles.Any())
                {
                    await ApplicationDbContextSeedData.GetDefaultPermissionToRole(context, "Admin");
                }

            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(ApplicationDbContext));

                    await SeedAsync(context, _env, _userManager, _logger, retryForAvaiability);
                }
            }
        }
        private async Task<ApplicationUser> GetDefaultAdminUserAsync()
        {

            // Get Encrypted values
            string email = "admin@smx.com";
            string EncryptedEmail = SecurityProvider.EncryptTextAsync(email);
            string EncryptedUserName = SecurityProvider.EncryptUsernameAsync(email);
            string EncryptedFirstName = SecurityProvider.EncryptTextAsync("Admin");
            string EncryptedLastName = SecurityProvider.EncryptTextAsync("User");

            ApplicationUser user = new()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = EncryptedFirstName,
                LastName = EncryptedLastName,
                Email = EncryptedEmail,
                UserName = email,
                NormalizedEmail = EncryptedEmail.Normalize(),
                NormalizedUserName = EncryptedUserName.Normalize(),
                CreatedDate = DateTime.UtcNow,
                IsApproved = true,
                IsDeleted = false,
                IsLoggedIn = false,
                UserRole = "Admin",
                UserStatusId = (int)UserStatus.Active,
                ImageZoomRatio = 0,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, "Pass@word1");
            IdentityResult userResult = await _userManager.CreateAsync(user, "Pass@word1");
            if (userResult.Errors.Any())
            {
                var Message = "Something went wrong. User registration failed!";
                var Errors = userResult.Errors.Select(e => e.Description).ToList();
                _logger.LogError("Something went wrong. User registration failed!", Errors);
                return new();
            }

            IdentityResult roleResult = await _userManager.AddToRoleAsync(user, user.UserRole);
            if (roleResult.Errors.Any())
            {
                var Message = "Something went wrong. Server could not process request.";
                var Errors = roleResult.Errors.Select(e => e.Description).ToList();
                _logger.LogError("Something went wrong. User registration failed!", Errors);
                return new();

            }

            return user;
        }
        #region Seed Stored procedures
        private void CreateStoreProcedure()
        {
            DataTable dtUpdateQuery = new();
            dtUpdateQuery.Columns.Add("Query");
            dtUpdateQuery.Columns.Add("QueryFileName");
            string line;
            string dataLine = "";
            //Console.Write($"--> Web Root Path: {Path.Combine(_env.WebRootPath, "SPs")}");
            Console.Write($"--> Content Root Path: {Path.Combine(_env.ContentRootPath, "SPs")}");
            //string[] files = Directory.GetFiles(Path.Combine(_env.WebRootPath, "SPs"));
            string[] files = Directory.GetFiles(Path.Combine(_env.ContentRootPath, "SPs"));

            for (int i = 0; i < files.Length; i++)
            {
                Console.Write($"\n--> Creating SPs: {files[i]}\n");
                dataLine = "";
                StreamReader file = new(files[i]);
                while ((line = file.ReadLine()) != null)
                {
                    dataLine = dataLine + "\n" + line;
                }
                dtUpdateQuery.Rows.Add(dataLine, files[i]);
                file.Close();
            }
            for (int i = 0; i < dtUpdateQuery.Rows.Count; i++)
            {
                try
                {
                    string query = dtUpdateQuery.Rows[i]["Query"].ToString();
                    DbSpFunction(query);
                }
                catch (Exception ex)
                {
                    string query = dtUpdateQuery.Rows[i]["Query"].ToString();
                    _logger.LogError(ex, $"\nEXCEPTION ERROR while creating store procedure: {query}\n");
                }
            }
        }
        private void DbSpFunction(string query)
        {
            using var command = _applicationDbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            _applicationDbContext.Database.OpenConnection();
            _ = command.ExecuteNonQuery();
        }
        #endregion

        #region Seed Functions
        private void CreateFunctions()
        {
            DataTable dtUpdateQuery = new();
            dtUpdateQuery.Columns.Add("Query");
            dtUpdateQuery.Columns.Add("QueryFileName");
            string line;
            string dataLine = "";
            //Console.Write($"--> Web Root Path: {Path.Combine(_env.WebRootPath, "SPs")}");
            Console.Write($"--> Content Root Path: {Path.Combine(_env.ContentRootPath, "Functions")}");
            //string[] files = Directory.GetFiles(Path.Combine(_env.WebRootPath, "SPs"));
            string[] files = Directory.GetFiles(Path.Combine(_env.ContentRootPath, "Functions"));

            for (int i = 0; i < files.Length; i++)
            {
                Console.Write($"\n--> Creating Functions: {files[i]}\n");
                dataLine = "";
                StreamReader file = new(files[i]);
                while ((line = file.ReadLine()) != null)
                {
                    dataLine = dataLine + "\n" + line;
                }
                dtUpdateQuery.Rows.Add(dataLine, files[i]);
                file.Close();
            }
            for (int i = 0; i < dtUpdateQuery.Rows.Count; i++)
            {
                try
                {
                    string query = dtUpdateQuery.Rows[i]["Query"].ToString();
                    DbFunctFunction(query);
                }
                catch (Exception ex)
                {
                    string query = dtUpdateQuery.Rows[i]["Query"].ToString();
                    _logger.LogError(ex, $"\nEXCEPTION ERROR while creating function: {query}\n");
                }
            }
        }
        private void DbFunctFunction(string query)
        {
            using var command = _applicationDbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            _applicationDbContext.Database.OpenConnection();
            _ = command.ExecuteNonQuery();
        }
        #endregion
        static string[] GetHeaders(string[] requiredHeaders, string csvfile)
        {
            string[] csvheaders = File.ReadLines(csvfile).First().ToLowerInvariant().Split(',');

            if (csvheaders.Length != requiredHeaders.Length)
            {
                throw new Exception($"requiredHeader count '{requiredHeaders.Length}' is different then read header '{csvheaders.Length}'");
            }

            foreach (var requiredHeader in requiredHeaders)
            {
                if (!csvheaders.Contains(requiredHeader))
                {
                    throw new Exception($"does not contain required header '{requiredHeader}'");
                }
            }

            return csvheaders;
        }
    }
}
