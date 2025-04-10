using ID.Domain.Enums;
using MediatR;

namespace ID.Queries
{
    public class LoginVerifyQuery : IRequest<LoginVerifyQueryResponse>
    {
        public LoginVerifyQuery(string id, string code)
        {
            Id = id;
            Code = code;
        }

        public string Id { get; }
        public string Code { get; }
    }

    public class LoginVerifyQueryResponse
    {
        public LoginVerifyQueryResponse(AuthResponseStatus status)
        {
            Status = status;
        }

        public AuthResponseStatus Status { get; set; }
    }
}
