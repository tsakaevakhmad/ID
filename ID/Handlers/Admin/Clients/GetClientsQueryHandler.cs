using ID.Data;
using ID.Domain.Dto.Admin;
using ID.Extensions;
using ID.Queries.Admin.Clients;
using MediatR;
using OpenIddict.Abstractions;

namespace ID.Handlers.Admin.Clients
{
    public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, IEnumerable<ClientListDto>>
    {
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly PgDbContext _context;

        public GetClientsQueryHandler(IOpenIddictApplicationManager applicationManager, PgDbContext context) 
        {
            _applicationManager = applicationManager;
            _context = context;
        }

        public async Task<IEnumerable<ClientListDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                IAsyncEnumerable<object> applications = _applicationManager.ListAsync();
                return applications.ToBlockingEnumerable<dynamic>().Select(x => new ClientListDto
                {
                    ClientId = x.ClientId,
                    ClientType = _applicationManager.GetClientType((string)x.ClientType),
                    ApplicationType = _applicationManager.GetApplicationType((string)x.ApplicationType),

                });
            }
            catch
            {
                throw;
            }
        }
    }
}
