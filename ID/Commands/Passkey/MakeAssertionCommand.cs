using Fido2NetLib;
using MediatR;

namespace ID.Commands.Passkey
{
    public class MakeAssertionCommand : IRequest
    {
        public MakeAssertionCommand(AuthenticatorAttestationRawResponse response)
        {
            Response = response;
        }

        public AuthenticatorAttestationRawResponse Response { get; set; }
    }
}
