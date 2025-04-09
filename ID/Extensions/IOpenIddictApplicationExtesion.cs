using ID.Domain.Enums;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using System.Collections.Immutable;
using System.Globalization;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static System.Net.Mime.MediaTypeNames;
using Endpoint = ID.Domain.Enums.Endpoint;

namespace ID.Extensions
{
    public static class IOpenIddictApplicationManagerExtension
    {
        public static string GetClientType(this IOpenIddictApplicationManager manager, ClientType type)
        {
            return type switch
            {
                ClientType.Public => ClientTypes.Public,
                _ => ClientTypes.Confidential
            };
        }

        public static string GetApplicationType(this IOpenIddictApplicationManager manager, ApplicationType type)
        {
            return type switch
            {
                ApplicationType.Native => ApplicationTypes.Native,
                _ => ApplicationTypes.Web
            };
        }

        public static string GetConsentType(this IOpenIddictApplicationManager manager, ConsentType type)
        {
            return type switch
            {
                ConsentType.Implicit => ConsentTypes.Implicit,
                ConsentType.Explicit => ConsentTypes.Explicit,
                _ => ConsentTypes.External
            };
        }

        public static HashSet<string> GetPermissions(this IOpenIddictApplicationManager manager, IEnumerable<Scope>? scopes, IEnumerable<GrantType>? grantTypes, IEnumerable<Endpoint>? endpoints, ResponseType? response)
        {
            var permissions = new HashSet<string>();
            if (scopes != null)
                foreach (var scope in scopes)
                    permissions.Add(GetScopes(manager, scope));

            if (grantTypes != null)
                foreach (var grantType in grantTypes)
                    permissions.Add(GetGrantType(manager, grantType));

            if (endpoints != null)
                foreach (var endpoint in endpoints)
                    permissions.Add(GetEndpoint(manager, endpoint));

            if (response.HasValue)
                permissions.Add(GetResponseType(manager, response.Value));

            return permissions;
        }

        public static string GetScopes(this IOpenIddictApplicationManager manager, Scope scope)
        {
            return scope switch
            {
                Scope.Email => Permissions.Scopes.Email,
                Scope.Profile => Permissions.Scopes.Profile,
                Scope.Phone => Permissions.Scopes.Phone,
                Scope.Address => Permissions.Scopes.Address,
                Scope.OfflineAccess => Permissions.Prefixes.Scope + Scopes.OfflineAccess,
            };
        }

        public static string GetGrantType(this IOpenIddictApplicationManager manager, GrantType type)
        {
            return type switch
            {
                GrantType.AuthorizationCode => Permissions.GrantTypes.AuthorizationCode,
                GrantType.Implicit => Permissions.GrantTypes.Implicit,
                GrantType.ClientCredentials => Permissions.GrantTypes.ClientCredentials,
                GrantType.ResourceOwnerPassword => Permissions.GrantTypes.Password,
                GrantType.RefreshToken => Permissions.GrantTypes.RefreshToken,
                GrantType.DeviceCode => Permissions.GrantTypes.DeviceCode,
            };
        }

        public static string GetEndpoint(this IOpenIddictApplicationManager manager, Endpoint endpoint)
        {
            return endpoint switch
            {
                Endpoint.Authorization => Permissions.Endpoints.Authorization,
                Endpoint.Token => Permissions.Endpoints.Token,
                Endpoint.Introspection => Permissions.Endpoints.Introspection,
                Endpoint.Revocation => Permissions.Endpoints.Revocation,
                Endpoint.EndSession => Permissions.Endpoints.EndSession,
                Endpoint.PushedAuthorization => Permissions.Endpoints.PushedAuthorization,
                Endpoint.DeviceAuthorization => Permissions.Endpoints.DeviceAuthorization,
            };
        }

        public static string GetResponseType(this IOpenIddictApplicationManager manager, ResponseType type)
        {
            return type switch
            {
                ResponseType.CodeIdToken => Permissions.ResponseTypes.CodeIdToken,
                ResponseType.CodeIdTokenToken => Permissions.ResponseTypes.CodeIdTokenToken,
                ResponseType.IdTokenToken => Permissions.ResponseTypes.IdTokenToken,
                ResponseType.IdToken => Permissions.ResponseTypes.IdToken,
                ResponseType.None => Permissions.ResponseTypes.None,
                ResponseType.Token => Permissions.ResponseTypes.Token,
            };
        }

        public static string GetRequirementsType(this IOpenIddictApplicationManager manager, Requerment type)
        {
            return type switch
            {
                Requerment.ProofKeyForCodeExchange => Requirements.Features.ProofKeyForCodeExchange,
                Requerment.PushedAuthorizationRequests => Requirements.Features.PushedAuthorizationRequests,
            };
        }


        public static ClientType GetClientType(this IOpenIddictApplicationManager manager, string type)
        {
            return type switch
            {
                ClientTypes.Public => ClientType.Public,
                _ => ClientType.Confidential
            };
        }

        public static ApplicationType GetApplicationType(this IOpenIddictApplicationManager manager, string type)
        {
            return type switch
            {
                ApplicationTypes.Native => ApplicationType.Native,
                _ => ApplicationType.Web
            };
        }

        public static ConsentType GetConsentType(this IOpenIddictApplicationManager manager, string type)
        {
            return type switch
            {
                ConsentTypes.Implicit => ConsentType.Implicit,
                ConsentTypes.Explicit => ConsentType.Explicit,
                _ => ConsentType.External
            };
        }

