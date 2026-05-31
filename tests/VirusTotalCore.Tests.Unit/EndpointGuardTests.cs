using VirusTotalCore.IpAddresses.Endpoints;

namespace VirusTotalCore.Tests.Unit;

public sealed class EndpointGuardTests
{
    [Fact]
    public void IncorrectApiKeyAssign()
    {
        Assert.Throws<ArgumentException>(() => new AddressIpEndpoint(""));
    }
}
