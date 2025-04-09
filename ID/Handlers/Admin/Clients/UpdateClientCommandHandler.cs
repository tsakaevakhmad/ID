using ID.Commands.Admin.Clients;
using ID.Extensions;
using MediatR;
using OpenIddict.Abstractions;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using static ID.Extensions.IOpenIddictApplicationManagerExtension;

namespace ID.Handlers.Admin.Clients
{
    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand>
    {
        private readonly IOpenIddictApplicationManager _applicationManager;

        public UpdateClientCommandHandler(IOpenIddictApplicationManager applicationManager) 
        {
            _applicationManager = applicationManager;
        }

        public async Task Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var application = await _applicationManager.FindByIdAsync(request.Id);
                await Modify(application, request);
                await _applicationManager.UpdateAsync(application);
            }
            catch
            {
                throw;
            }
        }

        private async Task Modify(dynamic? application, UpdateClientCommand request)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };

            if (application == null) 
                throw new ArgumentNullException(nameof(application));
            
            application.ClientId = request.ClientId;
            application.ClientType = _applicationManager.GetClientType(request.ClientType);
            application.ApplicationType = _applicationManager.GetApplicationType(request.ApplicationType);

            var displayNames = new Dictionary<string, string>();
            foreach (var displayName in request.DisplayNames)
                displayNames.Add(displayName.Culture.ToLower(), displayName.Name);
            application.DisplayNames = JsonSerializer.Serialize(displayNames, options);

            var redirectUris = new HashSet<string>();
            foreach (var redirectUri in request.RedirectUris)
                redirectUris.Add(redirectUri);
            application.RedirectUris = JsonSerializer.Serialize(redirectUris, options);

            var postLogoutRedirectUris = new HashSet<string>();
            foreach (var postLogoutRedirectUri in request.PostLogoutRedirectUris)
                postLogoutRedirectUris.Add(postLogoutRedirectUri);
            application.PostLogoutRedirectUris = JsonSerializer.Serialize(postLogoutRedirectUris);

            application.Permissions = JsonSerializer.Serialize(
                _applicationManager.GetPermissions(request.Scopes, request.GrantTypes, request.Endpoints, request.ResponseType), options);

            var requirements = new HashSet<string>();
            foreach (var requirement in request.Requerments)
                requirements.Add(_applicationManager.GetRequirementsType(requirement));
            application.Requirements = JsonSerializer.Serialize(requirements, options);
        }
    }
}
