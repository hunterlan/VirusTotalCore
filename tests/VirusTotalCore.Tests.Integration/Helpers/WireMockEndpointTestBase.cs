namespace VirusTotalCore.Tests.Integration.Helpers;

/// <summary>
/// Template Method base class that manages the WireMockServer lifecycle for each test class.
/// </summary>
public abstract class WireMockEndpointTestBase : IDisposable
{
    protected WireMockServer Server { get; }

    private readonly HttpClient _httpClient;

    protected WireMockEndpointTestBase()
    {
        Server = WireMockServer.Start();
        _httpClient = new HttpClient { BaseAddress = new Uri(Server.Url! + "/") };
    }

    /// <summary>
    /// Returns the shared <see cref="HttpClient"/> whose <c>BaseAddress</c> points at the running WireMock server.
    /// </summary>
    protected HttpClient CreateHttpClient() => _httpClient;

    public void Dispose()
    {
        Server.Stop();
        Server.Dispose();
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}
