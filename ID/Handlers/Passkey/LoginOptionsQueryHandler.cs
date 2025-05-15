using Fido2NetLib;
using Fido2NetLib.Objects;
using ID.Data;
using ID.Domain.Entity;
using ID.Queries.Passkey;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace ID.Handlers.Passkey
{
    public class LoginOptionsQueryHandler : IRequestHandler<LoginOptionsQuery, AssertionOptions>
    {
        private readonly HttpContext? _httpContext;
        private readonly IFido2 _fido2;
        private readonly PgDbContext _context;
        private readonly UserManager<User> _userManager;

        public LoginOptionsQueryHandler(IFido2 fido2, PgDbContext context, IHttpContextAccessor httpContext, UserManager<User> userManager) 
        {
            _httpContext = httpContext.HttpContext;
            _fido2 = fido2;
            _context = context;
            _userManager = userManager;
        }

        public async Task<AssertionOptions> Handle(LoginOptionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var options = _fido2.GetAssertionOptions(null, UserVerificationRequirement.Preferred);
                var serialized = JsonSerializer.Serialize(options);
                _httpContext.Session.SetString("fido2.options", Base64UrlEncoder.Encode(serialized));
                return options;
            }
            catch
            {
                throw;
            }
        }
    }
}
