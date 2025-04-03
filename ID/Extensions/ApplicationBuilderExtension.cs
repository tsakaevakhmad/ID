﻿using ID.Data;
using ID.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using System.Runtime.CompilerServices;

namespace ID.Extesions
{
    public static class ApplicationBuilderExtension
    {
        public static WebApplicationBuilder OpenIddictSettings(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<PgDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddOpenIddict()
                .AddCore(options =>
                    {
                        options.UseEntityFrameworkCore()
                               .UseDbContext<PgDbContext>();
                    })
                .AddServer(options =>
                {
                    options.SetAuthorizationEndpointUris("/connect/authorize")
                           .SetTokenEndpointUris("/connect/token")
                           .SetUserInfoEndpointUris("/connect/userinfo")
                           .SetIntrospectionEndpointUris("/connect/introspect")
                           .SetRevocationEndpointUris("/connect/revoke")
                           .SetPushedAuthorizationEndpointUris("connect/par")
                           .IgnoreEndpointPermissions()
                           .IgnoreResponseTypePermissions();

                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow();
                    options.AllowClientCredentialsFlow();
                    options.AllowAuthorizationCodeFlow();
                    options.SetRefreshTokenLifetime(TimeSpan.FromDays(3));
                    options.SetAccessTokenLifetime(TimeSpan.FromHours(1));

                    options.DisableTokenStorage();

                    options.AddDevelopmentEncryptionCertificate()
                           .AddDevelopmentSigningCertificate();

                    options.RequireProofKeyForCodeExchange();
                    options.RequirePushedAuthorizationRequests();

                    options.RegisterScopes(
                        OpenIddictConstants.Scopes.OpenId,
                        OpenIddictConstants.Scopes.Email,
                        OpenIddictConstants.Scopes.OfflineAccess,
                        OpenIddictConstants.Scopes.Profile,
                        OpenIddictConstants.Scopes.Address,
                        OpenIddictConstants.Scopes.Phone,
                        OpenIddictConstants.Scopes.Roles
                        );
                    options.UseAspNetCore()
                          .EnableTokenEndpointPassthrough()
                          .EnableAuthorizationEndpointPassthrough()
                          .EnableUserInfoEndpointPassthrough();

                    options.AcceptAnonymousClients();
                });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });
            builder.Services.AddAuthorization();
            return builder;
        }

        public static IApplicationBuilder Migrate(this IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PgDbContext>();
                dbContext.Database.Migrate();
            }

            return applicationBuilder;
        }
    }
}
