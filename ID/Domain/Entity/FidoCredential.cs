using System.ComponentModel.DataAnnotations;

namespace ID.Domain.Entity
{
    public class FidoCredential
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Username { get; set; } = default!;
        public string CredentialId { get; set; } = default!; // base64url
        public byte[] PublicKey { get; set; } = default!;
        public uint SignatureCounter { get; set; }

        public string CredType { get; set; } = default!;
        public string? RegDate { get; set; }

        public string? AaGuid { get; set; }
        public string? AuthenticatorDescription { get; set; }

        public string? Transports { get; set; } // comma-separated list (usb,ble,nfc,internal)
        public string UserId { get; set; }
        public User User { get; set; }
    }

}
