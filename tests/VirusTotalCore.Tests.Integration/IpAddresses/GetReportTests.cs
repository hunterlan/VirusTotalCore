using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.IpAddresses.Endpoints;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;

namespace VirusTotalCore.Tests.Integration.IpAddresses;

public sealed class GetReportTests : WireMockEndpointTestBase
{
    private const string IpAddress = "8.8.8.8";

    private readonly AddressIpEndpoint _endpoint;

    public GetReportTests()
    {
        _endpoint = new AddressIpEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task GetReport_HappyPath_ReturnsDeserializedReport()
    {
        Server.StubGetIpReportSuccess(IpAddress);

        var report = await _endpoint.GetReport(IpAddress);

        Assert.NotNull(report);
        Assert.Equal(IpAddress, report.Id);
        Assert.Equal("ip_address", report.Type);
        Assert.NotNull(report.Attributes.LastAnalysisStats);
    }

    [Fact]
    public async Task GetReport_WhenForbidden_ThrowsForbiddenException()
    {
        Server.StubForbidden($"/ip_addresses/{IpAddress}");

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            _endpoint.GetReport(IpAddress));
    }

    [Fact]
    public async Task GetReport_WhenNotFound_ThrowsNotFoundException()
    {
        Server.StubNotFound($"/ip_addresses/{IpAddress}");

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _endpoint.GetReport(IpAddress));
    }

    [Fact]
    public async Task GetReport_WhenUnauthorized_ThrowsWrongCredentialsException()
    {
        Server.StubUnauthorized($"/ip_addresses/{IpAddress}");

        await Assert.ThrowsAsync<WrongCredentialsException>(() =>
            _endpoint.GetReport(IpAddress));
    }
}
