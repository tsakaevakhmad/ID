using MediatR;

namespace ID.Commands.Admin
{
    public class MailConfirmationCommand : IRequest<bool>
    {
        public MailConfirmationCommand(string id, string token)
        {
            Id = id;
            Token = token;
        }
        public string Id { get; set; }
        public string Token { get; set; }
    }
}
