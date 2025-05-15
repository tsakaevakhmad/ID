using AutoMapper;
using Fido2NetLib;
using Fido2NetLib.Objects;
using ID.Commands.Passkey;
using ID.Data;
using ID.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
        private readonly SignInManager<User> _signInManager;

        public LoginAssertionCommandHandler(PgDbContext context, IMapper mapper, IFido2 fido2, IHttpContextAccessor httpContext, SignInManager<User> signInManager) 
        {
            _context = context;
            _mapper = mapper;
            _fido2 = fido2;
            _httpContext = httpContext.HttpContext;
            _signInManager = signInManager;
        }

        public async Task Handle(LoginAssertionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var options = Base64UrlEncoder.Decode(_httpContext.Session.GetString("fido2.options"));
                var desirializedOptions = JsonSerializer.Deserialize<AssertionOptions>(options, jsonOptions);
                var credId = request.Id;
                var storedCredential = await _context.FidoCredentials.Include(x => x.User).FirstOrDefaultAsync(x => x.CredentialId == credId);
                var assertResponse = _mapper.Map<AuthenticatorAssertionRawResponse>(request);
                
                if(string.IsNullOrEmpty(request.Response.UserHandle))
                    assertResponse.Response.UserHandle = null;

                var res = await _fido2.MakeAssertionAsync(assertResponse, desirializedOptions, storedCredential.PublicKey, storedCredential.SignatureCounter, IsUserHandleOwnerOfCredentialId);
                if(res.Status == "ok")
                {
                    storedCredential.SignatureCounter = res.Counter;
                    await _context.SaveChangesAsync(cancellationToken);
                    _httpContext.Session.Remove("fido2.options");
                    await _signInManager.SignInAsync(storedCredential.User, false);
                }
            }
            catch
            {
                throw;
            }
        }

        private async Task<bool> IsUserHandleOwnerOfCredentialId(IsUserHandleOwnerOfCredentialIdParams args, CancellationToken cancellationToken = default)
        {
            var base64CredentialId = Base64Url.Encode(args.CredentialId);
            var base64UserHandle = Encoding.UTF8.GetString(args.UserHandle); 
            var result = await _context.FidoCredentials.AnyAsync(x => x.CredentialId == base64CredentialId && x.UserId == base64UserHandle, cancellationToken);
            return result;
        }
    }
}
