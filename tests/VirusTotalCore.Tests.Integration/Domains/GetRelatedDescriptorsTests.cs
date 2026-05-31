using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.Domains.Endpoints;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;

namespace VirusTotalCore.Tests.Integration.Domains;

public sealed class GetRelatedDescriptorsTests : WireMockEndpointTestBase
{
    private const string Domain = "example.com";
    private const string Relationship = "subdomains";

    private readonly DomainsEndpoint _endpoint;

    public GetRelatedDescriptorsTests()
    {
        _endpoint = new DomainsEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task GetRelatedDescriptors_WithoutCursor_QueryStringHasLimitOnly()
    {
        Server.StubGetDomainRelatedDescriptorsSuccess(Domain, Relationship);

        await _endpoint.GetRelatedDescriptors(Domain, Relationship, null);

        var entry = Server.LogEntries.Last();
        Assert.Contains("limit=10", entry.RequestMessage!.Url!);
        Assert.DoesNotContain("cursor", entry.RequestMessage!.Url!);
    }

    [Fact]
    public async Task GetRelatedDescriptors_WithCursor_QueryStringContainsCursor()
    {
        const string cursor = "next-page-cursor";
        Server.StubGetDomainRelatedDescriptorsSuccess(Domain, Relationship);

        await _endpoint.GetRelatedDescriptors(Domain, Relationship, cursor);

        var entry = Server.LogEntries.Last();
        Assert.Contains("limit=10", entry.RequestMessage!.Url!);
        Assert.Contains($"cursor={cursor}", entry.RequestMessage!.Url!);
    }

    [Fact]
    public async Task GetRelatedDescriptors_WhenNotFound_ThrowsNotFoundException()
    {
        Server.StubNotFound($"/domains/{Domain}/relationships/{Relationship}");

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _endpoint.GetRelatedDescriptors(Domain, Relationship, null));
    }
}
