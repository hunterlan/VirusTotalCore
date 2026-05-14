using VirusTotalCore.Common.Transport;

namespace VirusTotalCore.Common;

public abstract class BaseEndpoint
{
    private readonly IVirusTotalTransport _transport;
    protected readonly string CurrentEndpointName;

    protected BaseEndpoint(string apiKey, string endpoint)
        : this(new VirusTotalEndpointTransport(VirusTotalHttpClientProvider.Create(apiKey)), endpoint)
    {
    }

    protected BaseEndpoint(IHttpClientFactory httpClientFactory, string apiKey, string endpoint)
        : this(new VirusTotalEndpointTransport(VirusTotalHttpClientProvider.Create(httpClientFactory, apiKey)), endpoint)
    {
    }

    internal BaseEndpoint(IVirusTotalTransport transport, string endpoint)
    {
        _transport = transport ?? throw new ArgumentNullException(nameof(transport));
        CurrentEndpointName = endpoint;
    }

    protected Task<T> GetAsync<T>(string requestUrl, CancellationToken cancellationToken) =>
        _transport.GetAsync<T>(CurrentEndpointName, requestUrl, cancellationToken);

    protected Task<T> GetAsync<T>(string requestUrl, string jsonRootName, CancellationToken cancellationToken) =>
        _transport.GetAsync<T>(CurrentEndpointName, requestUrl, jsonRootName, cancellationToken);

    protected Task PostAsync(string requestUrl, object value, CancellationToken cancellationToken) =>
        _transport.PostAsync(CurrentEndpointName, requestUrl, value, cancellationToken);

    protected Task<T> PostAsync<T>(string requestUrl, string jsonRootName, object value, CancellationToken cancellationToken) =>
        _transport.PostAsync<T>(CurrentEndpointName, requestUrl, jsonRootName, value, cancellationToken);

    protected Task DeleteAsync(string requestUrl, CancellationToken cancellationToken) =>
        _transport.DeleteAsync(CurrentEndpointName, requestUrl, cancellationToken);

    protected Task<string> PostMultipartAsync(string? requestUrl, MultipartFormDataContent content, CancellationToken cancellationToken) =>
        _transport.PostMultipartAsync(CurrentEndpointName, requestUrl, content, cancellationToken);

    /// <summary>
    /// Get objects related to the given object.
    /// </summary>
    /// <param name="classObjectApiValue">Value of specific endpoint. For example, for IP it's IP value.</param>
    /// <param name="relationship">Relationship name. See VirusTotal relationship table for specific endpoint.</param>
    /// <param name="cursor">Continuation cursor</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="limit">Maximum number of related objects to retrieve</param>
    /// <returns>JSON string</returns>
    public virtual Task<string> GetRelatedObjects(string classObjectApiValue, string relationship, string? cursor,
        CancellationToken? cancellationToken, int limit = 10)
    {
        var requestUrl = VirusTotalRelationshipRequestBuilder.BuildObjects(classObjectApiValue, relationship, cursor, limit);
        return _transport.GetRawAsync(CurrentEndpointName, requestUrl, cancellationToken ?? CancellationToken.None);
    }

    /// <summary>
    /// Get related object IDs.
    /// </summary>
    /// <param name="classObjectApiValue">Value of specific endpoint. For example, for IP it's IP value.</param>
    /// <param name="relationship">Relationship name. See VirusTotal relationship table for specific endpoint.</param>
    /// <param name="cursor">Continuation cursor</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <param name="limit">Maximum number of related objects to retrieve</param>
    /// <returns>JSON string</returns>
    public Task<string> GetRelatedDescriptors(string classObjectApiValue, string relationship, string? cursor,
        CancellationToken? cancellationToken, int limit = 10)
    {
        var requestUrl = VirusTotalRelationshipRequestBuilder.BuildDescriptors(classObjectApiValue, relationship, cursor, limit);
        return _transport.GetRawAsync(CurrentEndpointName, requestUrl, cancellationToken ?? CancellationToken.None);
    }
}