using ID.Domain.Entity;
using ID.Domain.Enums;
using ID.Extensions;
using ID.Interfaces;
using ID.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ID.Handlers
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginQueryResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMailService _mailService;

        public LoginQueryHandler(UserManager<User> userManager, IMailService mailService)
        {
            _userManager = userManager;
            _mailService = mailService;
        }

        public async Task<LoginQueryResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await FindUserByIdentifire(request.Identifier);
                var confirmation = CheckConfirmation(request.Identifier, user);
                if (confirmation.HasValue)
                    return new LoginQueryResponse() { Id = user?.Id, Status = confirmation.Value };

                var code = await _userManager.GenerateLoginTokenAsync(user);
                
                if(IdentifireIs(request.Identifier) == "mail")
                {
                    await _mailService.SentAsync(user.Email, "login code", code);
                    return new LoginQueryResponse() { Id = user?.Id, Status = AuthResponseStatus.SendedLoginCodeToEmail };
                }
                await _mailService.SentAsync(user.Email, "login code", code);
                return new LoginQueryResponse() { Id = user?.Id, Status = AuthResponseStatus.SendedLoginCodeToPhoneNumber };
            }
            catch
            {
                throw;
            }
        }

        private async Task<User> FindUserByIdentifire(string identifire)
        {
            identifire = identifire.ToUpper();
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == identifire || x.PhoneNumber == identifire);
            return user;
        }

        private string IdentifireIs(string identifire)
        {
            return identifire switch
            {
                var x when x.Contains("@") => "mail",
                var x when x.StartsWith("+") => "phonenumber",
                _ => "unknown"
            };
        }

        private AuthResponseStatus? CheckConfirmation(string identifire, User user)
        {
            if (user == null)
                return AuthResponseStatus.UserNotFound;

            var identifireType = IdentifireIs(identifire);
            if (identifireType == "mail")
                if (!user.EmailConfirmed)
                    return AuthResponseStatus.UserMailNotConfirmed;
            
            if (identifireType == "phonenumber")
                if (!user.PhoneNumberConfirmed)
                    return AuthResponseStatus.UserPhoneNotConfirmed;
            return null;
        }
    }
}
