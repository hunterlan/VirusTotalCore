namespace VirusTotalCore.Common.Transport;

internal interface IVirusTotalTransport
{
    Task<T> GetAsync<T>(string endpointName, string requestUrl, CancellationToken cancellationToken);
    Task<T> GetAsync<T>(string endpointName, string requestUrl, string jsonRootName, CancellationToken cancellationToken);
    Task<string> GetRawAsync(string endpointName, string requestUrl, CancellationToken cancellationToken);
    Task PostAsync(string endpointName, string requestUrl, object value, CancellationToken cancellationToken);
    Task<T> PostAsync<T>(string endpointName, string requestUrl, string rootPropertyName, object value, CancellationToken cancellationToken);
    Task<string> PostMultipartAsync(string endpointName, string? requestUrl, MultipartFormDataContent content, CancellationToken cancellationToken);
    Task DeleteAsync(string endpointName, string requestUrl, CancellationToken cancellationToken);
}