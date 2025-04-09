using ID.Commands.Admin.Clients;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using System.Security.Cryptography;

namespace ID.Handlers.Admin.Clients
{
    public class RenewClientSecreteCommandHandler : IRequestHandler<RenewClientSecretCommand, string>
    {
        private readonly IOpenIddictApplicationManager _applicationManager;

        public RenewClientSecreteCommandHandler(IOpenIddictApplicationManager applicationManager) 
        {
            _applicationManager = applicationManager;
        }

        public async Task<string> Handle(RenewClientSecretCommand request, CancellationToken cancellationToken)
        {
            try
            {
                dynamic application = await _applicationManager.FindByIdAsync(request.Id);
                if (application == null)
                    throw new InvalidOperationException($"Client with {request.Id} is not exist");
                
                application.ClientSecret = new PasswordHasher<string>().HashPassword(null, request.Secrete);
                await _applicationManager.UpdateAsync(application);
                return request.Secrete;
            }
            catch
            {
                throw;
            }
        }
    }
}
