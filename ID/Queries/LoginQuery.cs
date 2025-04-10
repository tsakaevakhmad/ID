using ID.Domain.Enums;
using MediatR;

namespace ID.Queries
{
    public class LoginQuery : IRequest<LoginQueryResponse>
    {
        public string Identifier { get; set; }
    }

    public class LoginQueryResponse
    {
        public string Id { get; set; }
        public AuthResponseStatus Status { get; set; }
    }
}
