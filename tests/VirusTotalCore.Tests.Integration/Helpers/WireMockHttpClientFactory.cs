namespace VirusTotalCore.Tests.Integration.Helpers;

internal sealed class WireMockHttpClientFactory(string wireMockBaseUrl) : IHttpClientFactory
{
    public HttpClient CreateClient(string name) =>
        new() { BaseAddress = new Uri(wireMockBaseUrl.TrimEnd('/') + "/api/v3/") };
}
