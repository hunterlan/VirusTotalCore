using System.Net.Http.Json;
using System.Text.Json;

namespace VirusTotalCore.Common.Transport;

internal sealed class VirusTotalEndpointTransport(HttpClient httpClient) : IVirusTotalTransport
{
    public async Task<T> GetAsync<T>(string endpointName, string requestUrl, CancellationToken cancellationToken)
    {
        var responseJson = await SendGetAsync(endpointName, requestUrl, cancellationToken);

        using var jsonDocument = JsonDocument.Parse(responseJson);
        return jsonDocument.Deserialize<T>(VirusTotalJsonOptions.Default)!;
    }

    public async Task<T> GetAsync<T>(string endpointName, string requestUrl, string jsonRootName, CancellationToken cancellationToken)
    {
        var responseJson = await SendGetAsync(endpointName, requestUrl, cancellationToken);

        using var jsonDocument = JsonDocument.Parse(responseJson);
        return jsonDocument.RootElement.GetProperty(jsonRootName).Deserialize<T>(VirusTotalJsonOptions.Default)!;
    }

    public async Task<string> GetRawAsync(string endpointName, string requestUrl, CancellationToken cancellationToken)
    {
        var url = VirusTotalRequestPathBuilder.Build(endpointName, requestUrl);

        using var response = await httpClient.GetAsync(url, cancellationToken);
        return await EnsureSuccessAsync(response, cancellationToken);
    }

    public async Task PostAsync(string endpointName, string requestUrl, object value, CancellationToken cancellationToken)
    {
        await SendPostAsync(endpointName, requestUrl, value, cancellationToken);
    }

    public async Task<T> PostAsync<T>(string endpointName, string requestUrl, string rootPropertyName, object value, CancellationToken cancellationToken)
    {
        var responseJson = await SendPostAsync(endpointName, requestUrl, value, cancellationToken);

        using var jsonDocument = JsonDocument.Parse(responseJson);
        return jsonDocument.RootElement.GetProperty(rootPropertyName).Deserialize<T>(VirusTotalJsonOptions.Default)!;
    }

    public async Task<string> PostMultipartAsync(string endpointName, string? requestUrl, MultipartFormDataContent content, CancellationToken cancellationToken)
    {
        var url = string.IsNullOrEmpty(requestUrl)
            ? endpointName
            : Uri.IsWellFormedUriString(requestUrl, UriKind.Absolute)
                ? requestUrl
                : VirusTotalRequestPathBuilder.Build(endpointName, requestUrl);

        using var response = await httpClient.PostAsync(url, content, cancellationToken);
        return await EnsureSuccessAsync(response, cancellationToken);
    }

    public async Task DeleteAsync(string endpointName, string requestUrl, CancellationToken cancellationToken)
    {
        var url = VirusTotalRequestPathBuilder.Build(endpointName, requestUrl);

        using var response = await httpClient.DeleteAsync(url, cancellationToken);
        await EnsureSuccessAsync(response, cancellationToken);
    }

    private async Task<string> SendGetAsync(string endpointName, string requestUrl, CancellationToken cancellationToken)
    {
        var url = VirusTotalRequestPathBuilder.Build(endpointName, requestUrl);

        using var response = await httpClient.GetAsync(url, cancellationToken);
        return await EnsureSuccessAsync(response, cancellationToken);
    }

    private async Task<string> SendPostAsync(string endpointName, string requestUrl, object value, CancellationToken cancellationToken)
    {
        var url = VirusTotalRequestPathBuilder.Build(endpointName, requestUrl);
        var content = JsonContent.Create(value, value.GetType(), options: VirusTotalJsonOptions.Default);

        using var response = await httpClient.PostAsync(url, content, cancellationToken);
        return await EnsureSuccessAsync(response, cancellationToken);
    }

    private static async Task<string> EnsureSuccessAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw VirusTotalErrorHandler.Handle(responseJson);
        }

        return responseJson;
    }
}
