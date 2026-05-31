using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;
using VirusTotalCore.Urls.Endpoints;

namespace VirusTotalCore.Tests.Integration.Urls;

public sealed class ScanTests : WireMockEndpointTestBase
{
    private const string Url = "https://example.com";

    private readonly UrlEndpoint _endpoint;

    public ScanTests()
    {
        _endpoint = new UrlEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task Scan_HappyPath_ReturnsNonEmptyId()
    {
        Server.StubScanSuccess();

        var id = await _endpoint.Scan(Url);

        Assert.False(string.IsNullOrEmpty(id));
    }

    [Fact]
    public async Task Scan_SentAsMultipartFormData_WithUrlFieldName()
    {
        Server.StubScanSuccess();

        await _endpoint.Scan(Url);

        var entry = Server.LogEntries.Last();
        var contentType = entry.RequestMessage!.Headers!["Content-Type"].First();
        Assert.Contains("multipart/form-data", contentType);
        Assert.Contains("name=url", entry.RequestMessage!.Body!);
    }

    [Fact]
    public async Task Scan_WhenForbidden_ThrowsForbiddenException()
    {
        Server.StubForbidden("/urls", "POST");

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            _endpoint.Scan(Url));
    }
}
