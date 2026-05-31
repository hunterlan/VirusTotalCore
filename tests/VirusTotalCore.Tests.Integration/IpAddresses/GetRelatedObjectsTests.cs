using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.IpAddresses.Endpoints;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;

namespace VirusTotalCore.Tests.Integration.IpAddresses;

public sealed class GetRelatedObjectsTests : WireMockEndpointTestBase
{
    private const string IpAddress = "8.8.8.8";
    private const string Relationship = "resolutions";

    private readonly AddressIpEndpoint _endpoint;

    public GetRelatedObjectsTests()
    {
        _endpoint = new AddressIpEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task GetRelatedObjects_WithoutCursor_QueryStringHasLimitOnly()
    {
        Server.StubGetIpRelatedObjectsSuccess(IpAddress, Relationship);

        await _endpoint.GetRelatedObjects(IpAddress, Relationship, null);

        var entry = Server.LogEntries.Last();
        Assert.Contains("limit=10", entry.RequestMessage!.Url!);
        Assert.DoesNotContain("cursor", entry.RequestMessage!.Url!);
    }

    [Fact]
    public async Task GetRelatedObjects_WithCursor_QueryStringContainsCursor()
    {
        const string cursor = "next-page-cursor";
        Server.StubGetIpRelatedObjectsSuccess(IpAddress, Relationship);

        await _endpoint.GetRelatedObjects(IpAddress, Relationship, cursor);

        var entry = Server.LogEntries.Last();
        Assert.Contains("limit=10", entry.RequestMessage!.Url!);
        Assert.Contains($"cursor={cursor}", entry.RequestMessage!.Url!);
    }

    [Fact]
    public async Task GetRelatedObjects_WhenNotFound_ThrowsNotFoundException()
    {
        Server.StubNotFound($"/ip_addresses/{IpAddress}/{Relationship}");

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _endpoint.GetRelatedObjects(IpAddress, Relationship, null));
    }
}
