using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;
using VirusTotalCore.Urls.Endpoints;

namespace VirusTotalCore.Tests.Integration.Urls;

public sealed class GetRelatedObjectsTests : WireMockEndpointTestBase
{
    private const string UrlId = "abc123urlid";
    private const string Relationship = "analyses";

    private readonly UrlEndpoint _endpoint;

    public GetRelatedObjectsTests()
    {
        _endpoint = new UrlEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task GetRelatedObjects_WithoutCursor_QueryStringHasLimitOnly()
    {
        Server.StubGetUrlRelatedObjectsSuccess(UrlId, Relationship);

        await _endpoint.GetRelatedObjects(UrlId, Relationship, null);

        var entry = Server.LogEntries.Last();
        Assert.Contains("limit=10", entry.RequestMessage!.Url!);
        Assert.DoesNotContain("cursor", entry.RequestMessage!.Url!);
    }

    [Fact]
    public async Task GetRelatedObjects_WithCursor_QueryStringContainsCursor()
    {
        const string cursor = "next-page-cursor";
        Server.StubGetUrlRelatedObjectsSuccess(UrlId, Relationship);

        await _endpoint.GetRelatedObjects(UrlId, Relationship, cursor);

        var entry = Server.LogEntries.Last();
        Assert.Contains("limit=10", entry.RequestMessage!.Url!);
        Assert.Contains($"cursor={cursor}", entry.RequestMessage!.Url!);
    }

    [Fact]
    public async Task GetRelatedObjects_WhenNotFound_ThrowsNotFoundException()
    {
        Server.StubNotFound($"/urls/{UrlId}/{Relationship}");

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _endpoint.GetRelatedObjects(UrlId, Relationship, null));
    }
}