        /*public static HashSet<string> GetPermissions(this IOpenIddictApplicationManager manager, IEnumerable<Scope>? scopes, IEnumerable<GrantType>? grantTypes, IEnumerable<Domain.Enums.Endpoint>? endpoints, ResponseType? response)
        {
            var permissions = new HashSet<string>();
            if (scopes != null)
                foreach (var scope in scopes)
                    permissions.Add(GetScopes(manager, scope));

            if (grantTypes != null)
                foreach (var grantType in grantTypes)
                    permissions.Add(GetGrantType(manager, grantType));

            if (endpoints != null)
                foreach (var endpoint in endpoints)
                    permissions.Add(GetEndpoint(manager, endpoint));

            if (response.HasValue)
                permissions.Add(GetResponseTypes(manager, response.Value));

            return permissions;
        }*/

        public static Scope GetScopes(this IOpenIddictApplicationManager manager, string scope)
        {
            return scope switch
            {
                Permissions.Prefixes.Scope + Scopes.OpenId => Scope.OpenId,
                Permissions.Scopes.Email => Scope.Email,
                Permissions.Scopes.Profile => Scope.Profile,
                Permissions.Scopes.Phone => Scope.Phone,
                Permissions.Scopes.Address => Scope.Address,
                Permissions.Prefixes.Scope + Scopes.OfflineAccess => Scope.OfflineAccess,
            };
        }

        public static GrantType GetGrantType(this IOpenIddictApplicationManager manager, string type)
        {
            return type switch
            {
                Permissions.GrantTypes.AuthorizationCode => GrantType.AuthorizationCode,
                Permissions.GrantTypes.Implicit => GrantType.Implicit,
                Permissions.GrantTypes.ClientCredentials => GrantType.ClientCredentials,
                Permissions.GrantTypes.Password => GrantType.ResourceOwnerPassword,
                Permissions.GrantTypes.RefreshToken => GrantType.RefreshToken,
                Permissions.GrantTypes.DeviceCode => GrantType.DeviceCode,
            };
        }

        public static Endpoint GetEndpoint(this IOpenIddictApplicationManager manager, string endpoint)
        {
            return endpoint switch
            {
                Permissions.Endpoints.Authorization => Endpoint.Authorization,
                Permissions.Endpoints.Token => Endpoint.Token,
                Permissions.Endpoints.Introspection => Endpoint.Introspection,
                Permissions.Endpoints.Revocation => Endpoint.Revocation,
                Permissions.Endpoints.EndSession => Endpoint.EndSession,
                Permissions.Endpoints.PushedAuthorization => Endpoint.PushedAuthorization,
                Permissions.Endpoints.DeviceAuthorization => Endpoint.DeviceAuthorization,
            };
        }

        public static ResponseType GetResponseType(this IOpenIddictApplicationManager manager, string type)
        {
            return type switch
            {
                Permissions.ResponseTypes.CodeIdToken => ResponseType.CodeIdToken,
                Permissions.ResponseTypes.CodeIdTokenToken => ResponseType.CodeIdTokenToken,
                Permissions.ResponseTypes.IdTokenToken => ResponseType.IdTokenToken,
                Permissions.ResponseTypes.IdToken => ResponseType.IdToken,
                Permissions.ResponseTypes.None => ResponseType.None,
                Permissions.ResponseTypes.Token => ResponseType.Token,
            };
        }

        public static Requerment GetRequirementsType(this IOpenIddictApplicationManager manager, string type)
        {
            return type switch
            {
                Requirements.Features.ProofKeyForCodeExchange => Requerment.ProofKeyForCodeExchange,
                Requirements.Features.PushedAuthorizationRequests => Requerment.PushedAuthorizationRequests,
                _ => Requerment.None
            };
        }

        public static SecreteKeyStatus GetSecreteKeyStatus(this IOpenIddictApplicationManager manager, object application)
        {
            var app = application as dynamic;
            if (app.ClientSecret == null)
                return SecreteKeyStatus.UnSet;
            return SecreteKeyStatus.Set;
        }

        public static async Task<OpenIddictApplicationModel> AsModelApplication(this IOpenIddictApplicationManager manager, object application)
        {
            return new OpenIddictApplicationModel
            {
                Id = await manager.GetIdAsync(application),
                ClientId = await manager.GetClientIdAsync(application),
                ClientType = await manager.GetClientTypeAsync(application),
                ApplicationType = await manager.GetApplicationTypeAsync(application),
                ConsentType = await manager.GetConsentTypeAsync(application),
                DisplayNames = await manager.GetDisplayNamesAsync(application),
                RedirectUris = await manager.GetRedirectUrisAsync(application),
                PostLogoutRedirectUris = await manager.GetPostLogoutRedirectUrisAsync(application),
                Permissions = await manager.GetPermissionsAsync(application),
                Requerments = await manager.GetRequirementsAsync(application)
            };
        }

        public class OpenIddictApplicationModel
        {
            public string? Id { get; set; }
            public string? ClientId { get; set; }
            public string? ClientType { get; set; }
            public string? ApplicationType { get; set; }
            public string? ConsentType { get; set; }
            public ImmutableDictionary<CultureInfo, string> DisplayNames { get; set; }
            public ImmutableArray<string> RedirectUris { get; set; }
            public ImmutableArray<string> PostLogoutRedirectUris { get; set; }
            public ImmutableArray<string> Permissions { get; set; }
            public ImmutableArray<string> Requerments { get; set; }
        }
    }
}
