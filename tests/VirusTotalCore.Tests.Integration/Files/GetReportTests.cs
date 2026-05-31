using VirusTotalCore.Tests.Integration.TestData.Files;

namespace VirusTotalCore.Tests.Integration.Files;

public sealed class GetReportTests : FilesEndpointTestBase
{
    private const string ReportPath = $"/api/v3/files/{TEST_HASH}";

    public GetReportTests(FilesEndpointFixture fixture) : base(fixture) { }

    [Fact]
    public async Task GetReport_WhenSuccessful_ReturnsDeserializedReport()
    {
        Server
            .Given(Request.Create().WithPath(ReportPath).UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBody(FileReportTestData.ReportJson));

        var report = await Endpoint.GetReport(TEST_HASH);

        Assert.NotNull(report);
        Assert.Equal(TEST_HASH, report.Id);
        Assert.Equal("file", report.Type);
        Assert.NotNull(report.Attributes);
        Assert.Equal("ASCII text", report.Attributes.TypeDescription);
        Assert.Equal(TEST_HASH, report.Attributes.Sha256);
        Assert.Equal("test.txt", report.Attributes.MeaningfulName);
    }

    [Fact]
    public async Task GetReport_WhenNotFound_ThrowsNotFoundException()
    {
        Server
            .Given(Request.Create().WithPath(ReportPath).UsingGet())
            .RespondWith(WireMockErrorResponses.NotFound());

        await Assert.ThrowsAsync<NotFoundException>(() => Endpoint.GetReport(TEST_HASH));
    }

    [Fact]
    public async Task GetReport_WhenForbidden_ThrowsForbiddenException()
    {
        Server
            .Given(Request.Create().WithPath(ReportPath).UsingGet())
            .RespondWith(WireMockErrorResponses.Forbidden());

        await Assert.ThrowsAsync<ForbiddenException>(() => Endpoint.GetReport(TEST_HASH));
    }
}
