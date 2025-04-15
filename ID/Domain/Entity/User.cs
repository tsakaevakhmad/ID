using Microsoft.AspNetCore.Identity;

namespace ID.Domain.Entity
{
    public class User : IdentityUser
    {
        public IEnumerable<FidoCredential> FidoCredentials { get; set; }
    }
}
