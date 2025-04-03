using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace ID.Extesions
{
    public static class ModelBuilderExtension
    {
        public static void Seeds(this ModelBuilder modelBuilder, IOpenIddictApplicationManager manager)
        {
            ClientsSeed.Seed(manager);
        }
    }

    public static class ClientsSeed
    {
        public static void Seed(IOpenIddictApplicationManager manager)
        {
            manager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "client",
                ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
                DisplayName = "Client application",
                PostLogoutRedirectUris = { new Uri("http://localhost:53507/signout-callback-oidc") },
                RedirectUris = { new Uri("http://localhost:53507/signin-oidc") },
                Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.EndSession,
                        OpenIddictConstants.Permissions.Endpoints.Token
                    }
            });
        }
    }
}
