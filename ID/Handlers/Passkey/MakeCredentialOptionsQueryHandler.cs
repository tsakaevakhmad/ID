using Fido2NetLib;
using Fido2NetLib.Objects;
using ID.Queries.Passkey;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ID.Handlers.Passkey
{
    public class MakeCredentialOptionsQueryHandler : IRequestHandler<MakeCredentialsOptionsQuery, CredentialCreateOptions>
    {
        private readonly IFido2 _fido2;
        private readonly HttpContext? _httpContext;

        public MakeCredentialOptionsQueryHandler(IFido2 fido2, IHttpContextAccessor httpContextAccessor) 
        {
            _fido2 = fido2;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public Task<CredentialCreateOptions> Handle(MakeCredentialsOptionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var httpUser = _httpContext.User;
                var user = new Fido2User
                {
                    DisplayName = httpUser.FindFirst(x => x.Type == ClaimTypes.Name).Value,
                    Name = httpUser.FindFirst(x => x.Type == ClaimTypes.Name).Value,
                    Id = Encoding.UTF8.GetBytes(httpUser.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value)
                };

                var options = _fido2.RequestNewCredential(
                    user, 
                    new List<PublicKeyCredentialDescriptor>(),
                    AuthenticatorSelection.Default, 
                    AttestationConveyancePreference.None,
                    null);

                var serialized = JsonSerializer.Serialize(options);
                _httpContext.Session.SetString("fido2.options", Base64UrlEncoder.Encode(serialized));
                return Task.FromResult(options);
            }
            catch
            {
                throw;
            }
        }
    }
}
