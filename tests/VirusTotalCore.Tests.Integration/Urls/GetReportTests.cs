using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;
using VirusTotalCore.Urls.Endpoints;

namespace VirusTotalCore.Tests.Integration.Urls;

public sealed class GetReportTests : WireMockEndpointTestBase
{
    private const string Url = "https://example.com";
    private static readonly string UrlId = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Url));

    private readonly UrlEndpoint _endpoint;

    public GetReportTests()
    {
        _endpoint = new UrlEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task GetReport_HappyPath_ReturnsDeserializedReport()
    {
        Server.StubGetUrlReportSuccess(UrlId);

        var report = await _endpoint.GetReport(Url);

        Assert.NotNull(report);
        Assert.Equal(UrlId, report.Id);
        Assert.Equal("url", report.Type);
        Assert.NotNull(report.Attributes.LastAnalysisStats);
    }

    [Fact]
    public async Task GetReport_WhenForbidden_ThrowsForbiddenException()
    {
        Server.StubForbidden($"/urls/{UrlId}");

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            _endpoint.GetReport(Url));
    }

    [Fact]
    public async Task GetReport_WhenNotFound_ThrowsNotFoundException()
    {
        Server.StubNotFound($"/urls/{UrlId}");

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _endpoint.GetReport(Url));
    }

    [Fact]
    public async Task GetReport_WhenUnauthorized_ThrowsWrongCredentialsException()
    {
        Server.StubUnauthorized($"/urls/{UrlId}");

        await Assert.ThrowsAsync<WrongCredentialsException>(() =>
            _endpoint.GetReport(Url));
    }
}
