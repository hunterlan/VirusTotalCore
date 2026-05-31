using System.Text.Json;
using VirusTotalCore.Common.Enums;
using VirusTotalCore.Common.Exceptions;
using VirusTotalCore.IpAddresses.Endpoints;
using VirusTotalCore.Tests.Integration.Helpers;
using VirusTotalCore.Tests.Integration.Stubs;

namespace VirusTotalCore.Tests.Integration.IpAddresses;

public sealed class AddVoteTests : WireMockEndpointTestBase
{
    private const string IpAddress = "8.8.8.8";

    private readonly AddressIpEndpoint _endpoint;

    public AddVoteTests()
    {
        _endpoint = new AddressIpEndpoint(CreateHttpClient());
    }

    [Fact]
    public async Task AddVote_HarmlessVerdict_RequestBodyContainsVerdict()
    {
        Server.StubAddIpVoteSuccess(IpAddress);

        await _endpoint.AddVote(IpAddress, VerdictType.Harmless);

        var entry = Server.LogEntries.Last();
        var body = JsonDocument.Parse(entry.RequestMessage!.Body!);
        var verdict = body.RootElement
            .GetProperty("data")
            .GetProperty("attributes")
            .GetProperty("verdict")
            .GetString();
        Assert.Equal("harmless", verdict);
    }

    [Fact]
    public async Task AddVote_WhenForbidden_ThrowsForbiddenException()
    {
        Server.StubForbidden($"/ip_addresses/{IpAddress}/votes", "POST");

        await Assert.ThrowsAsync<ForbiddenException>(() =>
            _endpoint.AddVote(IpAddress, VerdictType.Harmless));
    }

    [Fact]
    public async Task AddVote_WhenNotFound_ThrowsNotFoundException()
    {
        Server.StubNotFound($"/ip_addresses/{IpAddress}/votes", "POST");

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _endpoint.AddVote(IpAddress, VerdictType.Harmless));
    }
}
