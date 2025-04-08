using ID.Domain.Dto.Admin;
using MediatR;

namespace ID.Queries.Admin.Clients
{
    public class GetClientDetailsQuery : IRequest<ClientDto>
    {
        public GetClientDetailsQuery(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
