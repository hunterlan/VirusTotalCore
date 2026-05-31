using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Domains.Endpoints;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;

namespace VirusTotalCore.Tests.Integration.Domains;

public sealed class GetReportTests : WireMockEndpointTestBase
{
    private const string Domain = "example.com";

    private readonly DomainsEndpoint _endpoint;

    public GetReportTests()
    {
        _endpoint = new DomainsEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task GetReport_HappyPath_ReturnsDeserializedReport()
    {
        Server.StubGetDomainReportSuccess(Domain);

        var report = await _endpoint.GetReport(Domain);

        Assert.NotNull(report);
        Assert.Equal(Domain, report.Id);
        Assert.Equal("domain", report.Type);
        Assert.NotNull(report.Attributes.LastAnalysisStats);
    }

    [Fact]
    public async Task GetReport_WhenForbidden_ThrowsForbiddenException()
    {
        Server.StubForbidden($"/domains/{Domain}");

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            _endpoint.GetReport(Domain));
    }

    [Fact]
    public async Task GetReport_WhenNotFound_ThrowsNotFoundException()
    {
        Server.StubNotFound($"/domains/{Domain}");

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _endpoint.GetReport(Domain));
    }

    [Fact]
    public async Task GetReport_WhenUnauthorized_ThrowsWrongCredentialsException()
    {
        Server.StubUnauthorized($"/domains/{Domain}");

        await Assert.ThrowsAsync<WrongCredentialsException>(() =>
            _endpoint.GetReport(Domain));
    }
}
