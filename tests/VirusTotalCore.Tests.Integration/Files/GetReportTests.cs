using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Files.Endpoints;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;

namespace VirusTotalCore.Tests.Integration.Files;

public sealed class GetReportTests : WireMockEndpointTestBase
{
    private const string FileHash = "275a021bbfb6489e54d471899f7db9d1663fc695ec2fe2a2c4538aabf651fd0f";

    private readonly FilesEndpoint _endpoint;

    public GetReportTests()
    {
        _endpoint = new FilesEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task GetReport_HappyPath_ReturnsDeserializedReport()
    {
        Server.StubGetReportSuccess(FileHash);

        var report = await _endpoint.GetReport(FileHash);

        Assert.NotNull(report);
        Assert.Equal(FileHash, report.Id);
        Assert.Equal("file", report.Type);
        Assert.Equal(FileHash, report.Attributes.Sha256);
    }

    [Fact]
    public async Task GetReport_WhenNotFound_ThrowsNotFoundException()
    {
        Server.StubNotFound($"/files/{FileHash}");

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _endpoint.GetReport(FileHash));
    }

    [Fact]
    public async Task GetReport_WhenForbidden_ThrowsForbiddenException()
    {
        Server.StubForbidden($"/files/{FileHash}");

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            _endpoint.GetReport(FileHash));
    }
}
