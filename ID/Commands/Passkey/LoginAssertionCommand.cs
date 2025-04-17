using Fido2NetLib;
using Fido2NetLib.Objects;
using MediatR;
using System.Text.Json.Serialization;

namespace ID.Commands.Passkey
{
    public class LoginAssertionCommand : IRequest
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("rawId")]
        public string RawId { get; set; }

        [JsonPropertyName("response")]
        public AssertionResponse Response { get; set; }

        [JsonPropertyName("type")]
        public PublicKeyCredentialType? Type { get; set; }

        [JsonPropertyName("extensions")]
        public AuthenticationExtensionsClientOutputs? Extensions { get; set; }

        public sealed class AssertionResponse
        {
            [JsonPropertyName("authenticatorData")]
            public string AuthenticatorData { get; set; }

            [JsonPropertyName("signature")]
            public string Signature { get; set; }

            [JsonPropertyName("clientDataJSON")]
            public string ClientDataJson { get; set; }

            [JsonPropertyName("userHandle")]
            public string? UserHandle { get; set; }
        }
    }
}
