using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.IpAddresses.Endpoints;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;

namespace VirusTotalCore.Tests.Integration.IpAddresses;

public sealed class GetRelatedDescriptorsTests : WireMockEndpointTestBase
{
    private const string IpAddress = "8.8.8.8";
    private const string Relationship = "resolutions";

    private readonly AddressIpEndpoint _endpoint;

    public GetRelatedDescriptorsTests()
    {
        _endpoint = new AddressIpEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task GetRelatedDescriptors_WithoutCursor_QueryStringHasLimitOnly()
    {
        Server.StubGetIpRelatedDescriptorsSuccess(IpAddress, Relationship);

        await _endpoint.GetRelatedDescriptors(IpAddress, Relationship, null);

        var entry = Server.LogEntries.Last();
        Assert.Contains("limit=10", entry.RequestMessage!.Url!);
        Assert.DoesNotContain("cursor", entry.RequestMessage!.Url!);
    }

    [Fact]
    public async Task GetRelatedDescriptors_WithCursor_QueryStringContainsCursor()
    {
        const string cursor = "next-page-cursor";
        Server.StubGetIpRelatedDescriptorsSuccess(IpAddress, Relationship);

        await _endpoint.GetRelatedDescriptors(IpAddress, Relationship, cursor);

        var entry = Server.LogEntries.Last();
        Assert.Contains("limit=10", entry.RequestMessage!.Url!);
        Assert.Contains($"cursor={cursor}", entry.RequestMessage!.Url!);
    }

    [Fact]
    public async Task GetRelatedDescriptors_WhenNotFound_ThrowsNotFoundException()
    {
        Server.StubNotFound($"/ip_addresses/{IpAddress}/relationships/{Relationship}");

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _endpoint.GetRelatedDescriptors(IpAddress, Relationship, null));
    }
}
