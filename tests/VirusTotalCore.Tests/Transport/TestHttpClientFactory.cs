using System.Net;
using System.Text;

namespace VirusTotalCore.Tests.Transport;

internal sealed class FakeHttpMessageHandler(HttpStatusCode statusCode, string responseBody) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(responseBody, Encoding.UTF8, "application/json")
        };

        return Task.FromResult(response);
    }
}

internal static class TestHttpClientFactory
{
    private static readonly Uri BaseAddress = new("https://www.virustotal.com/api/v3/");

    public static HttpClient Create(HttpStatusCode statusCode, string responseBody)
    {
        var handler = new FakeHttpMessageHandler(statusCode, responseBody);
        return new HttpClient(handler) { BaseAddress = BaseAddress };
    }
}
