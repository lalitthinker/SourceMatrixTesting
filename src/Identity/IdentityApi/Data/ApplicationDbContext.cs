using IdentityApi.Models.DropDownModel;
using IdentityApi.Models.FavoriteDockModels;
using IdentityApi.Models.ThemeCustomizationsModel;

namespace IdentityApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PermissionMappingRole> MappingRoles { get; set; }
        public DbSet<CustomClaim> CustomClaims { get; set; }
        public DbSet<ClaimType> ClaimTypes { get; set; }
        public DbSet<ClaimGroup> ClaimGroups { get; set; }
        public DbSet<ThemeCustomization> ThemeCustomizations { get; set; }
        public DbSet<FavoriteDock> FavoriteDocks { get; set; }
        public DbSet<UserProfileViewModel> userProfileViewModels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<PermissionModel>().ToView(nameof(PermissionModel)).HasNoKey();
            builder.Entity<RoleVM>().ToView(nameof(RoleVM)).HasNoKey();
            builder.Entity<UserByRoleIdVM>().ToView(nameof(UserByRoleIdVM)).HasNoKey();
            builder.Entity<RequestAllUsersRolesModel>().ToView(nameof(RequestAllUsersRolesModel)).HasNoKey();
            builder.Entity<ClaimValuesVM>().ToView(nameof(ClaimValuesVM)).HasNoKey();
            builder.Entity<ClaimGroupsVM>().ToView(nameof(ClaimGroupsVM)).HasNoKey();
            builder.Entity<ClaimTypesVM>().ToView(nameof(ClaimTypesVM)).HasNoKey();
            builder.Entity<GetAllUserModel>().ToView(nameof(GetAllUserModel)).HasNoKey();
            builder.Entity<ThemeCustomizationViewModel>().ToView(nameof(ThemeCustomizationViewModel)).HasNoKey();
            builder.Entity<FavoriteDockViewModel>().ToView(nameof(FavoriteDockViewModel)).HasNoKey();
            builder.Entity<UserProfileViewModel>().ToView(nameof(UserProfileViewModel)).HasNoKey();
            builder.Entity<DropDownVM>().ToView(nameof(DropDownVM)).HasNoKey();
            builder.Entity<DecryptedDropDownDataModel>().ToView(nameof(DecryptedDropDownDataModel)).HasNoKey();
            builder.Entity<FullNameVM>().ToView(nameof(FullNameVM)).HasNoKey();
            builder.Entity<DecryptedFullNameModel>().ToView(nameof(DecryptedFullNameModel)).HasNoKey();
            builder.Entity<RoleIdModel>().ToView(nameof(RoleIdModel)).HasNoKey();
            builder.Entity<PermissionNameResponseModel>().ToView(nameof(PermissionNameResponseModel)).HasNoKey();
            builder.Entity<GetUserProfileDetailsModel>().ToView(nameof(GetUserProfileDetailsModel)).HasNoKey();
        }

        //public DbSet<PermissionModel> PermissionRoles { get; set; }
        public DbSet<PermissionNameResponseModel> permissionAccessByUsers { get; set; }
        public DbSet<GetUserProfileDetailsModel> getUserProfileDetailsModels { get; set; }
        public DbSet<RoleIdModel> RoleIdsModel { get; set; }
        public DbSet<DropDownVM> DropDownVMs { get; set; }
        public DbSet<DecryptedDropDownDataModel> DecryptedDropDownDataModels { get; set; }
        public DbSet<RoleVM> RoleVMs { get; set; }
        public DbSet<UserByRoleIdVM> UserByRolesId { get; set; }
        public DbSet<RequestAllUsersRolesModel> RequestAllUsersRoles { get; set; }
        public DbSet<ClaimValuesVM> ClaimValuesVMs { get; set; }
        public DbSet<ClaimGroupsVM> CustomGroupsVMs { get; set; }
        public DbSet<ClaimTypesVM> ClaimTypesVMs { get; set; }
        public DbSet<GetAllUserModel> GetAllUserModels { get; set; }
        public DbSet<ThemeCustomizationViewModel> ThemeCustomizationViewModels { get; set; }
        public DbSet<FullNameVM> fullNameVMs { get; set; }

        public DbSet<FavoriteDockViewModel> FavoriteDockViewModels { get; set; }
    }
}
