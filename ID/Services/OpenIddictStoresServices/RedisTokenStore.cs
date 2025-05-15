using System.Text.Json;
using OpenIddict.Abstractions;
using System.Collections.Immutable;
using OpenIddict.EntityFrameworkCore.Models;
using Microsoft.Extensions.Caching.Distributed;

public class RedisTokenStore : IOpenIddictTokenStore<object>
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisTokenStore> _logger;
    private string GetRedisKey(int id) => $"oidc:token:{id}";
    private string GetRedisKey() => $"oidc:token";

    public IOpenIddictTokenStore<TToken> Get<TToken>() where TToken : class
    {
        throw new NotImplementedException();
    }

    public ValueTask<long> CountAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<long> CountAsync<TResult>(Func<IQueryable<object>, IQueryable<TResult>> query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask CreateAsync(object token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask DeleteAsync(object token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<object> FindAsync(string? subject, string? client, string? status, string? type, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<object> FindByApplicationIdAsync(string identifier, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<object> FindByAuthorizationIdAsync(string identifier, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<object?> FindByIdAsync(string identifier, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<object?> FindByReferenceIdAsync(string identifier, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<object> FindBySubjectAsync(string subject, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<string?> GetApplicationIdAsync(object token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<TResult?> GetAsync<TState, TResult>(Func<IQueryable<object>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<string?> GetAuthorizationIdAsync(object token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<DateTimeOffset?> GetCreationDateAsync(object token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<DateTimeOffset?> GetExpirationDateAsync(object token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<string?> GetIdAsync(object token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<string?> GetPayloadAsync(object token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(object token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<DateTimeOffset?> GetRedemptionDateAsync(object token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<string?> GetReferenceIdAsync(object token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<string?> GetStatusAsync(object token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<string?> GetSubjectAsync(object token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<string?> GetTypeAsync(object token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<object> InstantiateAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<object> ListAsync(int? count, int? offset, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<TResult> ListAsync<TState, TResult>(Func<IQueryable<object>, TState, IQueryable<TResult>> query, TState state, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<long> PruneAsync(DateTimeOffset threshold, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<long> RevokeAsync(string? subject, string? client, string? status, string? type, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<long> RevokeByApplicationIdAsync(string identifier, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<long> RevokeByAuthorizationIdAsync(string identifier, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<long> RevokeBySubjectAsync(string subject, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask SetApplicationIdAsync(object token, string? identifier, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask SetAuthorizationIdAsync(object token, string? identifier, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask SetCreationDateAsync(object token, DateTimeOffset? date, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask SetExpirationDateAsync(object token, DateTimeOffset? date, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask SetPayloadAsync(object token, string? payload, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask SetPropertiesAsync(object token, ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask SetRedemptionDateAsync(object token, DateTimeOffset? date, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask SetReferenceIdAsync(object token, string? identifier, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask SetStatusAsync(object token, string? status, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask SetSubjectAsync(object token, string? subject, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask SetTypeAsync(object token, string? type, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask UpdateAsync(object token, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public RedisTokenStore(IDistributedCache cache, ILogger<RedisTokenStore> logger)
    {
        _cache = cache;
        _logger = logger;
    }


    
}
