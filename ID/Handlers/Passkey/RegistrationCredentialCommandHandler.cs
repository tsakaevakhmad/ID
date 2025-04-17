using AutoMapper;
using Fido2NetLib;
using Fido2NetLib.Objects;
using ID.Commands.PassKey;
using ID.Data;
using ID.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace ID.Handlers.Passkey
{
    public class RegistrationCredentialCommandHandler : IRequestHandler<RegistrationCredentialCommand>
    {
        private readonly HttpContext? _httpContext;
        private readonly IFido2 _fido2;
        private readonly PgDbContext _pgDbContext;
        private readonly IMapper _mapper;

        public RegistrationCredentialCommandHandler(IFido2 fido2, PgDbContext pgDbContext, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _fido2 = fido2;
            _pgDbContext = pgDbContext;
            _mapper = mapper;
        }

        public async Task Handle(RegistrationCredentialCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var options = Base64UrlEncoder.Decode(_httpContext.Session.GetString("fido2.options"));
                var desirializedOptions = JsonSerializer.Deserialize<CredentialCreateOptions>(options, jsonOptions);
                var isCredentialIdUniqueToUser = new IsCredentialIdUniqueToUserAsyncDelegate(IsCredentialIdUniqueToUserAsync);
                var credentialCreationResult = await _fido2.MakeNewCredentialAsync(_mapper.Map<AuthenticatorAttestationRawResponse>(request), desirializedOptions, isCredentialIdUniqueToUser, null, cancellationToken);
                var mapped = _mapper.Map<FidoCredential>(credentialCreationResult.Result);
                await _pgDbContext.FidoCredentials.AddAsync(mapped);
                await _pgDbContext.SaveChangesAsync();
                _httpContext.Session.Remove("fido2.options");
            }
            catch
            {
                throw;
            }
        }

        private async Task<bool> IsCredentialIdUniqueToUserAsync(IsCredentialIdUniqueToUserParams credentialIdUserParams, CancellationToken cancellationToken)
        {
            var base64CredentialId = Base64Url.Encode(credentialIdUserParams.CredentialId);
            var userId = Base64Url.Encode(credentialIdUserParams.User.Id);
            var result = await _pgDbContext.FidoCredentials.AnyAsync(x => x.CredentialId == base64CredentialId, cancellationToken);
            return !result;
        }
    }
}
