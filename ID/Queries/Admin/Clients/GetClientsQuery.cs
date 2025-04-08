using ID.Domain.Dto.Admin;
using ID.Domain.Enums;
using MediatR;

namespace ID.Queries.Admin.Clients
{
    public class GetClientsQuery : IRequest<ClientListDto>
    {
        public string? ClientId { get; set; }
        public ApplicationType? ApplicationType { get; set; }
        public ClientType? ClientType { get; set; }
    }
}
