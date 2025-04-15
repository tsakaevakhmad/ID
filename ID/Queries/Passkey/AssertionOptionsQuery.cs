using Fido2NetLib;
using MediatR;

namespace ID.Queries.Passkey
{
    public class AssertionOptionsQuery : IRequest<AssertionOptions>
    {
        public string UserId { get; set; }
    }
}
