using System;
using IdentityApi.Configuration;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;

namespace IdentityApi.Data
{
    public class ConfigurationDbContextSeed
    {
        public async Task SeedAsync(ConfigurationDbContext context)
        {

            var clientUrls = new Dictionary<string, string>();

            clientUrls.Add("IdentityAPI", AppConstants.BaseUrl.IdentityAPI);
            clientUrls.Add("SourceMatrixAPI", AppConstants.BaseUrl.SourceMatrixAPI);
            clientUrls.Add("ResourceAPI", AppConstants.BaseUrl.ResourceAPI);

            if (!context.Clients.Any())
            {
                foreach (IdentityServer4.Models.Client client in Config.GetClients(clientUrls))
                {
                    _ = context.Clients.Add(client.ToEntity());
                }
                _ = await context.SaveChangesAsync();
            }
            // Checking always for old redirects to fix existing deployments
            // to use new swagger-ui redirect uri as of v3.0.0
            // There should be no problem for new ones
            // ref: https://github.com/dotnet-architecture/eShopOnContainers/issues/586
            else
            {
                List<ClientRedirectUri> oldRedirects = (await context.Clients.Include(c => c.RedirectUris).ToListAsync())
                    .SelectMany(c => c.RedirectUris)
                    .Where(ru => ru.RedirectUri.EndsWith("/o2c.html"))
                    .ToList();

                if (oldRedirects.Any())
                {
                    foreach (ClientRedirectUri ru in oldRedirects)
                    {
                        ru.RedirectUri = ru.RedirectUri.Replace("/o2c.html", "/oauth2-redirect.html");
                        _ = context.Update(ru.Client);
                    }
                    _ = await context.SaveChangesAsync();
                }
            }

            if (!context.IdentityResources.Any())
            {
                foreach (IdentityServer4.Models.IdentityResource resource in Config.GetResources())
                {
                    _ = context.IdentityResources.Add(resource.ToEntity());
                }
                _ = await context.SaveChangesAsync();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (IdentityServer4.Models.ApiScope api in Config.GetApiScope())
                {
                    _ = context.ApiScopes.Add(api.ToEntity());
                }

                _ = await context.SaveChangesAsync();
            }

        }
    }
}
