using ID.Domain.Entity;
using ID.Domain.Enums;
using ID.Extensions;
using ID.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ID.Handlers
{
    public class LoginVerifyQueryHandler : IRequestHandler<LoginVerifyQuery, LoginVerifyQueryResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public LoginVerifyQueryHandler(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<LoginVerifyQueryResponse> Handle(LoginVerifyQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.Id);
                if (user == null)
                    return new LoginVerifyQueryResponse(AuthResponseStatus.UserNotFound);

                var result = await _userManager.VerifyLoginTokenAsync(user, request.Code);
                if (result)
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return new LoginVerifyQueryResponse(AuthResponseStatus.Success);
                }

                return new LoginVerifyQueryResponse(AuthResponseStatus.InvalidToken);
            }
            catch
            {
                throw;
            }
        }
    }
}
