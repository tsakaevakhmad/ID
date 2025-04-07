using ID.Commands;
using ID.Domain.Enums;
using MediatR;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Endpoint = ID.Domain.Enums.Endpoint;

namespace ID.Handlers.Admin
{
    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, CreateClientCommandResponse>
    {
        private readonly IOpenIddictApplicationManager _manager;

        public CreateClientCommandHandler(IOpenIddictApplicationManager manager)
        {
            _manager = manager;
        }

        public async Task<CreateClientCommandResponse> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var application = new OpenIddictApplicationDescriptor
                {
                    ClientId = request.ClientId,
                    ClientSecret = request.ClientType == ClientType.Public ? null : request.ClientSecret,
                    ClientType = GetClientType(request.ClientType),
                    ApplicationType = GetApplicationType(request.ApplicationType),
                    ConsentType = GetConsentType(request.ConsentType),
                };

                foreach (var requerment in request.Requerments)
                    application.Requirements.Add(GetRequermentTypes(requerment));

                foreach (var displayName in request?.DisplayNames)
                    application.DisplayNames.Add(new System.Globalization.CultureInfo(displayName.Culture), displayName.Name);

                foreach (var redirectUri in request?.PostLogoutRedirectUris)
                    application.RedirectUris.Add(new Uri(redirectUri));

                foreach (var redirectUri in request?.RedirectUris)
                    application.RedirectUris.Add(new Uri(redirectUri));
                
                foreach (var permisssion in GetPermissions(request.Scopes, request.GrantTypes, request.Endpoints, request.ResponseType))
                    application.Permissions.Add(permisssion);
                
                await _manager.CreateAsync(application);
                
                return new CreateClientCommandResponse
                {
                    ClientId = request.ClientId,
                    ClientSecret = request.ClientSecret,
                    RedirectUris = request.RedirectUris.ToList(),
                    PostLogoutRedirectUri = request.PostLogoutRedirectUris.FirstOrDefault()
                };
            }
            catch
            {
                throw;
            }
        }

        public string GetClientType(ClientType type)
        {
            return type switch
            {
                ClientType.Public => OpenIddictConstants.ClientTypes.Public,
                _ => OpenIddictConstants.ClientTypes.Confidential
            };
        }

        public string GetApplicationType(ApplicationType type)
        {
            return type switch
            {
                ApplicationType.Native => OpenIddictConstants.ApplicationTypes.Native,
                _ => OpenIddictConstants.ApplicationTypes.Web
            };
        }

        public string GetConsentType(ConsentType type)
        {
            return type switch
            {
                ConsentType.Implicit => OpenIddictConstants.ConsentTypes.Implicit,
                ConsentType.Explicit => OpenIddictConstants.ConsentTypes.Explicit,
                _ => OpenIddictConstants.ConsentTypes.External
            };
        }

        public HashSet<string> GetPermissions(IEnumerable<Scope>? scopes, IEnumerable<GrantType>? grantTypes, IEnumerable<Endpoint>? endpoints, ResponseType? response)
        {
            var permissions = new HashSet<string>();
            if (scopes != null)
                foreach (var scope in scopes)
                    permissions.Add(GetScopes(scope));

            if (grantTypes != null)
                foreach (var grantType in grantTypes)
                    permissions.Add(GetGrantType(grantType));

            if (endpoints != null)
                foreach (var endpoint in endpoints)
                    permissions.Add(GetEndpoint(endpoint));
            
            if(response.HasValue)
                permissions.Add(GetResponseTypes(response.Value));

            return permissions;
        }

        public string GetScopes(Scope scope)
        {
            return scope switch
            {
                Scope.Email => OpenIddictConstants.Permissions.Scopes.Email,
                Scope.Profile => OpenIddictConstants.Permissions.Scopes.Profile,
                Scope.Phone => OpenIddictConstants.Permissions.Scopes.Phone,
                Scope.Address => OpenIddictConstants.Permissions.Scopes.Address,
                Scope.OfflineAccess => OpenIddictConstants.Permissions.Prefixes.Scope + Scopes.OfflineAccess,
            };
        }

        public string GetGrantType(GrantType type)
        {
            return type switch
            {
                GrantType.AuthorizationCode => OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                GrantType.Implicit => OpenIddictConstants.Permissions.GrantTypes.Implicit,
                GrantType.ClientCredentials => OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                GrantType.ResourceOwnerPassword => OpenIddictConstants.Permissions.GrantTypes.Password,
                GrantType.RefreshToken => OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                GrantType.DeviceCode => OpenIddictConstants.Permissions.GrantTypes.DeviceCode,
            };
        }

        public string GetEndpoint(Endpoint endpoint)
        {
            return endpoint switch
            {
                Endpoint.Authorization => OpenIddictConstants.Permissions.Endpoints.Authorization,
                Endpoint.Token => OpenIddictConstants.Permissions.Endpoints.Token,
                Endpoint.Introspection => OpenIddictConstants.Permissions.Endpoints.Introspection,
                Endpoint.Revocation => OpenIddictConstants.Permissions.Endpoints.Revocation,
                Endpoint.EndSession => OpenIddictConstants.Permissions.Endpoints.EndSession,
                Endpoint.PushedAuthorization => OpenIddictConstants.Permissions.Endpoints.PushedAuthorization,
                Endpoint.DeviceAuthorization => OpenIddictConstants.Permissions.Endpoints.DeviceAuthorization,
            };
        }

        public string GetResponseTypes(ResponseType type)
        {
            return type switch
            {
                ResponseType.CodeIdToken => OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken,
                ResponseType.CodeIdTokenToken => OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken,
                ResponseType.IdTokenToken => OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken,
                ResponseType.IdToken => OpenIddictConstants.Permissions.ResponseTypes.IdToken,
                ResponseType.None => OpenIddictConstants.Permissions.ResponseTypes.None,
                ResponseType.Token => OpenIddictConstants.Permissions.ResponseTypes.Token,
            };
        }

        public string GetRequermentTypes(Requerment type)
        {
            return type switch
            {
                Requerment.ProofKeyForCodeExchange => OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange,
                Requerment.PushedAuthorizationRequests => OpenIddictConstants.Requirements.Features.PushedAuthorizationRequests,
            };
        }
    }
}
