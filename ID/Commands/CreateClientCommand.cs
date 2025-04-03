using ID.Domain.Enums;
using MediatR;

namespace ID.Commands
{
    public class CreateClientCommand : IRequest<CreateClientCommandResponse>
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; } = Guid.NewGuid().ToString();
        public ClientType ClientType { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public ConsentType ConsentType { get; set; }
        public IEnumerable<DisplayNames> DisplayNames { get; set; }
        public IEnumerable<string> RedirectUris { get; set; } = [];
        public IEnumerable<string>? PostLogoutRedirectUris { get; set; } = [];
        public IEnumerable<Scope>? Scopes { get; set; }
        public IEnumerable<GrantType>? GrantTypes { get; set; }
        //public IEnumerable<Domain.Enums.Endpoint>? Endpoints { get; set; }
        //public ResponseType ResponseType { get; set; }
        public IEnumerable<Requerment> Requerments { get; set; } = [];
    }

    public class DisplayNames
    {
        private string _culture;
        public string Culture
        {
            get => _culture;
            set => _culture = value.ToLower();
        }
        public string Name { get; set; }
    }

    public class Settings
    {
        public string Logo { get; set; }
    }

    public class CreateClientCommandResponse
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public List<string> RedirectUris { get; set; }
        public string PostLogoutRedirectUri { get; set; }
    }
}
