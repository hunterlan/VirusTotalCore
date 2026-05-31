using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Domains.Endpoints;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;

namespace VirusTotalCore.Tests.Integration.Domains;

public sealed class GetRelatedObjectsTests : WireMockEndpointTestBase
{
    private const string Domain = "example.com";
    private const string Relationship = "subdomains";

    private readonly DomainsEndpoint _endpoint;

    public GetRelatedObjectsTests()
    {
        _endpoint = new DomainsEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task GetRelatedObjects_WithoutCursor_QueryStringHasLimitOnly()
    {
        Server.StubGetDomainRelatedObjectsSuccess(Domain, Relationship);

        await _endpoint.GetRelatedObjects(Domain, Relationship, null);

        var entry = Server.LogEntries.Last();
        Assert.Contains("limit=10", entry.RequestMessage!.Url!);
        Assert.DoesNotContain("cursor", entry.RequestMessage!.Url!);
    }

    [Fact]
    public async Task GetRelatedObjects_WithCursor_QueryStringContainsCursor()
    {
        const string cursor = "next-page-cursor";
        Server.StubGetDomainRelatedObjectsSuccess(Domain, Relationship);

        await _endpoint.GetRelatedObjects(Domain, Relationship, cursor);

        var entry = Server.LogEntries.Last();
        Assert.Contains("limit=10", entry.RequestMessage!.Url!);
        Assert.Contains($"cursor={cursor}", entry.RequestMessage!.Url!);
    }

    [Fact]
    public async Task GetRelatedObjects_WhenNotFound_ThrowsNotFoundException()
    {
        Server.StubNotFound($"/domains/{Domain}/{Relationship}");

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _endpoint.GetRelatedObjects(Domain, Relationship, null));
    }
}
