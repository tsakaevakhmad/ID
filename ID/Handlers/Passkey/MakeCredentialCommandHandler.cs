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
    public class MakeCredentialCommandHandler : IRequestHandler<MakeCredentialCommand>
    {
        private readonly HttpContext? _httpContext;
        private readonly IFido2 _fido2;
        private readonly PgDbContext _pgDbContext;
        private readonly IMapper _mapper;

        public MakeCredentialCommandHandler(IFido2 fido2, PgDbContext pgDbContext, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _fido2 = fido2;
            _pgDbContext = pgDbContext;
            _mapper = mapper;
        }

        public async Task Handle(MakeCredentialCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var challenge = Base64UrlEncoder.Decode(_httpContext.Session.GetString("fido2.options"));
                var desirializedOptions = JsonSerializer.Deserialize<CredentialCreateOptions>(challenge, jsonOptions);
                var isCredentialIdUniqueToUser = new IsCredentialIdUniqueToUserAsyncDelegate(IsCredentialIdUniqueToUserAsync);
                var options = await _fido2.MakeNewCredentialAsync(request.AttestationResponse, desirializedOptions, isCredentialIdUniqueToUser, null, cancellationToken);
                await _pgDbContext.FidoCredentials.AddAsync(_mapper.Map<FidoCredential>(options));
                await _pgDbContext.SaveChangesAsync();
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
            return await _pgDbContext.FidoCredentials.AnyAsync(x => x.CredentialId == base64CredentialId, cancellationToken);
        }
    }
}
