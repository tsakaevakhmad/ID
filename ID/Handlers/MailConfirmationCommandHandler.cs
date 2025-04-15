using ID.Commands;
using ID.Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Web;

namespace ID.Handlers
{
    public class MailConfirmationCommandHandler : IRequestHandler<MailConfirmationCommand, bool>
    {
        private readonly UserManager<User> _userManage;

        public MailConfirmationCommandHandler(UserManager<User> userManager)
        {
            _userManage = userManager;
        }

        public async Task<bool> Handle(MailConfirmationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                request.Token = Base64UrlEncoder.Decode(request.Token);
                var result = await _userManage.ConfirmEmailAsync(await _userManage.FindByIdAsync(request.Id), request.Token);
                if (result.Succeeded)
                    return true;
                return false;
            }
            catch
            {
                throw;
            }
        }
    }
}
