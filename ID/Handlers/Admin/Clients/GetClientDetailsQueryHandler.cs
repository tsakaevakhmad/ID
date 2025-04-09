using ID.Domain.Dto.Admin;
using ID.Domain.Enums;
using ID.Extensions;
using ID.Queries.Admin.Clients;
using MediatR;
using OpenIddict.Abstractions;

namespace ID.Handlers.Admin.Clients
{
    public class GetClientDetailsQueryHandler : IRequestHandler<GetClientDetailsQuery, ClientDto>
    {
        private readonly IOpenIddictApplicationManager _applicationManager;

        public GetClientDetailsQueryHandler(IOpenIddictApplicationManager applicationManager) 
        {
            _applicationManager = applicationManager;
        }

        public async Task<ClientDto> Handle(GetClientDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var application = await _applicationManager.FindByIdAsync(request.Id);
                var permissions = await _applicationManager.GetPermissionsAsync(application);
                return new ClientDto
                {
                    Id = await _applicationManager.GetIdAsync(application),
                    ClientId = await _applicationManager.GetClientIdAsync(application),
                    ClientSecret = _applicationManager.GetSecreteKeyStatus(application).ToString(),
                    ClientType = _applicationManager.GetClientType(await _applicationManager.GetClientTypeAsync(application)),
                    ApplicationType = _applicationManager.GetApplicationType(await _applicationManager.GetApplicationTypeAsync(application)),
                    ConsentType = _applicationManager.GetConsentType(await _applicationManager.GetConsentTypeAsync(application)),
                    DisplayNames = (await _applicationManager.GetDisplayNamesAsync(application))
                    .Select(x => new Domain.Dto.DisplayName { Culture = x.Key.Name, Name = x.Value}),
                    RedirectUris = await _applicationManager.GetRedirectUrisAsync(application),
                    PostLogoutRedirectUris = await _applicationManager.GetPostLogoutRedirectUrisAsync(application),
                    
                    Scopes = permissions
                    .Where(x => x.StartsWith(OpenIddictConstants.Permissions.Prefixes.Scope))
                    .Select(_applicationManager.GetScopes),
                    
                    GrantTypes = permissions
                    .Where(x => x.StartsWith(OpenIddictConstants.Permissions.Prefixes.GrantType))
                    .Select(_applicationManager.GetGrantType),
                    
                    Endpoints = permissions
                    .Where(x => x.StartsWith(OpenIddictConstants.Permissions.Prefixes.Endpoint))
                    .Select(_applicationManager.GetEndpoint)
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
