using Fido2NetLib;
using ID.Configurations;
using ID.Data;
using ID.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

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
                .AddTotpTokenProviders()
                .AddDefaultTokenProviders();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".ID.Session";
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.None; 
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

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
                           .SetDeviceAuthorizationEndpointUris("/connect/device")
                           .SetEndUserVerificationEndpointUris("/connect/verify")
                           .SetUserInfoEndpointUris("/connect/userinfo")
                           .SetIntrospectionEndpointUris("/connect/introspect")
                           .SetRevocationEndpointUris("/connect/revoke")
                           .SetPushedAuthorizationEndpointUris("connect/par")
                           .IgnoreEndpointPermissions()
                           .IgnoreResponseTypePermissions();

                    options.AllowPasswordFlow()
                    .AllowRefreshTokenFlow()
                    .AllowClientCredentialsFlow()
                    .AllowDeviceAuthorizationFlow()
                    .AllowAuthorizationCodeFlow();

                    options.SetRefreshTokenLifetime(TimeSpan.FromDays(3))
                    .SetAccessTokenLifetime(TimeSpan.FromHours(1))
                    .SetAuthorizationCodeLifetime(TimeSpan.FromMinutes(1));
                    //.DisableTokenStorage();
                    //options.EnableDegradedMode(); // Для device authorization flow

                    options.AddDevelopmentSigningCertificate();
                    options.AddDevelopmentEncryptionCertificate();
                    options.DisableAccessTokenEncryption();

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
                          .EnableUserInfoEndpointPassthrough()
                          .EnableEndUserVerificationEndpointPassthrough();

                    if (builder.Environment.IsDevelopment())
                        options.UseAspNetCore()
                        .DisableTransportSecurityRequirement();
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            });
            builder.Services.AddAuthorization();
            Fido2Configuration(builder);
            return builder;
        }

        private static void Fido2Configuration(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IFido2>(sp =>
            {
                var config = new Fido2Configuration
                {
                    ServerDomain = builder.Configuration["Fido2:ServerDomain"],
                    ServerName = builder.Configuration["Fido2:ServerName"],
                    Origin = "https://localhost:7253/", // обязательно совпадает с тем, откуда фронт
                    TimestampDriftTolerance = 300000 // например, 5 минут
                };

                return new Fido2(config);
            });
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
