using ID.Commands.Admin.Clients;
using ID.Domain.Enums;
using ID.Extensions;
using MediatR;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Endpoint = ID.Domain.Enums.Endpoint;

namespace ID.Handlers.Admin.Clients
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
                    ClientType = _manager.GetClientType(request.ClientType),
                    ApplicationType = _manager.GetApplicationType(request.ApplicationType),
                    ConsentType = _manager.GetConsentType(request.ConsentType),
                };

                foreach (var requerment in request.Requerments)
                    application.Requirements.Add(_manager.GetRequirementsType(requerment));

                foreach (var displayName in request?.DisplayNames)
                    application.DisplayNames.Add(new System.Globalization.CultureInfo(displayName.Culture), displayName.Name);

                foreach (var redirectUri in request?.PostLogoutRedirectUris)
                    application.RedirectUris.Add(new Uri(redirectUri));

                foreach (var redirectUri in request?.RedirectUris)
                    application.RedirectUris.Add(new Uri(redirectUri));

                foreach (var permisssion in _manager.GetPermissions(request.Scopes, request.GrantTypes, request.Endpoints, request.ResponseType))
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
    }
}
