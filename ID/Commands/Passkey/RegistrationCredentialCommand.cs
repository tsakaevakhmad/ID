using Fido2NetLib;
using Fido2NetLib.Objects;
using MediatR;
using System.Text.Json.Serialization;

namespace ID.Commands.PassKey
{
    public class RegistrationCredentialCommand : IRequest
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("rawId")]
        public string RawId { get; set; }

        [JsonPropertyName("type")]
        public PublicKeyCredentialType? Type { get; set; }

        [JsonPropertyName("response")]
        public ResponseData Response { get; set; }

        [JsonPropertyName("extensions")]
        public AuthenticationExtensionsClientOutputs? Extensions { get; set; }

        public sealed class ResponseData
        {
            [JsonPropertyName("attestationObject")]
            public string AttestationObject { get; set; }

            [JsonPropertyName("clientDataJSON")]
            public string ClientDataJson { get; set; }
        }
    }
}
