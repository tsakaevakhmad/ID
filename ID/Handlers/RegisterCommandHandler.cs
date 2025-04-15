using AutoMapper;
using ID.Commands;
using ID.Domain.Entity;
using ID.Domain.Enums;
using ID.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Web;

namespace ID.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly HttpContext? _accessor;
        private readonly IMailService _mailService;
        private readonly IMediator _mediator;

        public RegisterCommandHandler(
            IMailService mailService, 
            IMediator mediator, 
            UserManager<User> userManager, 
            IMapper mapper, 
            IHttpContextAccessor accessor) 
        {
            _mailService = mailService;
            _mediator = mediator;
            _userManager = userManager;
            _mapper = mapper;
            _accessor = accessor.HttpContext;
        }

        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    user = _mapper.Map<User>(request);
                    var result = await _userManager.CreateAsync(user);
                }

                if (user.EmailConfirmed)
                    return new RegisterResponse(AuthResponseStatus.UserAlreadyExists);

                await SendTokenAsync(user);
                return new RegisterResponse(AuthResponseStatus.SendedMailConfirmationCode);
            }
            catch
            {
                throw;
            }
        }

        private async Task SendTokenAsync(User user) 
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"{_accessor.Request.Scheme + "://" + _accessor.Request.Host}/auth/confirmmail?token={Base64UrlEncoder.Encode(token)}&id={user.Id}";
            await _mailService.SentAsync(user.Email, "Mail confirmation", confirmationLink);
        }
    }
}
