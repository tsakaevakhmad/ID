using MediatR;

namespace ID.Commands.Admin.Clients
{
    public class RenewClientSecretCommand : IRequest<string>
    {
        public string Id { get; set; }
        public string? Secrete { get; set; } = Guid.NewGuid().ToString();
    }
}
