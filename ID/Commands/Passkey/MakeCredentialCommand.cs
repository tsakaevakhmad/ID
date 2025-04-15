using Fido2NetLib;
using MediatR;

namespace ID.Commands.PassKey
{
    public class MakeCredentialCommand : IRequest
    {
        public MakeCredentialCommand(AuthenticatorAttestationRawResponse rawResponse)
        {
            AttestationResponse = rawResponse;
        }

        public AuthenticatorAttestationRawResponse AttestationResponse { get; set; }
    }
}
