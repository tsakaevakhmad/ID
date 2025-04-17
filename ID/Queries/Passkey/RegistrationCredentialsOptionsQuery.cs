using Fido2NetLib;
using Fido2NetLib.Objects;
using MediatR;

namespace ID.Queries.Passkey
{
    public class RegistrationCredentialsOptionsQuery : IRequest<CredentialCreateOptions>
    {
    }
}
