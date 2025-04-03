using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ID.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Scope
    {

        Email,
        Profile,
        Phone,
        Address,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ClientType
    {
        Public,
        Confidential,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ApplicationType
    {
        Web,
        Native
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GrantType
    {
        AuthorizationCode,
        Implicit,
        ClientCredentials,
        ResourceOwnerPassword,
        RefreshToken,
        DeviceCode,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ConsentType
    {
        Implicit,
        Explicit,
        External,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Endpoint
    {
        Authorization,
        Token,
        Introspection,
        Revocation,
        EndSession,
        DeviceAuthorization,
        PushedAuthorization,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ResponseType
    {
        CodeIdToken,
        CodeIdTokenToken,
        IdTokenToken,
        IdToken,
        None,
        Token
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Requerment
    {
        PushedAuthorizationRequests,
        ProofKeyForCodeExchange
    }
}
