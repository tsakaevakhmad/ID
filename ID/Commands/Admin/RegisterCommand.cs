using ID.Domain.Enums;
using MediatR;

namespace ID.Commands.Admin
{
    public class RegisterCommand : IRequest<RegisterResponse>
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class RegisterResponse
    {
        public RegisterResponse(AuthResponseStatus status)
        {
            Status = status;
        }

        public AuthResponseStatus Status { get; set; }
    }
}