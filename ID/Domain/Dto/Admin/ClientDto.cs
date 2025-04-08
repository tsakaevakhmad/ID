using ID.Domain.Enums;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Endpoint = ID.Domain.Enums.Endpoint;

namespace ID.Domain.Dto.Admin
{
    public class ClientDto
    {
        public string Id { get; internal set; }
        public string ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public ClientType ClientType { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public ConsentType ConsentType { get; set; }
        public IEnumerable<DisplayName> DisplayNames { get; set; }
        public IEnumerable<string> RedirectUris { get; set; }
        public IEnumerable<string> PostLogoutRedirectUris { get; set; }
        public IEnumerable<Scope>? Scopes { get; set; }
        public IEnumerable<GrantType>? GrantTypes { get; set; }
        public IEnumerable<Endpoint>? Endpoints { get; set; }
    }

    public class ClientListDto
    {
        public string ClientId { get; set; }
        public ClientType ClientType { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public IEnumerable<string> DisplayNames { get; set; }
    }

    public class Settings
    {
        public string Logo { get; set; }
    }
}
