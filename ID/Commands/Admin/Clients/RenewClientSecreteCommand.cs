using MediatR;

namespace ID.Commands.Admin.Clients
{
    public class RenewClientSecreteCommand : IRequest
    {
        public string ClientId { get; set; }
    }
}
