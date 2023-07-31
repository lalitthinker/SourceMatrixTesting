using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdentityApi.Services.Accounts;
using IdentityApi.Services.CommonSp;
using IdentityApi.Services.DropDown;
using IdentityApi.Services.Excel;
using IdentityApi.Services.FavoriteDocks;
using IdentityApi.Services.PDF;
using IdentityApi.Services.Permission;
using IdentityApi.Services.Roles;
using IdentityApi.Services.ThemeCustomizations;
using IdentityApi.Services.User;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace IdentityApi
{
    public class Startup
    {
        public IConfiguration _configuration { get; }
        public readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            _configuration.GetSection(nameof(BaseUrl)).Bind(AppConstants.BaseUrl);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            _ = ConfigureCors(services, _env);
            var connectionString = _configuration["ConnectionString"];
            // Add framework services.
            _ = services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    }));

            _ = services.AddIdentity<ApplicationUser, ApplicationRole>()
                 .AddEntityFrameworkStores<ApplicationDbContext>()
                 .AddDefaultTokenProviders();

            // custom error message for HTTP 400 response (Bad Request)
            _ = services.AddMvc()
                //.SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var problems = new CustomBadRequest(context);
                        return new BadRequestObjectResult(problems);
                    };
                });

            _ = services.AddTransient<ILoginService<ApplicationUser>, EFLoginService>();
            _ = services.AddTransient<IRedirectService, RedirectService>();
            _ = services.AddTransient<IProfileService, ProfileService>();
            _ = services.AddTransient<IEmailService, EmailService>();
            _ = services.AddScoped<ICommonService, CommonService>();
            _ = services.AddScoped<IPDFService, PDFService>();
            _ = services.AddScoped<IUserService, UserService>();
            _ = services.AddScoped<IPDFCommonSettings, PDFCommonSettings>();
            _ = services.AddScoped<IPermissionService, PermissionService>();
            _ = services.AddScoped<IThemeCustomizationService, ThemeCustomizationService>();
            _ = services.AddScoped<IHttpApiRequests, HttpApiRequests>();
            _ = services.AddScoped<IRoleService, RoleService>();
            _ = services.AddScoped<IFavoriteDockService, FavoriteDockService>();
            _ = services.AddScoped<IDropDownService, DropDownService>();
            _ = services.AddScoped<IAccountService, AccountService>();
            _ = services.AddScoped<IExcelService, ExcelService>();
            _ = services.AddScoped<ICommonSPServices, CommonSPServices>();
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            // Adds IdentityServer
            _ = services.AddIdentityServer(x =>
            {
                x.IssuerUri = "null";
                x.Authentication.CookieLifetime = TimeSpan.FromHours(2);
            })
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<ProfileService>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString,
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            _ = sqlOptions.MigrationsAssembly(migrationsAssembly);
                            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                            _ = sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString,
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            _ = sqlOptions.MigrationsAssembly(migrationsAssembly);
                            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                            _ = sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
                });


            _ = services.AddAuthentication(IISDefaults.AuthenticationScheme);

            _ = services.AddControllers();
            _ = services.AddControllersWithViews();
            _ = services.AddRazorPages();
            _ = services.AddSwaggerGen();

            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity.API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });
        }

        private static IServiceCollection ConfigureCors(IServiceCollection services, IWebHostEnvironment env)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy(AppConstants.CorsPolicyName, builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();

                    //if (env.IsDevelopment())
                    //{
                    //    builder.AllowAnyOrigin()
                    //           .AllowAnyHeader()
                    //           .AllowAnyMethod();
                    //}
                    //else
                    //{
                    //    builder.WithOrigins(AppConstants.BaseUrl.AdminWeb)
                    //           .AllowAnyHeader()
                    //           .AllowAnyMethod()
                    //           .AllowCredentials();
                    //}
                });
            });
        }
    }
}