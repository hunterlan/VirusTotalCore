using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Files.Endpoints;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;

namespace VirusTotalCore.Tests.Integration.Files;

public sealed class GetRelatedObjectsTests : WireMockEndpointTestBase
{
    private const string FileHash = "275a021bbfb6489e54d471899f7db9d1663fc695ec2fe2a2c4538aabf651fd0f";
    private const string Relationship = "graphs";

    private readonly FilesEndpoint _endpoint;

    public GetRelatedObjectsTests()
    {
        _endpoint = new FilesEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task GetRelatedObjects_WithoutCursor_QueryStringHasLimitOnly()
    {
        Server.StubGetRelatedSuccess(FileHash, Relationship);

        await _endpoint.GetRelatedObjects(FileHash, Relationship, null);

        var entry = Server.LogEntries.Last();
        Assert.Contains("limit=10", entry.RequestMessage!.Url!);
        Assert.DoesNotContain("cursor", entry.RequestMessage!.Url!);
    }

    [Fact]
    public async Task GetRelatedObjects_WithCursor_QueryStringContainsCursor()
    {
        const string cursor = "next-page-cursor";
        Server.StubGetRelatedSuccess(FileHash, Relationship);

        await _endpoint.GetRelatedObjects(FileHash, Relationship, cursor);

        var entry = Server.LogEntries.Last();
        Assert.Contains("limit=10", entry.RequestMessage!.Url!);
        Assert.Contains($"cursor={cursor}", entry.RequestMessage!.Url!);
    }

    [Fact]
    public async Task GetRelatedObjects_WhenNotFound_ThrowsNotFoundException()
    {
        Server.StubNotFound($"/files/{FileHash}/{Relationship}");

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _endpoint.GetRelatedObjects(FileHash, Relationship, null));
    }
}
