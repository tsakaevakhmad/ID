﻿using AutoMapper;
using Fido2NetLib;
using Fido2NetLib.Objects;
using ID.Commands.Passkey;
using ID.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using static Fido2NetLib.AuthenticatorAssertionRawResponse;

namespace ID.Handlers.Passkey
{
    public class LoginAssertionCommandHandler : IRequestHandler<LoginAssertionCommand>
    {
        private readonly PgDbContext _context;
        private readonly IMapper _mapper;
        private readonly IFido2 _fido2;
        private readonly HttpContext? _httpContext;

        public LoginAssertionCommandHandler(PgDbContext context, IMapper mapper, IFido2 fido2, IHttpContextAccessor httpContext) 
        {
            _context = context;
            _mapper = mapper;
            _fido2 = fido2;
            _httpContext = httpContext.HttpContext;
        }

        public async Task Handle(LoginAssertionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var options = Base64UrlEncoder.Decode(_httpContext.Session.GetString("fido2.options"));
                var desirializedOptions = JsonSerializer.Deserialize<AssertionOptions>(options, jsonOptions);

                var storedCredential = await _context.FidoCredentials.FirstOrDefaultAsync(x => x.CredentialId == Base64Url.Encode(request.Response.Id));

                var res = await _fido2.MakeAssertionAsync(request.Response, desirializedOptions, storedCredential.PublicKey, storedCredential.SignatureCounter, IsUserHandleOwnerOfCredentialId);

                var cred = await _context.FidoCredentials.FirstAsync(x => x.CredentialId == storedCredential.Id);
                cred.SignatureCounter = res.Counter;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                throw;
            }
        }

        private async Task<bool> IsUserHandleOwnerOfCredentialId(IsUserHandleOwnerOfCredentialIdParams args, CancellationToken cancellationToken = default)
        {
            var base64CredentialId = Base64Url.Encode(args.CredentialId);
            var base64UserHandle = Base64Url.Encode(args.UserHandle);
            var result = await _context.FidoCredentials.AnyAsync(x => x.CredentialId == base64CredentialId && x.UserId == base64UserHandle, cancellationToken);
            return result;
        }
    }
}
