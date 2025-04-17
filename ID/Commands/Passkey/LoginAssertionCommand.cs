using Fido2NetLib;
using MediatR;

namespace ID.Commands.Passkey
{
    public class LoginAssertionCommand : IRequest
    {
        public LoginAssertionCommand(AuthenticatorAssertionRawResponse response)
        {
            Response = response;
        }

        public AuthenticatorAssertionRawResponse Response { get; set; }
    }
}
