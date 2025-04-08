using ID.Domain.Dto;
using ID.Domain.Enums;
using MediatR;

namespace ID.Commands.Admin.Clients
{
    public class CreateClientCommand : IRequest<CreateClientCommandResponse>
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; } = Guid.NewGuid().ToString();
        public ClientType ClientType { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public ConsentType ConsentType { get; set; }
        public IEnumerable<DisplayName> DisplayNames { get; set; }
        public IEnumerable<string> RedirectUris { get; set; } = [];
        public IEnumerable<string>? PostLogoutRedirectUris { get; set; } = [];
        public IEnumerable<Scope>? Scopes { get; set; }
        public IEnumerable<GrantType>? GrantTypes { get; set; }
        public IEnumerable<Domain.Enums.Endpoint>? Endpoints { get; set; }
        public ResponseType? ResponseType { get; set; }
        public IEnumerable<Requerment> Requerments { get; set; } = [];
    }

    public class CreateClientCommandResponse
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public List<string> RedirectUris { get; set; }
        public string PostLogoutRedirectUri { get; set; }
    }
}
